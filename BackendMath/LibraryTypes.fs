namespace EngineeringMath
open Microsoft.FSharp.Data.UnitSystems.SI.UnitSymbols

type PureRegion = SupercriticalFluid|Gas|Vapor|Liquid|Solid
type PhaseRegion = 
    | SupercriticalFluid
    | Gas
    | Vapor
    | Liquid
    | Solid
    | SolidLiquid
    | LiquidVapor
    | SolidVapor
    | SolidLiquidVapor

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

type ThermoTableAction =
    | PerformPtvEntryQuery of (PtvEntryQuery -> PtvEntry)
    | CalculateRankineCycle of (RankineArgs -> CycleResult)
    | CalculateRegenerativeCycle of (RegenerativeCycleArgs -> CycleResult)
    | CalculateIsentropicExpansion of (IsentropicExpansionQuery -> IsentropicExpansionResult)