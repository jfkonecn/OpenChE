namespace EngineeringMath
open Microsoft.FSharp.Data.UnitSystems.SI.UnitSymbols
open EngineeringMath.Common



type PhaseInfo = {
    VaporFraction: float
    LiquidFraction: float
    SolidFraction: float
    PhaseRegion: PhaseRegion
}

type PtvEntry = {
    // Temperature
    T: float<K>
    // Pressure
    P: float<Pa>
    PhaseInfo: PhaseInfo
    // Internal Energy
    U: float<J / kg>
    // Enthalpy
    H: float<J / kg>
    // Entropy
    S: float<J / (kg * K)>
    // Isochoric Heat Capacity
    Cv: float<J / (kg * K)>
    // Isobaric Heat Capacity
    Cp: float<J / (kg * K)>
    SpeedOfSound: float<m / s>
    // Density
    Rho: float<kg / m ^ 3>
    // Specific Volume
    V: float<m ^ 3 / kg>
}


type UiMessage = 
    |OutOfRange
    |NotEnoughArguments
    |FailToLoadFile of string
    |FailedToConverge

type RegenerativeCycleArgs = { 
    inletBoilerT:float<K>;
    boilerT:float<K>; 
    boilerP:float<Pa>; 
    condenserP:float<Pa>;
    powerRequirement:float<W>;
    stages:int;
    minTemperatureDelta:float<K>;
    turbineEfficiency:float;
    pumpEfficiency:float; }

type RankineArgs = { 
    boilerT:float<K>; 
    boilerP:float<Pa>; 
    condenserP:float<Pa>;
    powerRequirement:float<W>;
    turbineEfficiency:float;
    pumpEfficiency:float; }

type CycleResult = {
    boilerPtv:PtvEntry; 
    condenserPtv:PtvEntry; 
    condenserSteamQuality:float;
    pumpWork:float<J/kg>;
    boilerWork:float<J/kg>;
    turbineWork:float<J/kg>;
    condenserWork:float<J/kg>;
    netWork:float<J/kg>;
    thermalEfficiency:float;
    steamRate:float<kg/s>;
    boilerHeatTransferRate:float<J/s>;
    condenserHeatTransferRate:float<J/s>;
}

type IsentropicExpansionQuery = { inletVaporT:float<K>; inletVaporP:float<Pa>; outletP:float<Pa> }
type IsentropicExpansionResult = { 
    vaporPtv:PtvEntry; 
    wetVaporPtv:PtvEntry; 
    satVapor:PtvEntry; 
    satLiquid:PtvEntry; 
    vaporQuality:float }


type PtvEntryQuery = 
    | PtQuery of float<Pa> * float<K>
    | SatTempQuery of float<K> * PureRegion 
    | SatPreQuery of float<Pa> * PureRegion 
    | EnthalpyQuery of float<J / kg> * float<Pa>
    | EntropyQuery of float<J / (kg * K)> * float<Pa>

type PerformPtvEntryQuery = PtvEntryQuery -> AsyncResult<PtvEntry, UiMessage>
type CalculateRankineCycle = RankineArgs -> AsyncResult<CycleResult, UiMessage>
type CalculateRegenerativeCycle = RegenerativeCycleArgs -> AsyncResult<CycleResult, UiMessage>
type CalculateIsentropicExpansion = IsentropicExpansionQuery -> AsyncResult<IsentropicExpansionResult, UiMessage>




module UiMessage =
    let internal mapFromDomainError (x:DomainError) =
        match x with
        | DomainError.OutOfRange -> UiMessage.OutOfRange
        | DomainError.NotEnoughArguments -> UiMessage.NotEnoughArguments
        | (DomainError.FailToLoadFile f) -> UiMessage.FailToLoadFile f
        | DomainError.FailedToConverge -> UiMessage.OutOfRange

module PhaseInfo =
    let mapFromValidatedPhaseInfo (x:ValidatedPhaseInfo) =
        { VaporFraction = (MassFraction.value x.VaporFraction);
        LiquidFraction = (MassFraction.value x.LiquidFraction);
        SolidFraction = (MassFraction.value x.SolidFraction);
        PhaseRegion = x.PhaseRegion; }


module PtvEntry =
    let mapFromValidatedEntry (x:ValidatedPtvEntry)=
        { PtvEntry.T = x.T;
        P = x.P;
        PhaseInfo = PhaseInfo.mapFromValidatedPhaseInfo x.PhaseInfo;
        U =  x.U;
        H =  x.H;
        S = x.S;
        Cv = x.Cv;
        Cp =  x.Cp;
        SpeedOfSound =  x.SpeedOfSound;
        V = x.V;
        Rho =  x.Rho; }
