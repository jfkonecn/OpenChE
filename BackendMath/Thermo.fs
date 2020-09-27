namespace EngineeringMath

open NumericalMethods.FiniteDifferenceFormulas
open System.Threading.Tasks
open Microsoft.FSharp.Data.UnitSystems.SI.UnitSymbols
open NumericalMethods
open EngineeringMath.Common


type ValidatedBasicPtvEntryQuery = 
    |PtQuery of float<Pressure> * float<Temperature>
    |SatTempQuery of float<Temperature> * ValidatedPureRegion 
    |SatPreQuery of float<Pressure> * ValidatedPureRegion 

type ValidatedPtvEntryQuery = 
    |BasicPtvEntryQuery of ValidatedBasicPtvEntryQuery
    |EnthalpyQuery of float<Enthalpy> * float<Pressure>
    |EntropyQuery of float<Entropy> * float<Pressure>

type ValidatedIsentropicExpansionQuery = { inletVaporT:float<Temperature>; inletVaporP:float<Pressure>; outletP:float<Pressure> }
type ValidatedIsentropicExpansionResult = { 
    vaporPtv:ValidatedPtvEntry; 
    wetVaporPtv:ValidatedPtvEntry; 
    satVapor:ValidatedPtvEntry; 
    satLiquid:ValidatedPtvEntry; 
    vaporQuality:Fraction }

type PvtTableReader = ValidatedBasicPtvEntryQuery -> Result<ValidatedPtvEntry, DomainError>
type PtvQuery<'a> = PvtTableReader -> Result<'a, DomainError>
type AsyncPtvQuery<'a> = PvtTableReader -> AsyncResult<'a, DomainError>
type PvtQueryHandler<'a> = 
    | PtvQuery of PtvQuery<'a>
    | AsyncPtvQuery of AsyncPtvQuery<'a>

type ValidatedRankineArgs = { 
    boilerT:float<Temperature>; 
    boilerP:float<Pressure>; 
    condenserP:float<Pressure>;
    powerRequirement:float<W>;
    turbineEfficiency:Fraction;
    pumpEfficiency:Fraction; }
type ValidatedCycleResult = {
    boilerPtv:ValidatedPtvEntry; 
    condenserPtv:ValidatedPtvEntry; 
    condenserSteamQuality:Fraction;
    pumpWork:float<J/kg>;
    boilerWork:float<J/kg>;
    turbineWork:float<J/kg>;
    condenserWork:float<J/kg>;
    netWork:float<J/kg>;
    thermalEfficiency:Fraction;
    steamRate:float<kg/s>;
    boilerHeatTransferRate:float<J/s>;
    condenserHeatTransferRate:float<J/s>;
}

type ValidatedRegenerativeCycleArgs = { 
    inletBoilerT:float<K>;
    boilerT:float<Temperature>; 
    boilerP:float<Pressure>; 
    condenserP:float<Pressure>;
    powerRequirement:float<W>;
    stages:int;
    minTemperatureDelta:float<K>;
    turbineEfficiency:Fraction;
    pumpEfficiency:Fraction; }


