namespace EngineeringMath.UnitTests

open NUnit.Framework
open EngineeringMath.Fluids.BernoullisEquation
open EngineeringMath
open System

[<TestFixture>]
module FluidTests =
    let isWithin (propName:string) (e:float<'a>) (a:float<'a>) =
        match (float e) with
        | value when value = 0.0 ->  Assert.That(a, Is.EqualTo(e).Within(1e-3), propName) 
        | value -> Assert.That(a, Is.EqualTo(value).Within(0.5).Percent, propName) 

    let assertBernoulliResultsAreEqual (e:BernoulliResult) (a:BernoulliResult) =
        isWithin "vIn" e.vIn a.vIn
        isWithin "vOut" e.vOut a.vOut
        isWithin "hIn" e.hIn a.hIn
        isWithin "hOut" e.hOut a.hOut
        isWithin "pIn" e.pIn a.pIn
        isWithin "pOut" e.pOut a.pOut
        isWithin "rho" e.rho a.rho

    [<Test>]
    let ``Should Work for simple case`` () =
        let args = BernoulliArgument.SolveForVIn {
            vOut=10.0<Velocity>
            hIn=10.0<Length>
            hOut=9.0<Length>
            pIn=100.0<Pressure>
            pOut=20410.0<Pressure>
            rho=1000.0<Density>
        } 
        let actualResult = bernoullisEquation args
        let expected = {
            vIn=11.0<Velocity>
            vOut=10.0<Velocity>
            hIn=10.0<Length>
            hOut=9.0<Length>
            pIn=100.0<Pressure>
            pOut=20410.0<Pressure>
            rho=1000.0<Density>
        }
        match actualResult with
        | Ok actual -> assertBernoulliResultsAreEqual expected actual
        | Error _ -> Assert.Fail("Expected Success")

    [<Test>]
    let AllEquationsShouldYieldSameResult (
                                                    [<Values(0.0, 5.0)>]vIn,
                                                    [<Values(0.0, 5.0)>]vOut,
                                                    [<Values(0.0, 10.0)>]hIn,
                                                    [<Values(0.0, 10.0, 5.0)>]hOut,
                                                    [<Values(0.0, 500.0)>]pIn,
                                                    [<Values(0.0, 500.0, 100.0)>]pOut
                                                    ) =


        let assertRemainingResultsMatch rho =

            let vInArgs = BernoulliArgument.SolveForVIn {
                vOut=vOut
                hIn=hIn
                hOut=hOut
                pIn=pIn
                pOut=pOut
                rho=rho
            } 
            let vOutArgs = BernoulliArgument.SolveForVOut {
                vIn=vIn
                hIn=hIn
                hOut=hOut
                pIn=pIn
                pOut=pOut
                rho=rho
            } 
            let hInArgs = BernoulliArgument.SolveForHIn {
                vIn=vIn
                vOut=vOut
                hOut=hOut
                pIn=pIn
                pOut=pOut
                rho=rho
            } 
            let hOutArgs = BernoulliArgument.SolveForHOut {
                vIn=vIn
                vOut=vOut
                hIn=hIn
                pIn=pIn
                pOut=pOut
                rho=rho
            } 
            let pInArgs = BernoulliArgument.SolveForPIn {
                vIn=vIn
                vOut=vOut
                hIn=hIn
                hOut=hOut
                pOut=pOut
                rho=rho
            } 
            let pOutArgs = BernoulliArgument.SolveForPOut {
                vIn=vIn
                vOut=vOut
                hIn=hIn
                hOut=hOut
                pIn=pIn
                rho=rho
            } 


            let actualVIn = bernoullisEquation vInArgs
            let actualVOut = bernoullisEquation vOutArgs
            let actualHIn = bernoullisEquation hInArgs
            let actualHOut = bernoullisEquation hOutArgs
            let actualPIn = bernoullisEquation pInArgs
            let actualPOut = bernoullisEquation pOutArgs

            let expected = {
                vIn=vIn
                vOut=vOut
                hIn=hIn
                hOut=hOut
                pIn=pIn
                pOut=pOut
                rho=rho
            }

            let assertEqualAndSuccess expected actualResult=
                match actualResult with
                | Ok actual -> assertBernoulliResultsAreEqual expected actual
                | Error _ -> Assert.Fail("Expected Success")

            assertEqualAndSuccess expected actualVIn
            assertEqualAndSuccess expected actualVOut
            assertEqualAndSuccess expected actualHIn
            assertEqualAndSuccess expected actualHOut
            assertEqualAndSuccess expected actualPIn
            assertEqualAndSuccess expected actualPOut

        let rhoArgs = BernoulliArgument.SolveForDensity {
            vIn=vIn
            vOut=vOut
            hIn=hIn
            hOut=hOut
            pIn=pIn
            pOut=pOut
        } 
        let actualRho = bernoullisEquation rhoArgs
        match actualRho with
        | Ok {rho=rho} -> assertRemainingResultsMatch rho
        | Error _ -> Assert.Pass()
