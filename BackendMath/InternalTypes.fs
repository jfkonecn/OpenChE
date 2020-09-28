namespace EngineeringMath.Common
open Microsoft.FSharp.Data.UnitSystems.SI.UnitSymbols
open System
open EngineeringMath



[<Measure>] type Temperature = K
[<Measure>] type Pressure = Pa
[<Measure>] type Length = m
[<Measure>] type IsochoricHeatCapacity = J / (kg * K)
[<Measure>] type Cv = IsochoricHeatCapacity
[<Measure>] type IsobaricHeatCapacity = J / (kg * K)
[<Measure>] type Cp = IsobaricHeatCapacity
[<Measure>] type Power = W
[<Measure>] type Enthalpy = J / kg
[<Measure>] type Entropy = J / (kg * K)
[<Measure>] type Speed = m / s
[<Measure>] type Velocity = m / s
[<Measure>] type Acceleration = m / s ^ 2
[<Measure>] type Density = kg / m ^ 3
[<Measure>] type SpecificVolume = m ^ 3 / kg


type Fraction = private Fraction of float



type MassFraction = private MassFraction of Fraction
type VaporQuality = private VaporQuality of Fraction


type ValidatedPhaseInfo = internal {
    VaporFraction: MassFraction
    LiquidFraction: MassFraction
    SolidFraction: MassFraction
    PhaseRegion: PhaseRegion
}

type ValidatedPtvEntry = internal {
    T: float<Temperature>
    P: float<Pressure>
    PhaseInfo: ValidatedPhaseInfo
    U: float<J / kg>
    H: float<Enthalpy>
    S: float<Entropy>
    Cv: float<IsochoricHeatCapacity>
    Cp: float<IsobaricHeatCapacity>
    SpeedOfSound: float<Speed>
    Rho: float<Density>
    V: float<SpecificVolume>
}






type internal DomainError = 
    | OutOfRange
    | NotEnoughArguments
    | FailToLoadFile of string
    | FailedToConverge




module PhysicalConstants =
    let Gravity = 9.81<m / s ^ 2>
    

module Fraction =

    let value (Fraction f) = f

    let create fieldName f = 
        ConstrainedType.createFloat fieldName Fraction (float 0) (float 1) f

module MassFraction =

    let value (MassFraction f) = Fraction.value f

    let create fieldName f = 
        Result.map MassFraction (Fraction.create fieldName f)

module VaporQuality =

    let value (VaporQuality f) = Fraction.value f

    let fractionValue (VaporQuality q) = q

    let create fieldName f = 
        Result.map VaporQuality (Fraction.create fieldName f)

module ValidatedPhaseInfo =


    let create fieldName region vaporFraction liquidFraction solidFraction =
        let sum = [vaporFraction; liquidFraction; solidFraction] |> List.reduce (+)

        match (region, sum, vaporFraction, liquidFraction, solidFraction) with
            | (PureRegion Vapor, 1.0, 1.0, 0.0, 0.0)
            | (PureRegion Liquid, 1.0, 0.0, 1.0, 0.0)
            | (PureRegion Solid, 1.0, 0.0, 0.0, 1.0)
            | (PureRegion Gas, 0.0, 0.0, 0.0, 0.0)
            | (SolidLiquidVapor, 1.0, _, _, _)
            | (LiquidVapor, 1.0, _, _, 0.0)
            | (SolidVapor, 1.0, _, 0.0, _)
            | (SolidLiquid, 1.0, 0.0, _, _)
            | (PureRegion SupercriticalFluid, 0.0, 0.0, 0.0, 0.0) -> 
                Ok({ VaporFraction = MassFraction (Fraction vaporFraction); 
                     LiquidFraction = MassFraction (Fraction liquidFraction); 
                     SolidFraction = MassFraction (Fraction solidFraction);
                     PhaseRegion = region; })
            | _ -> Error(sprintf "%s: Phase info is out of range" fieldName)

    let createAsPure fieldName region = 
        match region with
            | (Vapor) -> create fieldName (PhaseRegion.PureRegion region) 1.0 0.0 0.0
            | (Liquid) -> create fieldName (PhaseRegion.PureRegion region) 0.0 1.0 0.0
            | (Solid) -> create fieldName (PhaseRegion.PureRegion region) 0.0 0.0 1.0
            | (Gas)
            | (SupercriticalFluid) -> create fieldName (PhaseRegion.PureRegion region) 0.0 0.0 0.0
