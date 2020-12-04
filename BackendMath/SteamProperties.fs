namespace EngineeringMath
open EngineeringMath.AssetFiles
open EngineeringMath.Common
open FSharp.Data
open Microsoft.FSharp.Data.UnitSystems.SI.UnitSymbols
open System.Threading.Tasks
open NumericalMethods
open Thermo

module SteamProperties =

    type private SpecificRegionPoint = private {
        T: float<K>
        P: float<Pa>
        Tau: float
        Pi: float
        Gamma: float
        GammaPi: float
        GammaPiPi: float
        GammaTau: float
        GammaTauTau: float
        GammaPiTau: float
    }
    
    type private SpecificRegion3Point = private {
        T: float<K>
        P: float<Pa>
        Tau: float
        Delta: float
        Phi: float
        PhiDelta: float
        PhiDeltaDelta: float
        PhiTau: float
        PhiTauTau: float
        PhiDeltaTau: float
    }
    
    
    type private IPoint = I of float
    type private JPoint = J of float
    type private NPoint = N of float
    
    
    type private VaporRegionPoints = Points of seq<JPoint * NPoint> * seq<IPoint * JPoint * NPoint>
    
    type private Region1And4Point = IJNPoint of IPoint * JPoint * NPoint
    type private Region2IdealPoint = JNPoint of JPoint * NPoint
    type private Region2ResidualPoint = IJNPoint of IPoint * JPoint * NPoint
    
    type private Region3Points = Points of NPoint * seq<IPoint * JPoint * NPoint>
    
    type private Region34BoundaryPoint = NPoint of NPoint
    type private Region4Point = NPoint of NPoint
    type private Region5IdealPoint = JNPoint of JPoint * NPoint
    type private Region5ResidualPoint = IJNPoint of IPoint * JPoint * NPoint
    type private AllRegionPoints = AllPoints of seq<Region1And4Point> * 
                                                seq<Region2IdealPoint> * 
                                                seq<Region2ResidualPoint> * 
                                                Region3Points *
                                                seq<Region34BoundaryPoint> *
                                                seq<Region4Point> *
                                                seq<Region5IdealPoint> * 
                                                seq<Region5ResidualPoint>

    type private SteamEquationRegion = private Region1|Region2|Region3|Region4|Region5
    type private PtPoint = private { pressure:float<Pressure>; temperature:float<Temperature> }
   

    module private VaporRegionPoints =
        let value (VaporRegionPoints.Points (iPoints, resPoints)) = 
            (iPoints, resPoints)
    
        let create fi iPoints fr resPoints  =
            let t = Seq.map fi iPoints
            VaporRegionPoints.Points (Seq.map fi iPoints, Seq.map fr resPoints)
    let criticalTemperature = 647.096<K>
    let criticalPressure = 22.06e6<Pa>
    let waterGasConstant = 461.526<J / (kg * K)>

    let private regionPoints =
        let loadRegionPoints fileName mapper =
            let mapRegionCsv fileContents = 
                let rowToPoint (r:CsvRow) = 
                    let getValue (name:string) =
                        try 
                            double r.[name]
                        with
                            | _ -> double nan
                    ( I (getValue "I"), 
                      J (getValue "J"), 
                      N (getValue "N"))

                CsvFile.Parse(fileContents, hasHeaders = true)
                |> (fun csv -> csv.Rows)
                |> Seq.map rowToPoint
                |> Seq.map mapper
            let tryParse fileName opt =
                match opt with
                |Some contents -> AsyncResult.ofAsync (Async.map mapRegionCsv contents)
                |None -> AsyncResult.ofError (DomainError.FailToLoadFile fileName)
            getAssetFileContents fileName
            |> tryParse fileName

        


        let mapReg3Points (x) =
            match (Seq.toList x) with
            | (_, _, n0)::tail -> AsyncResult.ofSuccess (Region3Points.Points (n0, (List.toSeq tail)))
            | _ -> AsyncResult.ofError (DomainError.FailToLoadFile "Region3.csv")

        asyncResult {
            let! reg14 = loadRegionPoints "Region1and4.csv" (fun x -> Region1And4Point.IJNPoint x)
            let! regIdeal2 = loadRegionPoints "Region2Ideal.csv" (fun (_, j, n) -> Region2IdealPoint.JNPoint (j, n))
            let! regRes2 = loadRegionPoints "Region2Residual.csv" (fun x -> Region2ResidualPoint.IJNPoint x)
            let! reg3 = loadRegionPoints "Region3.csv" (fun x -> x) |> AsyncResult.bind mapReg3Points
            let! nReg34 = loadRegionPoints "nBoundary34.csv" (fun (_, _, n) -> Region34BoundaryPoint.NPoint n)
            let! nReg4 = loadRegionPoints "nRegion4.csv" (fun (_, _, n) -> Region4Point.NPoint n)
            let! regIdeal5 = loadRegionPoints "Region5Ideal.csv" (fun (_, j, n) -> Region5IdealPoint.JNPoint (j, n))
            let! regRes5 = loadRegionPoints "Region5Residual.csv" (fun x -> Region5ResidualPoint.IJNPoint x)
            return AllRegionPoints.AllPoints (reg14, regIdeal2, regRes2, reg3, nReg34, nReg4, regIdeal5, regRes5)
        }
    let internal performSteamQuery (queryHandler:PvtQueryHandler<'a>) : AsyncResult<'a, DomainError> =

        let resolveRegion (query, nReg34:array<Region34BoundaryPoint>, reg4Points:array<Region4Point>) =
            let getReg4NPoint i = 
                let (Region4Point.NPoint (N n)) = reg4Points.[i]
                n
            
            let getSaturationPressure temperature =
                let calculateSaturationPressure() =
                    let satTempRatio = float temperature / 1.0
                    let theta = satTempRatio + ((getReg4NPoint 8) / (satTempRatio - (getReg4NPoint 9)))
                    let A = theta ** 2.0 + (getReg4NPoint 0) * theta + (getReg4NPoint 1)
                    let B = (getReg4NPoint 2) * (theta ** 2.0) + (getReg4NPoint 3) * theta + (getReg4NPoint 4)
                    let C = (getReg4NPoint 5) * (theta ** 2.0) + (getReg4NPoint 6) * theta + (getReg4NPoint 7)
                    (((2.0 * C) / (-B + (((B ** 2.0) - 4.0 * A * C) ** 0.5))) ** 4.0) * 1e6<Pa>
                
                match temperature with
                    | t when t < criticalTemperature -> Some ((calculateSaturationPressure()))
                    | _ -> None


            let getSaturationTemperature pressure =
                let calculateSaturationTemperature() =
                    let beta = (float pressure / 1e6) ** 0.25
                    let E = (beta ** 2.0) + (getReg4NPoint 2) * beta + (getReg4NPoint 5)
                    let F = (getReg4NPoint 0) * (beta ** 2.0) + (getReg4NPoint 3) * beta + (getReg4NPoint 6)
                    let G = (getReg4NPoint 1) * (beta ** 2.0) + (getReg4NPoint 4) * beta + (getReg4NPoint 7)
                    let D = (2.0 * G) / (-F - (((F ** 2.0) - 4.0 * E * G) ** 0.5))
                    ((getReg4NPoint 9) + D - (((((getReg4NPoint 9) + D) ** 2.0) - 4.0 * ((getReg4NPoint 8) + (getReg4NPoint 9) * D)) ** 0.5)) / 2.0 * 1.0<K>
                
                match pressure with
                    | p when p < criticalPressure -> Some (calculateSaturationTemperature())
                    | _ -> None

            let getBoundary34Pressure temperature =
                let calculateSaturationPressure() =
                    let getReg4NPoint i = 
                        let (Region34BoundaryPoint.NPoint (N n)) = nReg34.[i]
                        n
                    let theta = float temperature / 1.0
                    ((getReg4NPoint 0) + (getReg4NPoint 1) * theta + (getReg4NPoint 2) * (theta ** 2.0)) * 1e6<Pa>
                
                match temperature with
                    | t when t >= criticalTemperature -> Some (calculateSaturationPressure())
                    | _ -> None

            let performNonSaturationQuery p t =
                let satPOpt = getSaturationPressure t
                let b34POpt = getBoundary34Pressure t
                let ptPoint = { PtPoint.pressure = p; temperature = t }
                match (p, t, satPOpt, b34POpt) with
                | (_, t, _, _) when t > (273.15<K> + 800.0<K>) -> Ok (ptPoint, Region5)
                | (_, t, _, _) when t > (273.15<K> + 600.0<K>) -> Ok (ptPoint, Region2)
                | (p, _, Some satP, _) when p > satP -> Ok (ptPoint, Region1)
                | (p, _, Some satP, _) when p < satP -> Ok (ptPoint, Region2)
                | (p, _, Some satP, _) when p = satP -> Ok (ptPoint, Region4)
                | (p, _, _, Some b34P) when p < b34P -> Ok (ptPoint, Region2)
                | (p, _, _, Some b34P) when p >= b34P -> Ok (ptPoint, Region3)
                | _ -> Error DomainError.OutOfRange

            let performSaturationQuery pOpt tOpt region =
                let satPOpt = Option.bind getSaturationPressure tOpt
                let satTOpt = Option.bind getSaturationTemperature pOpt
                let getPtOpt() = 
                    match (pOpt, tOpt, satPOpt, satTOpt) with
                    | (Some p, None, None, Some tSat) -> Some { PtPoint.pressure = p; temperature = tSat }
                    | (None, Some t, Some pSat, None) -> Some { pressure = pSat; temperature = t }
                    | _ -> None
                match (getPtOpt(), region) with
                | (Some point, Liquid) -> Ok (point, Region1)
                | (Some point, Vapor) -> Ok (point, Region2)
                | _ -> Error DomainError.OutOfRange

            match query with
            | ValidatedBasicPtvEntryQuery.PtQuery (p, t) when p > 50e6<Pa> && p <= 100e6<Pa> && t >= 273.15<K> && t <= 800.0<K> + 273.15<K> -> 
                (performNonSaturationQuery p t)
            | ValidatedBasicPtvEntryQuery.PtQuery (p, t) when p >= 0.0<Pa> && p <= 50e6<Pa> && t >= 273.15<K> && t <= 2000.0<K> + 273.15<K> -> 
                (performNonSaturationQuery p t)
            | ValidatedBasicPtvEntryQuery.SatPreQuery (p, region) when p >= 0.0<Pa> && p <= 100e6<Pa> -> 
                (performSaturationQuery (Some p) None region)
            | ValidatedBasicPtvEntryQuery.SatTempQuery (t, region) when t >= 273.15<K> && t <= 2000.0<K> + 273.15<K> -> 
                (performSaturationQuery None (Some t) region)
            | _ -> 
                Error DomainError.OutOfRange

        let createRegionQueryFunction (AllRegionPoints.AllPoints (reg14, regIdeal2, regRes2, reg3, nReg34, nReg4, regIdeal5, regRes5)) =
            let createPvtEntry (p:SpecificRegionPoint, validPhaseInfo) =
                let R = float waterGasConstant
                let phaseInfo = validPhaseInfo
                let speedOfSound =
                    let a = ((p.GammaPi - p.Tau * p.GammaPiTau) ** 2.0)
                    let b = (((p.GammaPi ** 2.0)) / ((a / ((p.Tau ** 2.0) * p.GammaTauTau)) - p.GammaPiPi))
                    sqrt(R * (float p.T) * b) * 1.0<Speed>
                let v = (p.Pi * (p.GammaPi * R * (float p.T)) / (float p.P)) * 1.0<SpecificVolume>
                { ValidatedPtvEntry.P = p.P;
                  T = p.T;
                  PhaseInfo = phaseInfo;
                  U = R * (float p.T) * (p.Tau * p.GammaTau - p.Pi * p.GammaPi) * 1.0<J / kg>;
                  H = R * (float p.T) * p.Tau * p.GammaTau * 1.0<Enthalpy>;
                  S = R * (p.Tau * p.GammaTau - p.Gamma) * 1.0<Entropy>;
                  Cv = R * (-((-p.Tau) ** 2.0) * p.GammaTauTau + ((p.GammaPi - p.Tau * p.GammaPiTau) ** 2.0) / p.GammaPiPi) * 1.0<IsochoricHeatCapacity>;
                  Cp = R * -((-p.Tau) ** 2.0) * p.GammaTauTau * 1.0<IsobaricHeatCapacity>;
                  SpeedOfSound = speedOfSound;
                  V = v;
                  Rho = 1.0 / v; }
            let gibbsMethod (ptPoint:PtPoint) =
                let state = {
                    P = ptPoint.pressure;
                    T = ptPoint.temperature;
                    Pi = (float ptPoint.pressure) / 16.53e6;
                    Tau = 1386.0 / (float ptPoint.temperature);
                    Gamma = 0.0;
                    GammaPi = 0.0;
                    GammaPiPi = 0.0;
                    GammaTau = 0.0;
                    GammaTauTau = 0.0;
                    GammaPiTau = 0.0;
                }
                let (+) state x = 
                    let (Region1And4Point.IJNPoint (I i, J j, N n)) = x
                    let pi = state.Pi
                    let tau = state.Tau
                    { Pi = state.Pi;
                      Tau = state.Tau;
                      Gamma = state.Gamma + (n * ((7.1 - pi) ** i) * ((tau - 1.222) ** j));
                      GammaPi = state.GammaPi + (-n * i * ((7.1 - pi) ** (i - 1.0)) * ((tau - 1.222) ** j));
                      GammaPiPi = state.GammaPiPi + (n * i * (i - 1.0) * ((7.1 - pi) ** (i - 2.0)) * ((tau - 1.222) ** j));
                      GammaTau = state.GammaTau + (n * j * ((7.1 - pi) ** i) * ((tau - 1.222) ** (j - 1.0)));
                      GammaTauTau = state.GammaTauTau + (n * j * (j - 1.0) * ((7.1 - pi) ** i) * ((tau - 1.222) ** (j - 2.0)));
                      GammaPiTau = state.GammaPiTau + (-n * i * j * ((7.1 - pi) ** (i - 1.0)) * ((tau - 1.222) ** (j - 1.0)));
                      P = state.P;
                      T = state.T; }

                let phaseInfoResult = ValidatedPhaseInfo.createAsPure "phaseInfo" PureRegion.Liquid
                match phaseInfoResult with
                | Ok phaseInfo -> Ok (((Seq.fold (+) state reg14), phaseInfo) |> createPvtEntry)
                | Error _ -> Error DomainError.OutOfRange
                

            let vaporMethod (ptPoint:PtPoint) tau tauShift regionPoints =
                let (ideal, residual) = VaporRegionPoints.value regionPoints

                let sumIdeal state =
                    let (+) (state:SpecificRegionPoint) x = 
                        let (J j, N n) = x
                        let tau = state.Tau
                        { Pi = state.Pi;
                          Tau = state.Tau;
                          Gamma = state.Gamma + (n * (tau ** j));
                          GammaPi = state.GammaPi;
                          GammaPiPi = state.GammaPiPi;
                          GammaTau = state.GammaTau + (n * j * (tau ** (j - 1.0)));
                          GammaTauTau = state.GammaTauTau + (n * j * (j - 1.0) * (tau ** (j - 2.0)));
                          GammaPiTau = state.GammaPiTau;
                          P = state.P;
                          T = state.T; }
                    Seq.fold (+) state ideal

                let sumResidual state =
                    let (+) state x = 
                        let (I i, J j, N n) = x
                        let pi = state.Pi
                        let tau = state.Tau
                        { Pi = state.Pi;
                          Tau = state.Tau;
                          Gamma = state.Gamma + (n * (pi ** i) * ((tau - tauShift) ** j));
                          GammaPi = state.GammaPi + (n * i * (pi ** (i - 1.0)) * ((tau - tauShift) ** j));
                          GammaPiPi = state.GammaPiPi + (n * i * (i - 1.0) * (pi ** (i - 2.0)) * ((tau - tauShift) ** j));
                          GammaTau = state.GammaTau + (n * (pi ** i) * j * ((tau - tauShift) ** (j - 1.0)));
                          GammaTauTau = state.GammaTauTau + (n * (pi ** i) * j * (j - 1.0) * ((tau - tauShift) ** (j - 2.0)));
                          GammaPiTau = state.GammaPiTau + (n * i * (pi ** (i - 1.0)) * j * ((tau - tauShift) ** (j - 1.0)));
                          P = state.P;
                          T = state.T; }
                    Seq.fold (+) state residual

                let pi = (float ptPoint.pressure) / 1.0e6
                let state = {
                    P = ptPoint.pressure;
                    T = ptPoint.temperature;
                    Pi = pi;
                    Tau = tau;
                    Gamma = log(pi);
                    GammaPi = 1.0 / pi;
                    GammaPiPi = -1.0 / (pi ** 2.0);
                    GammaTau = 0.0;
                    GammaTauTau = 0.0;
                    GammaPiTau = 0.0;
                }
                
                let phase = match (ptPoint.temperature, ptPoint.pressure) with
                            | (t, p) when t > criticalTemperature && p > criticalPressure -> PureRegion.SupercriticalFluid
                            | (t, p) when t > criticalTemperature && p <= criticalPressure -> PureRegion.Gas
                            | _ -> PureRegion.Vapor

                let phaseInfoResult = ValidatedPhaseInfo.createAsPure "phaseInfo" phase

                match phaseInfoResult with
                | Ok phaseInfo -> Ok (((sumIdeal state |> sumResidual), phaseInfo) |> createPvtEntry)
                | Error _ -> Error DomainError.OutOfRange

            let region3Method (ptPoint:PtPoint) =
                let calculateSpecificPoint density =
                    
                    let (Region3Points.Points (N n1, reg3Seq)) = reg3
                    let delta = density / 322.0<Density>
                    let state = {
                        P = ptPoint.pressure;
                        T = ptPoint.temperature;
                        Tau = 647.096 / (float ptPoint.temperature);
                        Delta = delta;
                        Phi = n1 * log(delta);
                        PhiDelta = n1 / delta;
                        PhiDeltaDelta = -n1 / (delta ** 2.0);
                        PhiTau = 0.0;
                        PhiTauTau = 0.0;
                        PhiDeltaTau = 0.0;
                    }
                    let (+) state x = 
                        let (I i, J j, N n) = x
                        let delta = state.Delta
                        let tau = state.Tau
                        { P = ptPoint.pressure;
                          T = ptPoint.temperature;
                          Tau = tau;
                          Delta = delta;
                          Phi = state.Phi + (n * (delta ** i) * (tau ** j));
                          PhiDelta = state.PhiDelta + (n * i * (delta ** (i - 1.0)) * (tau ** j));
                          PhiDeltaDelta = state.PhiDeltaDelta + (n * i * (i - 1.0) * (delta ** (i - 2.0)) * (tau ** j));
                          PhiTau = state.PhiTau + (n * (delta ** i) * j * (tau ** (j - 1.0)));
                          PhiTauTau = state.PhiTauTau + (n * (delta ** i) * j * (j - 1.0) * (tau ** (j - 2.0)));
                          PhiDeltaTau = state.PhiDeltaTau + (n * i * (delta ** (i - 1.0)) * j * (tau ** (j - 1.0))); }
                    Seq.fold (+) state reg3Seq




                let calculatePressure density p =
                    (p.PhiDelta * p.Delta * (float density) * (float waterGasConstant) * (float p.T)) * 1.0<Pressure>
                 


                let createPtvEntry density p =
                    let R = float waterGasConstant
                    let speedOfSound = 
                        let c = ((p.Tau ** 2.0) * p.PhiTauTau)
                        let b = ((p.Delta * p.PhiDelta - p.Delta * p.Tau * p.PhiDeltaTau) ** 2.0) 
                        let a = 2.0 * p.Delta * p.PhiDelta + (p.Delta ** 2.0) * p.PhiDeltaDelta
                        sqrt((a - b / c) * R * (float p.T)) * 1.0<Speed>
                    
                    let cp =    
                        let a = -(p.Tau ** 2.0) * p.PhiTauTau
                        let b = ((p.Delta * p.PhiDelta - p.Delta * p.Tau * p.PhiDeltaTau) ** 2.0)
                        let c = (2.0 * p.Delta * p.PhiDelta + (p.Delta ** 2.0) * p.PhiDeltaDelta) 
                        (a + (b / c)) * R * 1.0<IsobaricHeatCapacity>

                    let temperature = ptPoint.temperature
                    let phaseInfoResult = ValidatedPhaseInfo.createAsPure "phaseInfo" PureRegion.SupercriticalFluid
                    match phaseInfoResult with
                    | Ok phaseInfo -> Ok { 
                        ValidatedPtvEntry.P = calculatePressure density p;
                        T = temperature;
                        PhaseInfo = phaseInfo;
                        U = p.Tau * p.PhiTau * R * (float temperature) * 1.0<J / kg>;
                        H = (p.Tau * p.PhiTau + p.Delta * p.PhiDelta) * R * (float temperature) * 1.0<Enthalpy>;
                        S = (p.Tau * p.PhiTau - p.Phi) * R * 1.0<Entropy>;
                        Cv = -(p.Tau ** 2.0) * p.PhiTauTau * R * 1.0<IsochoricHeatCapacity>;
                        Cp = cp;
                        SpeedOfSound = speedOfSound;
                        V = 1.0 / density;
                        Rho = density; }
                    | Error _ -> Error DomainError.OutOfRange
                    

                let solver density = calculateSpecificPoint density |> calculatePressure density |> fun x -> float (x - ptPoint.pressure)

                let result = (NumericalMethods.Zeros.newton (fun x -> solver (x * 1.0<Density>)) 1.0)
                match result with
                | Ok density -> calculateSpecificPoint (density * 1.0<Density>) |> createPtvEntry (density * 1.0<Density>)
                | Error ConvergeError.MaxIterationsReached -> Error DomainError.FailedToConverge
            fun query ->
                let regionResult = resolveRegion (query, (Seq.toArray nReg34), (Seq.toArray nReg4))
                let reg2IdealValue (Region2IdealPoint.JNPoint (n, j)) =
                    (n, j)
                let reg2ResValue (Region2ResidualPoint.IJNPoint (i, n, j)) =
                    (i, n, j)
                let reg5IdealValue (Region5IdealPoint.JNPoint (n, j)) =
                    (n, j)    
                let reg5ResValue (Region5ResidualPoint.IJNPoint (i, n, j)) =
                    (i, n, j)
                let reg2VaporPoints = VaporRegionPoints.create reg2IdealValue regIdeal2 reg2ResValue regRes2
                let reg5VaporPoints = VaporRegionPoints.create reg5IdealValue regIdeal5 reg5ResValue regRes5
                match regionResult with
                | Ok (ptPoint, Region1) -> gibbsMethod ptPoint
                | Ok (ptPoint, Region2) -> vaporMethod ptPoint (540.0 / float (ptPoint.temperature)) 0.5 reg2VaporPoints
                | Ok (ptPoint, Region3) -> region3Method ptPoint
                | Ok (ptPoint, Region4) -> gibbsMethod ptPoint
                | Ok (ptPoint, Region5) -> vaporMethod ptPoint (1000.0 / float (ptPoint.temperature)) 0.0 reg5VaporPoints
                | Error err -> Error err
        
        let applyRegionPoints (x) f = asyncResult {
            let (<!>) = AsyncResult.map
            let (<*>) = AsyncResult.applyM
            return! f <!> x
        }
        async {
            let! fResult = (regionPoints |> applyRegionPoints) createRegionQueryFunction    
            return! match (fResult, queryHandler) with
                    | (Ok f, AsyncPtvQuery handler) -> handler f
                    | (Ok f, PtvQuery handler) -> AsyncResult.ofResult (handler f)
                    | (Error err, _) -> AsyncResult.ofError err
        }

