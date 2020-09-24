namespace EngineeringMath.UnitTests

open System
open NUnit.Framework
open EngineeringMath.SteamProperties
open Microsoft.FSharp.Data.UnitSystems.SI.UnitSymbols
open EngineeringMath
open EngineeringMath.Thermo

[<TestFixture>]
module SteamTableTests =
    let validData =
        [
            yield  ({ T = 750.0<Temperature>;
                      P = 78.3e6<Pressure>;
                      PhaseInfo =  { VaporFraction = 0.0;
                                      LiquidFraction = 0.0;
                                      SolidFraction = 0.0;
                                      PhaseRegion = PhaseRegion.PureRegion SupercriticalFluid; };
                      InternalEnergy =  2102.06e3<J / kg>;
                      H =  2258.68e3<Enthalpy>;
                      S = 4.469e3<Entropy>;
                      Cv = 2.71e3<IsochoricHeatCapacity>;
                      Cp =  6.34e3<IsobaricHeatCapacity>;
                      SpeedOfSound =  760.69<Speed>;
                      V = 2e-3<SpecificVolume>;
                      Rho =  500.0<Density>; })
            yield  ({ T = 473.15<Temperature>;
                      P = 40e6<Pressure>;
                      PhaseInfo =  { VaporFraction = 0.0;
                                      LiquidFraction = 1.0;
                                      SolidFraction = 0.0;
                                      PhaseRegion = PhaseRegion.PureRegion Liquid; };
                      InternalEnergy =  825.22e3<J / kg>;
                      H =  870.12e3<Enthalpy>;
                      S = 2.275e3<Entropy>;
                      Cv = 3.29e3<IsochoricHeatCapacity>;
                      Cp =  4.315e3<IsobaricHeatCapacity>;
                      SpeedOfSound =  1457.4<Speed>;
                      V = 1.122e-3<SpecificVolume>;
                      Rho =  890.9<Density>; })
            yield  ({ T = 2000.0<Temperature>;
                      P = 30e6<Pressure>;
                      PhaseInfo =  { VaporFraction = 0.0;
                                      LiquidFraction = 0.0;
                                      SolidFraction = 0.0;
                                      PhaseRegion = PhaseRegion.PureRegion SupercriticalFluid; };
                      InternalEnergy =  5637.07e3<J / kg>;
                      H =  6571.22e3<Enthalpy>;
                      S = 8.53e3<Entropy>;
                      Cv = 2.39e3<IsochoricHeatCapacity>;
                      Cp =  2.88e3<IsobaricHeatCapacity>;
                      SpeedOfSound =  1067.36<Speed>;
                      V = 3.114e-2<SpecificVolume>;
                      Rho =  32.11<Density>; })
            yield  ({ T = 823.15<Temperature>;
                      P = 14e6<Pressure>;
                      PhaseInfo =  { VaporFraction = 0.0;
                                      LiquidFraction = 0.0;
                                      SolidFraction = 0.0;
                                      PhaseRegion = PhaseRegion.PureRegion Gas; };
                      InternalEnergy =  3114.3e3<J / kg>;
                      H =  3460.98e3<Enthalpy>;
                      S = 6.56e3<Entropy>;
                      Cv = 1.89e3<IsochoricHeatCapacity>;
                      Cp =  2.66e3<IsobaricHeatCapacity>;
                      SpeedOfSound =  666.05<Speed>;
                      V = 2.476e-2<SpecificVolume>;
                      Rho =  40.38<Density>; })
        ] 

    let validSatData =
        [
            yield  ({ T = 393.3<Temperature>;
                      P = 0.2e6<Pressure>;
                      PhaseInfo =  { VaporFraction = 0.0;
                                      LiquidFraction = 1.0;
                                      SolidFraction = 0.0;
                                      PhaseRegion = PhaseRegion.PureRegion Liquid; };
                      InternalEnergy =  504471.74<J / kg>;
                      H =  504683.84<Enthalpy>;
                      S = 1530.09<Entropy>;
                      Cv = 3666.9<IsochoricHeatCapacity>;
                      Cp =  4246.7<IsobaricHeatCapacity>;
                      SpeedOfSound =  1520.6<Speed>;
                      V = 1.0605e-3<SpecificVolume>;
                      Rho =  942.93<Density>; })
            yield  ({ T = 393.3<Temperature>;
                      P = 0.2e6<Pressure>;
                      PhaseInfo =  { VaporFraction = 1.0;
                                      LiquidFraction = 0.0;
                                      SolidFraction = 0.0;
                                      PhaseRegion = PhaseRegion.PureRegion Vapor; };
                      InternalEnergy =  2529094.32<J / kg>;
                      H =  2706241.34<Enthalpy>;
                      S = 7126.85<Entropy>;
                      Cv = 1615.96<IsochoricHeatCapacity>;
                      Cp =  2175.22<IsobaricHeatCapacity>;
                      SpeedOfSound =  481.88<Speed>;
                      V = 8.857e-1<SpecificVolume>;
                      Rho =  1.129<Density>; })

        ] 

    let validCompositeData =
        [
            yield  ({ T = 318.95<Temperature>;
                      P = 10e3<Pressure>;
                      PhaseInfo =  { VaporFraction = 0.805;
                                      LiquidFraction = 0.195;
                                      SolidFraction = 0.0;
                                      PhaseRegion = PhaseRegion.LiquidVapor; };
                      InternalEnergy =  1999135.82<J / kg>;
                      H =  2117222.94<Enthalpy>;
                      S = 6.6858e3<Entropy>;
                      Cv = 1966.28<IsochoricHeatCapacity>;
                      Cp =  2377.86<IsobaricHeatCapacity>;
                      SpeedOfSound =  655.0<Speed>;
                      V = 5.177e-3<SpecificVolume>;
                      Rho =  193.16<Density>; })
        ] 

    let validPtData = validData |> List.map (fun (e) -> TestCaseData(e, TestName=(sprintf "%.0f Pa, %.0f K" e.P e.T)))
    let validSatPreData = validSatData |> List.map (fun (e) -> TestCaseData(e, TestName=(sprintf "Saturated %.0f Pa, %A" e.P e.PhaseInfo.PhaseRegion)))
    let validSatTempData = validSatData |> List.map (fun (e) -> TestCaseData(e, TestName=(sprintf "Saturated %.0f K, %A" e.T e.PhaseInfo.PhaseRegion)))
    let validEnthalpyData = validCompositeData @ validData |> List.map (fun (e) -> TestCaseData(e, TestName=(sprintf "Enthalpy %.0f Pa %.0f J / kg" e.P e.H)))
    let validEntropyData = validCompositeData @ validData |> List.map (fun (e) -> TestCaseData(e, TestName=(sprintf "Entropy %.0f Pa %.0f J / (kg * K)" e.P e.S)))
    
    let isWithin (propName:string) (e:float<'a>) (a:float<'a>) =
        match (float e) with
        | value when value = 0.0 ->  Assert.That(a, Is.EqualTo(e).Within(1e-3), propName) 
        | value -> Assert.That(a, Is.EqualTo(value).Within(0.5).Percent, propName) 

    let assertPtvAreEqual e a =
        isWithin "Temperature" e.T a.T
        isWithin "Pressure" e.P a.P
        isWithin "PhaseInfo.VaporFraction" e.PhaseInfo.VaporFraction a.PhaseInfo.VaporFraction
        isWithin "PhaseInfo.LiquidFraction" e.PhaseInfo.LiquidFraction a.PhaseInfo.LiquidFraction
        isWithin "PhaseInfo.SolidFraction" e.PhaseInfo.SolidFraction a.PhaseInfo.SolidFraction
        Assert.AreEqual(e.PhaseInfo.PhaseRegion, a.PhaseInfo.PhaseRegion, "Phase Region")
        isWithin "InternalEnergy" e.InternalEnergy a.InternalEnergy
        isWithin "Enthalpy" e.H a.H
        isWithin "Entropy" e.S a.S
        isWithin "Cv" e.Cv a.Cv
        isWithin "Cp" e.Cp a.Cp
        isWithin "SpeedOfSound" e.SpeedOfSound a.SpeedOfSound
        isWithin "Density" e.Rho a.Rho
        isWithin "V" e.V a.V

    [<TestCaseSource("validPtData")>]
    let ShouldGetExpectedSteamEntry (ptvEntry:PtvEntry) =
           
        match (performSteamQuery (PtvQuery (performPtvEntryQuery (PtvEntryQuery.BasicPtvEntryQuery (PtQuery (ptvEntry.P, ptvEntry.T)))))) |> Async.RunSynchronously with
        | Ok x -> assertPtvAreEqual ptvEntry x
        | Error e -> Assert.Fail(sprintf "Unexpected error: %A" e) 

    [<TestCaseSource("validPtData")>]
    let ShouldGetExpectedSaturatedPressureSteamEntry (ptvEntry:PtvEntry) =
           
        match (performSteamQuery (PtvQuery (performPtvEntryQuery (PtvEntryQuery.BasicPtvEntryQuery (PtQuery (ptvEntry.P, ptvEntry.T)))))) |> Async.RunSynchronously with
        | Ok x -> assertPtvAreEqual ptvEntry x
        | Error e -> Assert.Fail(sprintf "Unexpected error: %A" e) 

    [<TestCaseSource("validSatTempData")>]
    let ShouldGetExpectedSaturatedTemperatureSteamEntry (ptvEntry:PtvEntry) =
        let performQuery t r = match (performSteamQuery (PtvQuery (performPtvEntryQuery (PtvEntryQuery.BasicPtvEntryQuery (SatTempQuery (t, r)))))) |> Async.RunSynchronously with
                               | Ok x -> assertPtvAreEqual ptvEntry x
                               | Error e -> Assert.Fail(sprintf "Unexpected error: %A" e)
        
        match ptvEntry.PhaseInfo.PhaseRegion with
        | (PhaseRegion.PureRegion r) -> (performQuery ptvEntry.T r)
        | _ -> Assert.Fail("Pure region expected")

    [<TestCaseSource("validEnthalpyData")>]
    let ShouldGetExpectedEnthalpySteamEntry (ptvEntry:PtvEntry) =
        match (performSteamQuery (PtvQuery (performPtvEntryQuery (EnthalpyQuery (ptvEntry.H, ptvEntry.P))))) |> Async.RunSynchronously with
        | Ok x -> assertPtvAreEqual ptvEntry x
        | Error e -> Assert.Fail(sprintf "Unexpected error: %A" e) 

    [<TestCaseSource("validEntropyData")>]
    let ShouldGetExpectedEntropySteamEntry (ptvEntry:PtvEntry) =
        match (performSteamQuery (PtvQuery (performPtvEntryQuery (EntropyQuery (ptvEntry.S, ptvEntry.P))))) |> Async.RunSynchronously with
        | Ok x -> assertPtvAreEqual ptvEntry x
        | Error e -> Assert.Fail(sprintf "Unexpected error: %A" e) 


    [<Test>]
    let ShouldMatchAllQueries ([<Range(1e6, 1.2e6, 5e4)>]p, [<Range(273.0, 673.0, 100.0)>]t) =
        let assertSameResult e a =
            match (e, a) with
            | (Ok e, Ok a) -> assertPtvAreEqual e a
            | (Error e, Error a) -> Assert.AreEqual(e, a)
            | (Ok _, Error _) -> Assert.Fail "Expected success got and error"
            | (Error _, Ok _) -> Assert.Fail "Expected an error got and success"

        let checkEnthalpyEntropyQueries ptvEntry =
            let enthalpyResult = performSteamQuery (PtvQuery (performPtvEntryQuery (EnthalpyQuery (ptvEntry.H, ptvEntry.P)))) |> Async.RunSynchronously
            assertSameResult (Ok ptvEntry) enthalpyResult
            let entropyResult = performSteamQuery (PtvQuery (performPtvEntryQuery (EntropyQuery (ptvEntry.S, ptvEntry.P)))) |> Async.RunSynchronously
            assertSameResult (Ok ptvEntry) entropyResult

        let ptEntry = performSteamQuery (PtvQuery (performPtvEntryQuery (PtvEntryQuery.BasicPtvEntryQuery (PtQuery (p, t))))) |> Async.RunSynchronously
        match ptEntry with
                | Ok x -> checkEnthalpyEntropyQueries x
                | Error _ -> ()

    type isentropicAssertion = { wetVaporT:float<K>; wetVaporP:float<Pa>; wetVaporH:float<Enthalpy>; vaporQuality:float}
    let isentropicExpansionData =
        [
            yield  ({inletVaporT=300.0<K>; inletVaporP=10.0<Pa>; outletP=10.0<Pa>})
        ] |> List.map (fun (e) -> TestCaseData(e, TestName=(sprintf "Isentropic Expansion %.0f Pa, %.0f K" e.inletVaporP e.inletVaporT)))

    [<TestCaseSource("isentropicExpansionData")>]
    let IsentropicExpansionShouldWork (query:IsentropicExpansionQuery) =
        match (performSteamQuery (AsyncPtvQuery (isentropicExpansion query))) |> Async.RunSynchronously with
        | Ok x -> ()
        | Error e -> Assert.Fail(sprintf "Unexpected error: %A" e) 

    [<Test>]
    let RankineCycleShouldWork() =
        let args = { 
            RankineArgs.boilerT= 773.15<K>; 
            boilerP= 8600e3<Pressure>; 
            condenserP= 10e3<Pressure>;
            powerRequirement= 80e6<W>;
            turbineEfficiency= 0.75;
            pumpEfficiency= 0.75; }

        let expectedBoilerPtv = { T = 773.15<Temperature>;
                                  P = 8.6e6<Pressure>;
                                  PhaseInfo =  { VaporFraction = 0.0;
                                                  LiquidFraction = 0.0;
                                                  SolidFraction = 0.0;
                                                  PhaseRegion = PhaseRegion.PureRegion Gas; };
                                  InternalEnergy =  3059796.86<J / kg>;
                                  H =  3392157.80<Enthalpy>;
                                  S = 6685.87<Entropy>;
                                  Cv = 1820.19<IsochoricHeatCapacity>;
                                  Cp =  2507.52<IsobaricHeatCapacity>;
                                  SpeedOfSound =  651.85<Speed>;
                                  V = 0.0386<SpecificVolume>;
                                  Rho =  25.87<Density>; }

        let expectedCondenserPtv = { T = 318.95<Temperature>;
                                     P = 1e4<Pressure>;
                                     PhaseInfo =  { VaporFraction = 0.805;
                                                     LiquidFraction = 0.195;
                                                     SolidFraction = 0.0;
                                                     PhaseRegion = PhaseRegion.LiquidVapor; };
                                     InternalEnergy =  2e6<J / kg>;
                                     H =  2117246.34<Enthalpy>;
                                     S = 6685.87<Entropy>;
                                     Cv = 1966.25<IsochoricHeatCapacity>;
                                     Cp =  2377.84<IsobaricHeatCapacity>;
                                     SpeedOfSound =  654.99<Speed>;
                                     V = 11.80<SpecificVolume>;
                                     Rho =  193.15<Density>; }
        let assertIsExpected (a:CycleResult) =
            isWithin "condenserSteamQuality" 0.8051 a.condenserSteamQuality
            isWithin "pumpWork" 11.6e3<J/kg> a.pumpWork
            isWithin "boilerWork" 3189e3<J/kg> a.boilerWork
            isWithin "condenserWork" 2244e3<J/kg> a.condenserWork
            isWithin "turbineWork" 956e3<J/kg> a.turbineWork
            isWithin "thermalEfficiency" 0.299 a.thermalEfficiency
            isWithin "netWork" 944.4e3<J/kg> a.netWork
            isWithin "steamRate" 84.75<kg/s> a.steamRate
            isWithin "boilerHeatTransferRate" 270.05e6<J/s> a.boilerHeatTransferRate
            isWithin "condenserHeatTransferRate" 190.1e6<J/s> a.condenserHeatTransferRate
            assertPtvAreEqual expectedBoilerPtv a.boilerPtv
            assertPtvAreEqual expectedCondenserPtv a.condenserPtv

        match (performSteamQuery (AsyncPtvQuery (rankineCycle args))) |> Async.RunSynchronously with
        | Ok x -> assertIsExpected x
        | Error e -> Assert.Fail(sprintf "Unexpected error: %A" e) 


    
    [<Test>]
    let RegenerativeCycleShouldWork() =
        let args = { 
            RegenerativeCycleArgs.boilerT= 773.15<K>; 
            boilerP= 8600e3<Pressure>; 
            condenserP= 10e3<Pressure>;
            inletBoilerT= 499.15<K>;
            powerRequirement= 80e6<W>;
            stages=5;
            minTemperatureDelta=5.0<K>;
            turbineEfficiency= 0.75;
            pumpEfficiency= 0.75; }

        let expectedBoilerPtv = { T = 773.15<Temperature>;
                                  P = 8.6e6<Pressure>;
                                  PhaseInfo =  { VaporFraction = 0.0;
                                                  LiquidFraction = 0.0;
                                                  SolidFraction = 0.0;
                                                  PhaseRegion = PhaseRegion.PureRegion Gas; };
                                  InternalEnergy =  3059796.86<J / kg>;
                                  H =  3392157.80<Enthalpy>;
                                  S = 6685.87<Entropy>;
                                  Cv = 1820.19<IsochoricHeatCapacity>;
                                  Cp =  2507.52<IsobaricHeatCapacity>;
                                  SpeedOfSound =  651.85<Speed>;
                                  V = 0.0386<SpecificVolume>;
                                  Rho =  25.87<Density>; }

        let expectedCondenserPtv = { T = 324.0<Temperature>;
                                     P = 1.3486e4<Pressure>;
                                     PhaseInfo =  { VaporFraction = 0.91;
                                                     LiquidFraction = 0.0886;
                                                     SolidFraction = 0.0;
                                                     PhaseRegion = PhaseRegion.LiquidVapor; };
                                     InternalEnergy =  2.247e6<J / kg>;
                                     H =  2.383e6<Enthalpy>;
                                     S = 7395.4<Entropy>;
                                     Cv = 1695.3<IsochoricHeatCapacity>;
                                     Cp =  2148.9<IsobaricHeatCapacity>;
                                     SpeedOfSound =  542.1<Speed>;
                                     V = 10.09<SpecificVolume>;
                                     Rho =  87.6<Density>; }
        let assertIsExpected (a:CycleResult) =
            isWithin "condenserSteamQuality" 0.911 a.condenserSteamQuality
            isWithin "pumpWork" 11.6e3<J/kg> a.pumpWork
            isWithin "boilerWork" 2620.0e3<J/kg> a.boilerWork
            isWithin "condenserWork" 1668e3<J/kg> a.condenserWork
            isWithin "turbineWork" 956e3<J/kg> a.turbineWork
            isWithin "thermalEfficiency" 0.363 a.thermalEfficiency
            isWithin "netWork" 944.4e3<J/kg> a.netWork
            isWithin "steamRate" 85.0<kg/s> a.steamRate
            isWithin "boilerHeatTransferRate" 222.9e6<J/s> a.boilerHeatTransferRate
            isWithin "condenserHeatTransferRate" 141.9e6<J/s> a.condenserHeatTransferRate
            assertPtvAreEqual expectedBoilerPtv a.boilerPtv
            assertPtvAreEqual expectedCondenserPtv a.condenserPtv

        match (performSteamQuery (AsyncPtvQuery (regenerativeCycle args))) |> Async.RunSynchronously with
        | Ok x -> assertIsExpected x
        | Error e -> Assert.Fail(sprintf "Unexpected error: %A" e) 
