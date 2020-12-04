
namespace NumericalMethods


// https://github.com/scipy/scipy/blob/master/scipy/optimize/zeros.py#L94-L363
// https://github.com/scipy/scipy/blob/master/scipy/optimize/tests/test_zeros.py
// https://github.com/scipy/scipy/blob/master/benchmarks/benchmarks/optimize_zeros.py
// https://github.com/numpy/numpy/blob/master/numpy/core/numeric.py#L2201-L2316

type ConvergeError = MaxIterationsReached

module Zeros =
    let withinTolerance x y aTol rTol =
        abs(x - y) <= aTol + rTol * abs(y)
    let newton f x0 =
        let fixX1 x1 = 
            match x1 with
            | x1 when x1 >= 0.0 -> x1
            | _ -> x1 - 1e-4
        let x1 = x0 * (1.0 + 1e-4) |> fixX1
        let maxIter = 50
        let aTol = 1.48e-12
        let rTol = 0.0
        let rec secantMethod x0 y0 x1 itr =
            let y1 = f x1
            match (x0, y0, x1, y1, itr) with
            | _ when itr > maxIter 
                -> Error ConvergeError.MaxIterationsReached
            | _ when (withinTolerance x0 x1 rTol aTol) -> 
                Ok x1
            | _ when abs(y1) > abs(y0) ->
                let x2 = (-y0 / y1 * x1 * (float itr / float maxIter) + x0) / (1.0 - y0 / y1)
                secantMethod x1 y1 x2 (itr + 1)
            | _ ->
                let x2 = (-y1 / y0 * x0 + x1) / (1.0 - y1 / y0)
                secantMethod x1 y1 x2 (itr + 1)

        secantMethod x0 (f x0) x1 1


module FiniteDifferenceFormulas =
    type ThreePointBackward<[<Measure>]'a, [<Measure>]'b> = { iMinus2:float<'a>; iMinus1:float<'a>; i:float<'a>; step:float<'b>; }
    type TwoPointCentral<[<Measure>]'a, [<Measure>]'b> = { iMinus1:float<'a>; i:float<'a>; i1:float<'a>; step:float<'b>; }
    type ThreePointForward<[<Measure>]'a, [<Measure>]'b> = { i:float<'a>; i1:float<'a>; i2:float<'a>; step:float<'b>; }
    type TwoPointBackward<[<Measure>]'a, [<Measure>]'b> = { iMinus1:float<'a>; i:float<'a>; step:float<'b>; }
    type TwoPointForward<[<Measure>]'a, [<Measure>]'b> = { i:float<'a>; i1:float<'a>; step:float<'b>; }

    type FirstDerivativeArgs<[<Measure>]'a, [<Measure>]'b> =
        | ThreePointBackward of ThreePointBackward<'a,'b>
        | TwoPointCentral of TwoPointCentral<'a, 'b>
        | ThreePointForward of ThreePointForward<'a,'b>
        | TwoPointBackward of TwoPointBackward<'a,'b>
        | TwoPointForward of TwoPointForward<'a,'b>

    let FirstDerivative (args:FirstDerivativeArgs<'a, 'b>) =
        match args with
        | (ThreePointBackward x) -> (x.iMinus2 - 4.0 * x.iMinus1 + 3.0 * x.i) / (2.0 * x.step)
        | (TwoPointCentral x) -> (x.i1 - x.iMinus1) / (2.0 * x.step)
        | (ThreePointForward x) -> (-3.0 * x.i + 4.0 * x.i1 - x.i2) / (2.0 * x.step)
        | (TwoPointBackward x) -> (x.i - x.iMinus1) / x.step
        | (TwoPointForward x) -> (x.i1 - x.i) / x.step
