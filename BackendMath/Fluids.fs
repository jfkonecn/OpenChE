namespace EngineeringMath

open System

module Fluids =
    type DischargeCoefficient = DischargeCoefficient of float
    module DischargeCoefficient =
        let value (DischargeCoefficient x) = x
    type BernoulliResult = {
        vIn:float<Velocity>
        vOut:float<Velocity>
        hIn:float<Length>
        hOut:float<Length>
        pIn:float<Pressure>
        pOut:float<Pressure>
        rho:float<Density>
    }
    type SolveForVIn = {
        vOut:float<Velocity>
        hIn:float<Length>
        hOut:float<Length>
        pIn:float<Pressure>
        pOut:float<Pressure>
        rho:float<Density>
    }
    type SolveForVOut = {
        vIn:float<Velocity>
        hIn:float<Length>
        hOut:float<Length>
        pIn:float<Pressure>
        pOut:float<Pressure>
        rho:float<Density>
    }
    type SolveForHIn = {
        vIn:float<Velocity>
        vOut:float<Velocity>
        hOut:float<Length>
        pIn:float<Pressure>
        pOut:float<Pressure>
        rho:float<Density>
    }
    type SolveForHOut = {
        vIn:float<Velocity>
        vOut:float<Velocity>
        hIn:float<Length>
        pIn:float<Pressure>
        pOut:float<Pressure>
        rho:float<Density>
    }
    type SolveForPIn = {
        vIn:float<Velocity>
        vOut:float<Velocity>
        hIn:float<Length>
        hOut:float<Length>
        pOut:float<Pressure>
        rho:float<Density>
    }
    type SolveForPOut = {
        vIn:float<Velocity>
        vOut:float<Velocity>
        hIn:float<Length>
        hOut:float<Length>
        pIn:float<Pressure>
        rho:float<Density>
    }
    type SolveForDensity = {
        vIn:float<Velocity>
        vOut:float<Velocity>
        hIn:float<Length>
        hOut:float<Length>
        pIn:float<Pressure>
        pOut:float<Pressure>
    }

    type BernoulliArgument = 
        | SolveForVIn of SolveForVIn
        | SolveForVOut of SolveForVOut
        | SolveForHIn of SolveForHIn
        | SolveForHOut of SolveForHOut
        | SolveForPIn of SolveForPIn
        | SolveForPOut of SolveForPOut
        | SolveForDensity of SolveForDensity

    let bernoullisEquation (args:BernoulliArgument) =
        let g = float PhysicalConstants.Gravity

        let result = match args with
                     | (SolveForVIn x) -> {
                             vIn= sqrt(
                                     2.0 * (((float x.vOut ** 2.0) / 2.0 + g * float x.hOut + float x.pOut / float x.rho) -
                                         (g * float x.hIn + float x.pIn / float x.rho))
                                     ) * 1.0<Velocity>
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
                                     ) * 1.0<Velocity>
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
                                     ((float x.vIn ** 2.0) / 2.0 + float x.pIn / float x.rho)) * 1.0<Length>
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
                                     ((float x.vOut ** 2.0) / 2.0 + float x.pOut / float x.rho)) * 1.0<Length>
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
                                     ((float x.vIn ** 2.0) / 2.0 + g * float x.hIn)) * 1.0<Pressure>
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
                                     ((float x.vOut ** 2.0) / 2.0 + g * float x.hOut)) * 1.0<Pressure>
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
                                         ((float x.vOut ** 2.0) / 2.0 + g * float x.hOut)) * 1.0<Density>
                         }

        let isOutOfRange x =
            Double.IsInfinity(x) || Double.IsNaN(x) || x < 0.0
        match result with
        | { vIn=x } when (isOutOfRange (float x)) -> Error UiMessage.OutOfRange
        | { vOut=x } when (isOutOfRange (float x)) -> Error UiMessage.OutOfRange
        | { hIn=x } when (isOutOfRange (float x)) -> Error UiMessage.OutOfRange
        | { hOut=x } when (isOutOfRange (float x)) -> Error UiMessage.OutOfRange
        | { pIn=x } when (isOutOfRange (float x)) -> Error UiMessage.OutOfRange
        | { pOut=x } when (isOutOfRange (float x)) -> Error UiMessage.OutOfRange
        | { rho=x } when float x <= 0.0 || (isOutOfRange (float x)) -> Error UiMessage.OutOfRange
        | _ -> Ok result