module Thermo =

    let performPtvEntryQuery (query:ValidatedPtvEntryQuery) (tableReader:PvtTableReader) = 
        let handleNonTableQuery p targetValue getTargetValue =
            let interpolateEntry liquidEntry vaporEntry liqFrac =
                let vaporFrac = 1.0 - liqFrac
                let interpolateEntryProperty getProperty =
                    (getProperty(liquidEntry) * liqFrac) + (getProperty(vaporEntry) * vaporFrac)
                match (ValidatedPhaseInfo.create "phaseInfo" ValidatedPhaseRegion.LiquidVapor vaporFrac liqFrac 0.0) with
                | (Ok info) -> Ok { 
                    ValidatedPtvEntry.P = (interpolateEntryProperty (fun x -> float x.P)) * 1.0<Pressure>;
                    T = (interpolateEntryProperty (fun x -> float x.T)) * 1.0<Temperature>;
                    PhaseInfo = info;
                    U = (interpolateEntryProperty (fun x -> float x.U)) * 1.0<J / kg>;
                    H = (interpolateEntryProperty (fun x -> float x.H)) * 1.0<Enthalpy>;
                    S = (interpolateEntryProperty (fun x -> float x.S)) * 1.0<Entropy>;
                    Cv = (interpolateEntryProperty (fun x -> float x.Cv)) * 1.0<IsochoricHeatCapacity>;
                    Cp = (interpolateEntryProperty (fun x -> float x.Cp)) * 1.0<IsobaricHeatCapacity>;
                    SpeedOfSound = (interpolateEntryProperty (fun x -> float x.SpeedOfSound)) * 1.0<Speed>;
                    V = 1.0<SpecificVolume> / (interpolateEntryProperty (fun x -> float x.Rho));
                    Rho = (interpolateEntryProperty (fun x -> float x.Rho)) * 1.0<Density>; }
                | Error _ -> Error DomainError.OutOfRange 


            let liqEntry =  tableReader (ValidatedBasicPtvEntryQuery.SatPreQuery (p, ValidatedPureRegion.Liquid))
            let vaporEntry = tableReader (ValidatedBasicPtvEntryQuery.SatPreQuery (p, ValidatedPureRegion.Vapor))
            let resolve liq vap =
                let mergeLiqVapor =
                    let liqFrac = (getTargetValue(vap) - targetValue) / (getTargetValue(vap) - getTargetValue(liq))
                    interpolateEntry liq vap liqFrac
                match (getTargetValue(liq), getTargetValue(vap)) with
                | (liqTarget, vapTarget) when vapTarget >= targetValue && liqTarget <= targetValue -> mergeLiqVapor
                | _ -> Error DomainError.OutOfRange
            let (<!>) = Result.map
            let (<*>) = Result.apply
            result {
                let satPointResult = resolve <!> liqEntry <*> vaporEntry
                let tryAssumingNotSaturated() =
                    let solver t =
                        let x = match tableReader (ValidatedBasicPtvEntryQuery.PtQuery (p, (t * 1.0<Temperature>))) with
                                |Ok x -> (getTargetValue x)
                                |Error _ -> float nan
                        x - targetValue

                    match NumericalMethods.Zeros.newton solver 300.0 with
                        |Ok t -> tableReader (ValidatedBasicPtvEntryQuery.PtQuery (p, t * 1.0<Temperature>))
                        |Error ConvergeError.MaxIterationsReached -> Error DomainError.FailedToConverge
                return! match satPointResult with
                        | Ok (Ok x) -> Ok x
                        | _ -> tryAssumingNotSaturated()
            }
        match query with
        | (ValidatedPtvEntryQuery.BasicPtvEntryQuery q) -> tableReader q 
        | (ValidatedPtvEntryQuery.EnthalpyQuery (e, p)) -> (handleNonTableQuery p (float e) (fun x -> float x.H))
        | (ValidatedPtvEntryQuery.EntropyQuery (e, p)) -> (handleNonTableQuery p (float e) (fun x -> float x.S))


    let isentropicExpansion (query:ValidatedIsentropicExpansionQuery) tableReader =
        asyncResult {
            let vaporTask = Task.Run(fun () -> 
                    result {
                    let! inVapor = tableReader (PtQuery (query.inletVaporP, query.inletVaporT))
                    let! pressureSample = performPtvEntryQuery (EntropyQuery (inVapor.S, query.outletP)) tableReader 
                    return (inVapor, pressureSample)
                    }
                )

            let satVaporTask = Task.Run(fun () -> tableReader (SatPreQuery (query.outletP, ValidatedPureRegion.Vapor)))
            let satLiqTask = Task.Run(fun () -> tableReader (SatPreQuery (query.outletP, ValidatedPureRegion.Liquid)))
        

            let! _ = Task.WhenAll(satVaporTask, satLiqTask, vaporTask) |> Async.AwaitTask |> AsyncResult.ofAsync

            let calculatePtvPoint (inVapor, sampleP, satVapor, satLiq) =
                let fieldName = "vapor quality"



                let createWetVaporPoint (vapQuality:VaporQuality) =
                    let vapQualityValue = VaporQuality.value vapQuality
                    let liqQuality = 1.0 - vapQualityValue
                    let phaseInfoResult = ValidatedPhaseInfo.create "PhaseInfo" LiquidVapor vapQualityValue liqQuality 0.0
                    let interpolate (liq:float<'a>) (vap:float<'a>) =
                        liq * liqQuality + vap * vapQualityValue
                    match phaseInfoResult with 
                    | Ok phaseInfo ->
                        Ok { ValidatedPtvEntry.P = (interpolate satLiq.P satVapor.P);
                             T = (interpolate satLiq.T satVapor.T);
                             PhaseInfo =  phaseInfo;
                             U = (interpolate satLiq.U satVapor.U);
                             H = (interpolate satLiq.H satVapor.H);
                             S = (interpolate satLiq.S satVapor.S);
                             Cv = (interpolate satLiq.Cv satVapor.Cv);
                             Cp = (interpolate satLiq.Cp satVapor.Cp);
                             SpeedOfSound = (interpolate satLiq.SpeedOfSound satVapor.SpeedOfSound);
                             V = (interpolate satLiq.V satVapor.V);
                             Rho = (interpolate satLiq.Rho satVapor.Rho); }
                    | Error _ -> Error DomainError.OutOfRange


                match (sampleP.S, satVapor.S) with
                | (samS, vapS) when samS >= vapS -> 
                    let quality = VaporQuality.create fieldName 1.0
                    match quality with
                    | Ok quality -> Ok (sampleP, quality)
                    | Error _ -> Error DomainError.OutOfRange
                | _ -> 
                    let quality = VaporQuality.create fieldName ((inVapor.S - satLiq.S) / (satVapor.S - satLiq.S))
                    match quality with
                    | Ok quality -> ((createWetVaporPoint quality |> Result.map (fun x -> (x, quality))))
                    | Error _ -> Error DomainError.OutOfRange
                |> Result.map (fun (wetPtv, q) -> { vaporPtv= inVapor; 
                                                    wetVaporPtv= wetPtv; 
                                                    satVapor= satVapor; 
                                                    satLiquid= satLiq; 
                                                    vaporQuality= VaporQuality.fractionValue q})


            return! match (vaporTask.Result, satVaporTask.Result, satLiqTask.Result) with
                    | (Ok (inVapor, sampleP), Ok satVapor, Ok satLiq) -> Ok (inVapor, sampleP, satVapor, satLiq)
                    | (Error e, _, _) 
                    | (_, Error e, _) 
                    | (_, _, Error e) -> Error e
                    |> Result.bind calculatePtvPoint
                    |> AsyncResult.ofResult
        }

    let rankineCycle (args:ValidatedRankineArgs) (tableReader:PvtTableReader) = 
        asyncResult {
            let pumpEff = Fraction.value args.pumpEfficiency 
            let turbineEff = Fraction.value args.turbineEfficiency
            let! x = (isentropicExpansion {inletVaporT=args.boilerT; inletVaporP=args.boilerP; outletP=args.condenserP} tableReader)
            let b = x.vaporPtv
            let c = x.wetVaporPtv
            let cLiq = x.satLiquid
            let pumpWork = ((cLiq.V * (b.P - c.P))) / pumpEff;
            let bWork = b.H - (cLiq.H + pumpWork)
            let tWork = (b.H - c.H) * turbineEff
            let cWork = (b.H - tWork) - cLiq.H
            let netWork = tWork - pumpWork
            let sRate = args.powerRequirement / netWork
            let! thermalEff = (Fraction.create "" (abs(tWork) / bWork)) 
                              |> Result.mapError (fun _ -> DomainError.OutOfRange) 
                              |> AsyncResult.ofResult
            return {
                boilerPtv= b; 
                condenserPtv= c; 
                condenserSteamQuality= x.vaporQuality;
                pumpWork= pumpWork;
                boilerWork= bWork;
                turbineWork= tWork;
                condenserWork = cWork;
                netWork= netWork;
                thermalEfficiency= thermalEff;
                steamRate= sRate;
                boilerHeatTransferRate= sRate * bWork;
                condenserHeatTransferRate= sRate * cWork;
            }
        }       


    type private PtvMassBasis = ValidatedPtvEntry * float<kg/s>
    type private RegenerativeStage = {
        ExitVaporBasis:PtvMassBasis
        ExitCondensateBasis:PtvMassBasis
        ExitCoolingLiquid:ValidatedPtvEntry
        CondenserConditions:ValidatedPtvEntry
        TotalWork:float<Enthalpy>
        VaporQuality:Fraction
    }
    let regenerativeCycle (args:ValidatedRegenerativeCycleArgs) (tableReader:PvtTableReader):AsyncResult<ValidatedCycleResult,DomainError> =
        asyncResult {

            let boilerPtvTask = Task.Run(fun () -> tableReader (PtQuery (args.boilerP, args.boilerT)))
            let condenserTask = Task.Run(fun () -> 
                result {
                    let! condenserPtv = tableReader (SatPreQuery (args.condenserP, ValidatedPureRegion.Liquid))
                    let! pumpPtv = tableReader (SatTempQuery (condenserPtv.T, ValidatedPureRegion.Liquid))
                    let step = 0.5<Temperature>
                    let! imin1 = tableReader (PtQuery (condenserPtv.P, condenserPtv.T - step))
                    let! imin2 = tableReader (PtQuery (condenserPtv.P, condenserPtv.T - (2.0 * step)))
                    let dVdT = FirstDerivative(ThreePointBackward {i = condenserPtv.V; iMinus1=imin1.V; iMinus2=imin2.V; step=step})
                    let thermalExpansion = dVdT / condenserPtv.V
                    return (condenserPtv, pumpPtv, thermalExpansion)
                })
            let _ = Task.WhenAll(boilerPtvTask, condenserTask) |> Async.AwaitTask |> AsyncResult.ofAsync
            let! boilerPtv = AsyncResult.ofResult boilerPtvTask.Result
            let! (condenserLiquidPtv, pumpPtv, thermalExpansion) = AsyncResult.ofResult condenserTask.Result
            let pumpOutT = (((pumpPtv.V  / (Fraction.value args.pumpEfficiency)) - (pumpPtv.V * (1.0 - thermalExpansion * pumpPtv.T))) / pumpPtv.Cp) * (boilerPtv.P - pumpPtv.P) + pumpPtv.T

            let stageTempDelta = (args.inletBoilerT - pumpOutT) / float (args.stages - 1)

            let rec handleCycles (vapor:PtvMassBasis, coolingLiq:PtvMassBasis, condensate:Option<PtvMassBasis>, totalWork) (stage:int) =
                            
                
                let (inletVapor, inVaporBasis) = vapor
                let (inletCoolingLiquid, inletCoolingLiquidBasis) = coolingLiq

                let calculateStageProperties () =
                    asyncResult {
                        let! exitCoolingLiquid = AsyncResult.ofResult (tableReader (PtQuery (inletCoolingLiquid.P, inletCoolingLiquid.T + stageTempDelta)))
                        let! exitCondensate = AsyncResult.ofResult (tableReader (SatTempQuery (exitCoolingLiquid.T + args.minTemperatureDelta, ValidatedPureRegion.Liquid)))
                        let! isenExpansion = (isentropicExpansion {inletVaporT=inletVapor.T; inletVaporP=inletVapor.P; outletP=exitCondensate.P} tableReader)
                        let workProduced = (inletVapor.H - isenExpansion.wetVaporPtv.H) * (Fraction.value args.turbineEfficiency)
                        let! exitVapor = AsyncResult.ofResult (performPtvEntryQuery (EnthalpyQuery (inletVapor.H - workProduced, exitCondensate.P)) tableReader )
                        let (inletCondensateBasis, condensateCreated) = match condensate with
                                                                        | Some (inletCondensate, inletCondensateBasis) ->
                                                                            (inletCondensateBasis, 
                                                                                (inletCondensateBasis * (inletCondensate.H - exitCondensate.H) 
                                                                                    - inletCoolingLiquidBasis * (exitCoolingLiquid.H - inletCoolingLiquid.H))
                                                                                / (exitCondensate.H - inletVapor.H))
                                                                        | None ->
                                                                            (0.0<kg/s>, 
                                                                                (inletCoolingLiquidBasis * (exitCoolingLiquid.H - inletCoolingLiquid.H))
                                                                                / (exitVapor.H - exitCondensate.H))
                        let outVaporBasis = inVaporBasis - condensateCreated
                        let outCondensateBasis = condensateCreated + inletCondensateBasis
                        return {
                            ExitVaporBasis = (exitVapor, outVaporBasis);
                            ExitCondensateBasis = (exitCondensate, outCondensateBasis);
                            ExitCoolingLiquid = exitCoolingLiquid;
                            CondenserConditions = isenExpansion.wetVaporPtv;
                            TotalWork = (totalWork + workProduced);
                            VaporQuality = isenExpansion.vaporQuality;
                        }
                    }


                match (stage, condensate) with
                | _ when stage < args.stages ->
                    asyncResult {
                        let! result = calculateStageProperties()
                        let (exitCondensate, outCondensateBasis) = result.ExitCondensateBasis
                        let (exitVapor, outVaporBasis) = result.ExitVaporBasis
                        let exitCoolingLiquid = result.ExitCoolingLiquid
                        let totalWork = result.TotalWork
                        let! nextStageCoolingLiquid = AsyncResult.ofResult (tableReader (PtQuery (exitCoolingLiquid.P, exitCoolingLiquid.T - (2.0 * stageTempDelta))))
                        return! handleCycles ((exitVapor, outVaporBasis), (nextStageCoolingLiquid, inletCoolingLiquidBasis), Some (exitCondensate, outCondensateBasis), totalWork) (stage + 1)
                    }
                | (_, Some _) when stage = args.stages -> 
                    asyncResult {
                        let! result = calculateStageProperties()
                        return (result.TotalWork, result.VaporQuality, result.CondenserConditions)
                    }
                | _ -> AsyncResult.ofError DomainError.OutOfRange

            
            let! coolingLiquid = AsyncResult.ofResult (tableReader (PtQuery (args.boilerP, args.inletBoilerT - stageTempDelta)))
            let! (turbineWork, vaporQuality, condenserPtv) = handleCycles ((boilerPtv, 1.0<kg/s>), (coolingLiquid, 1.0<kg/s>), None, 0.0<Enthalpy>) (1)
            let pumpWork = (condenserLiquidPtv.V * (boilerPtv.P - condenserLiquidPtv.P)) / (Fraction.value args.pumpEfficiency);
            let boilerWork = (boilerPtv.H - coolingLiquid.H);
            let condenserWork = boilerWork - turbineWork
            let netWork = turbineWork - pumpWork
            let steamRate = args.powerRequirement / netWork
            let! thermalEff = (Fraction.create "" (abs(turbineWork) / boilerWork)) 
                              |> Result.mapError (fun _ -> DomainError.OutOfRange) 
                              |> AsyncResult.ofResult
            return {
                boilerPtv= boilerPtv; 
                condenserPtv= condenserPtv; 
                condenserSteamQuality= vaporQuality;
                pumpWork= pumpWork;
                boilerWork= boilerWork;
                turbineWork= turbineWork;
                condenserWork = condenserWork;
                netWork= netWork;
                thermalEfficiency= thermalEff;
                steamRate= steamRate;
                boilerHeatTransferRate= steamRate * boilerWork;
                condenserHeatTransferRate= steamRate * condenserWork;
            }
        }

      
