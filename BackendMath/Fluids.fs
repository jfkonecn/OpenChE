namespace EngineeringMath.Fluids
open EngineeringMath.Common
open Microsoft.FSharp.Data.UnitSystems.SI.UnitSymbols
open System

module BernoullisEquation =
    type DischargeCoefficient = DischargeCoefficient of float
    module DischargeCoefficient =
        let value (DischargeCoefficient x) = x
    type BernoulliResult = {
        vIn:float<m/s>
        vOut:float<m/s>
        hIn:float<m>
        hOut:float<m>
        pIn:float<Pa>
        pOut:float<Pa>
        rho:float<kg/m^3>
    }
    type SolveForVIn = {
        vOut:float<m/s>
        hIn:float<m>
        hOut:float<m>
        pIn:float<Pa>
        pOut:float<Pa>
        rho:float<kg/m^3>
    }
    type SolveForVOut = {
        vIn:float<m/s>
        hIn:float<m>
        hOut:float<m>
        pIn:float<Pa>
        pOut:float<Pa>
        rho:float<kg/m^3>
    }
    type SolveForHIn = {
        vIn:float<m/s>
        vOut:float<m/s>
        hOut:float<m>
        pIn:float<Pa>
        pOut:float<Pa>
        rho:float<kg/m^3>
    }
    type SolveForHOut = {
        vIn:float<m/s>
        vOut:float<m/s>
        hIn:float<m>
        pIn:float<Pa>
        pOut:float<Pa>
        rho:float<kg/m^3>
    }
    type SolveForPIn = {
        vIn:float<m/s>
        vOut:float<m/s>
        hIn:float<m>
        hOut:float<m>
        pOut:float<Pa>
        rho:float<kg/m^3>
    }
    type SolveForPOut = {
        vIn:float<m/s>
        vOut:float<m/s>
        hIn:float<m>
        hOut:float<m>
        pIn:float<Pa>
        rho:float<kg/m^3>
    }
    type SolveForDensity = {
        vIn:float<m/s>
        vOut:float<m/s>
        hIn:float<m>
        hOut:float<m>
        pIn:float<Pa>
        pOut:float<Pa>
    }

    type BernoulliArgument = 
        | SolveForVIn of SolveForVIn
        | SolveForVOut of SolveForVOut
        | SolveForHIn of SolveForHIn
        | SolveForHOut of SolveForHOut
        | SolveForPIn of SolveForPIn
        | SolveForPOut of SolveForPOut
        | SolveForDensity of SolveForDensity



    let internal bernoullisEquation (args:BernoulliArgument) =
        let g = float PhysicalConstants.Gravity

        let result = match args with
                     | (SolveForVIn x) -> {
                             vIn= sqrt(
                                     2.0 * (((float x.vOut ** 2.0) / 2.0 + g * float x.hOut + float x.pOut / float x.rho) -
                                         (g * float x.hIn + float x.pIn / float x.rho))
                                     ) * 1.0<m / s>
                             vOut=x.vOut
                             hIn=x.hIn
                             hOut=x.hOut
                             pIn=x.pIn
                             pOut=x.pOut
                             rho=x.rho
                         }
                     | (SolveForVOut x) -> {
                             vIn=x.vIn
                             vOut= sqrt(
                                     2.0 * (((float x.vIn ** 2.0) / 2.0 + g * float x.hIn + float x.pIn / float x.rho) -
                                         (g * float x.hOut + float x.pOut / float x.rho))
                                     ) * 1.0<m / s>
                             hIn=x.hIn
                             hOut=x.hOut
                             pIn=x.pIn
                             pOut=x.pOut
                             rho=x.rho
                         }
                     | (SolveForHIn x) -> {
                             vIn=x.vIn
                             vOut=x.vOut
                             hIn= (1.0 / g) * (((float x.vOut ** 2.0) / 2.0 + g * float x.hOut + float x.pOut / float x.rho) -
                                     ((float x.vIn ** 2.0) / 2.0 + float x.pIn / float x.rho)) * 1.0<m>
                             hOut=x.hOut
                             pIn=x.pIn
                             pOut=x.pOut
                             rho=x.rho
                         }
                     | (SolveForHOut x) -> {
                             vIn=x.vIn
                             vOut=x.vOut
                             hIn=x.hIn
                             hOut= (1.0 / g) * (((float x.vIn ** 2.0) / 2.0 + g * float x.hIn + float x.pIn / float x.rho) -
                                     ((float x.vOut ** 2.0) / 2.0 + float x.pOut / float x.rho)) * 1.0<m>
                             pIn=x.pIn
                             pOut=x.pOut
                             rho=x.rho
                         }
                     | (SolveForPIn x) -> {
                             vIn=x.vIn
                             vOut=x.vOut
                             hIn=x.hIn
                             hOut=x.hOut
                             pIn= float x.rho * (((float x.vOut ** 2.0) / 2.0 + g * float x.hOut + float x.pOut / float x.rho) -
                                     ((float x.vIn ** 2.0) / 2.0 + g * float x.hIn)) * 1.0<Pa>
                             pOut=x.pOut
                             rho=x.rho
                         }
                     | (SolveForPOut x) -> {
                             vIn=x.vIn
                             vOut=x.vOut
                             hIn=x.hIn
                             hOut=x.hOut
                             pIn=x.pIn
                             pOut= float x.rho * (((float x.vIn ** 2.0) / 2.0 + g * float x.hIn + float x.pIn / float x.rho) -
                                     ((float x.vOut ** 2.0) / 2.0 + g * float x.hOut)) * 1.0<Pa>
                             rho=x.rho
                         }
                     | (SolveForDensity x) -> {
                             vIn=x.vIn
                             vOut=x.vOut
                             hIn=x.hIn
                             hOut=x.hOut
                             pIn=x.pIn
                             pOut=x.pOut
                             rho= (float x.pOut - float x.pIn) /
                                     (((float x.vIn ** 2.0) / 2.0 + g * float x.hIn) -
                                         ((float x.vOut ** 2.0) / 2.0 + g * float x.hOut)) * 1.0<kg / m ^ 3>
                         }

        let isOutOfRange x =
            Double.IsInfinity(x) || Double.IsNaN(x) || x < 0.0
        match result with
        | { vIn=x } when (isOutOfRange (float x)) -> Error DomainError.OutOfRange
        | { vOut=x } when (isOutOfRange (float x)) -> Error DomainError.OutOfRange
        | { hIn=x } when (isOutOfRange (float x)) -> Error DomainError.OutOfRange
        | { hOut=x } when (isOutOfRange (float x)) -> Error DomainError.OutOfRange
        | { pIn=x } when (isOutOfRange (float x)) -> Error DomainError.OutOfRange
        | { pOut=x } when (isOutOfRange (float x)) -> Error DomainError.OutOfRange
        | { rho=x } when float x <= 0.0 || (isOutOfRange (float x)) -> Error DomainError.OutOfRange
        | _ -> Ok result


    let orificPlate =
        ()
