using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Resources.PVTTables
{
    public class SteamTable : IPVTTable
    {


        public SteamTable()
        {

        }

        private enum SteamEquationRegion
        {
            Region1,
            Region2,
            Region3,
            Region4,
            Region5,
            OutOfRange
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="temperature">in K</param>
        /// <param name="pressure">in Pa</param>
        /// <returns></returns>
        private SteamEquationRegion FindRegion(double temperature, double pressure)
        {
            if (temperature < MinTemperature || temperature > MaxTemperature ||
                pressure < MinPressure || pressure > MaxPressure ||
                (pressure > 10e6 && temperature > 1073.15))
            {
                return SteamEquationRegion.OutOfRange;
            }

            if (temperature > (273.15 + 800))
            {
                return SteamEquationRegion.Region5;
            }
            else if (TryGetSatPressureUsingTemperature(temperature, out double satPressure))
            {
                if (satPressure == pressure)
                {
                    return SteamEquationRegion.Region4;
                }
                else if (satPressure > pressure)
                {
                    return SteamEquationRegion.Region2;
                }
                return SteamEquationRegion.Region1;
            }
            else if (TryGetBoundary34PressureUsingTemperature(temperature, out double boundaryPressure))
            {
                if (boundaryPressure > pressure)
                {
                    return SteamEquationRegion.Region2;
                }
                return SteamEquationRegion.Region3;
            }
            return SteamEquationRegion.OutOfRange;
        }




        private bool TryGetSatPressureUsingTemperature(double temperature, out double pressure)
        {
            if (temperature < MinTemperature || temperature >= CriticalTemperature)
            {
                pressure = double.NaN;
                return false;
            }
            double
                satTempRatio = temperature / 1,
                theta = satTempRatio + (nRegion4[8] / (satTempRatio - nRegion4[9])),
                A = Math.Pow(theta, 2) + nRegion4[0] * theta + nRegion4[1],
                B = nRegion4[2] * Math.Pow(theta, 2) + nRegion4[3] * theta + nRegion4[4],
                C = nRegion4[5] * Math.Pow(theta, 2) + nRegion4[6] * theta + nRegion4[7];
            pressure = Math.Pow((2 * C) / (-B + Math.Pow(Math.Pow(B, 2) - 4 * A * C, 0.5)), 4) * 1e6;
            return true;
        }

        private bool TryGetSatTemperatureUsingPressure(double pressure, out double temperature)
        {
            if (pressure < MinPressure || pressure >= CriticalPressure)
            {
                temperature = double.NaN;
                return false;
            }

            double beta = Math.Pow(pressure / 1e6, 0.25),
                E = Math.Pow(beta, 2) + nRegion4[2] * beta + nRegion4[5],
                F = nRegion4[0] * Math.Pow(beta, 2) + nRegion4[3] * beta + nRegion4[6],
                G = nRegion4[1] * Math.Pow(beta, 2) + nRegion4[4] * beta + nRegion4[7],
                D = (2 * G) / (-F - Math.Pow(Math.Pow(F, 2) - 4 * E * G, 0.5));
            temperature = (nRegion4[9] + D - Math.Pow(Math.Pow(nRegion4[9] + D, 2) - 4 * (nRegion4[8] + nRegion4[9] * D), 0.5)) / 2;
            return true;
        }


        private bool TryGetBoundary34PressureUsingTemperature(double temperature, out double pressure)
        {
            if (temperature > MaxTemperature || temperature < CriticalTemperature)
            {
                pressure = double.NaN;
                return false;
            }
            double theta = temperature / 1;
            pressure = (nBoundary34[0] + nBoundary34[1] * theta + nBoundary34[2] * Math.Pow(theta, 2));
            return true;
        }

        private bool TryGetBoundary34TemperatureUsingPressure(double pressure, out double temperature)
        {
            if (pressure > MaxPressure || pressure < CriticalPressure)
            {
                temperature = double.NaN;
                return false;
            }
            double pi = pressure / 1e6;
            temperature = (nBoundary34[3] + Math.Sqrt((pi - nBoundary34[4]) / nBoundary34[2]));
            return true;
        }
        #region Region1Support
        private IThermoEntry Region1Equation(double temperature, double pressure)
        {
            double gamma = 0,
                gammaPi = 0,
                gammaPiPi = 0,
                gammaTau = 0,
                gammaTauTau = 0,
                gammaPiTau = 0,
                pi = pressure / 16.53e6,
                tau = 1386.0 / temperature;
            foreach (RegionCoefficients item in Region1Coefficients)
            {
                gamma += item.N * Math.Pow(7.1 - pi, item.I) * Math.Pow(tau - 1.222, item.J);
                gammaPi += -item.N * item.I * Math.Pow(7.1 - pi, item.I - 1) * Math.Pow(tau - 1.222, item.J);
                gammaPiPi += item.N * item.I * (item.I - 1) * Math.Pow(7.1 - pi, item.I - 2) * Math.Pow(tau - 1.222, item.J);
                gammaTau += item.N * item.J * Math.Pow(7.1 - pi, item.I) * Math.Pow(tau - 1.222, item.J - 1);
                gammaTauTau += item.N * item.J * (item.J - 1) * Math.Pow(7.1 - pi, item.I) * Math.Pow(tau - 1.222, item.J - 2);
                gammaPiTau += -item.N * item.I * item.J * Math.Pow(7.1 - pi, item.I - 1) * Math.Pow(tau - 1.222, item.J - 1);
            }

            return CreateThermoEntryWithGibbs(Region.Liquid, temperature, pressure, tau, pi, gamma,
                gammaPi, gammaPiPi, gammaTau, gammaTauTau, gammaPiTau);
        }



        private readonly RegionCoefficients[] Region1Coefficients = new RegionCoefficients[]
        {
            new RegionCoefficients(0, -2, 1.4632971213167E-01),
            new RegionCoefficients(0, -1, -8.4548187169114E-01),
            new RegionCoefficients(0, 0, -3.756360367204),
            new RegionCoefficients(0, 1, 3.3855169168385E+00),
            new RegionCoefficients(0, 2, -9.5791963387872E-01),
            new RegionCoefficients(0, 3, 1.5772038513228E-01),
            new RegionCoefficients(0, 4, -1.6616417199501E-02),
            new RegionCoefficients(0, 5, 8.1214629983568E-04),
            new RegionCoefficients(1, -9, 2.8319080123804E-04),
            new RegionCoefficients(1, -7, -6.0706301565874E-04),
            new RegionCoefficients(1, -1, -1.8990068218419E-02),
            new RegionCoefficients(1, 0, -3.2529748770505E-02),
            new RegionCoefficients(1, 1, -2.1841717175414E-02),
            new RegionCoefficients(1, 3, -5.2838357969930E-05),
            new RegionCoefficients(2, -3, -4.7184321073267E-04),
            new RegionCoefficients(2, 0, -3.0001780793026E-04),
            new RegionCoefficients(2, 1, 4.7661393906987E-05),
            new RegionCoefficients(2, 3, -4.4141845330846E-06),
            new RegionCoefficients(2, 17, -7.2694996297594E-16),
            new RegionCoefficients(3, -4, -3.1679644845054E-05),
            new RegionCoefficients(3, 0, -2.8270797985312E-06),
            new RegionCoefficients(3, 6, -8.5205128120103E-10),
            new RegionCoefficients(4, -5, -2.2425281908000E-06),
            new RegionCoefficients(4, -2, -6.5171222895601E-07),
            new RegionCoefficients(4, 10, -1.4341729937924E-13),
            new RegionCoefficients(5, -8, -4.0516996860117E-07),
            new RegionCoefficients(8, -11, -1.2734301741641E-09),
            new RegionCoefficients(8, -6, -1.7424871230634E-10),
            new RegionCoefficients(21, -29, -6.8762131295531E-19),
            new RegionCoefficients(23, -31, 1.4478307828521E-20),
            new RegionCoefficients(29, -38, 2.6335781662795E-23),
            new RegionCoefficients(30, -39, -1.1947622640071E-23),
            new RegionCoefficients(31, -40, 1.8228094581404E-24),
            new RegionCoefficients(32, -41, -9.3537087292458E-26)
        };
        #endregion

        #region Region2Support
        private IThermoEntry Region2Equation(double temperature, double pressure)
        {

            double pi = pressure / 1.0e6,
            tau = 540.0 / temperature;
            return CreateVaporEntry(temperature, pressure, pi, tau, Region2IdealCoefficients, Region2ResidualCoefficients);
        }
        private readonly RegionCoefficients[] Region2IdealCoefficients = new RegionCoefficients[]
        {
            new RegionCoefficients(0,   -9.6927686500217E+00),
            new RegionCoefficients(1,   1.0086655968018E+01),
            new RegionCoefficients(-5,  -5.6087911283020E-03),
            new RegionCoefficients(-4,  7.1452738081455E-02),
            new RegionCoefficients(-3,  -4.0710498223928E-01),
            new RegionCoefficients(-2,  1.4240819171444E+00),
            new RegionCoefficients(-1,  -4.3839511319450E+00),
            new RegionCoefficients(2,  -2.8408632460772E-01),
            new RegionCoefficients(3,   2.1268463753307E-02)
        };
        private readonly RegionCoefficients[] Region2ResidualCoefficients = new RegionCoefficients[]
        {
            new RegionCoefficients(1,   0,   -1.7731742473213E-03),
            new RegionCoefficients(1,   1,   -1.7834862292358E-02),
            new RegionCoefficients(1,   2,   -4.5996013696365E-02),
            new RegionCoefficients(1,   3,   -5.7581259083432E-02),
            new RegionCoefficients(1,   6,   -5.0325278727930E-02),
            new RegionCoefficients(2,   1,   -3.3032641670203E-05),
            new RegionCoefficients(2,   2,   -1.8948987516315E-04),
            new RegionCoefficients(2,   4,   -3.9392777243355E-03),
            new RegionCoefficients(2,   7,   -4.3797295650573E-02),
            new RegionCoefficients(2,   36,  -2.6674547914087E-05),
            new RegionCoefficients(3,   0,   2.0481737692309E-08),
            new RegionCoefficients(3,   1,   4.3870667284435E-07),
            new RegionCoefficients(3,   3,   -3.2277677238570E-05),
            new RegionCoefficients(3,   6,   -1.5033924542148E-03),
            new RegionCoefficients(3,   35,  -4.0668253562649E-02),
            new RegionCoefficients(4,   1,   -7.8847309559367E-10),
            new RegionCoefficients(4,   2,   1.2790717852285E-08),
            new RegionCoefficients(4,   3,   4.8225372718507E-07),
            new RegionCoefficients(5,   7,   2.2922076337661E-06),
            new RegionCoefficients(6,   3,   -1.6714766451061E-11),
            new RegionCoefficients(6,   16,  -2.1171472321355E-03),
            new RegionCoefficients(6,   35,  -2.3895741934104E+01),
            new RegionCoefficients(7,   0,   -5.9059564324270E-18),
            new RegionCoefficients(7,   11,  -1.2621808899101E-06),
            new RegionCoefficients(7,   25,  -3.8946842435739E-02),
            new RegionCoefficients(8,   8,   1.1256211360459E-11),
            new RegionCoefficients(8,   36,  -8.2311340897998E+00),
            new RegionCoefficients(9,   13,  1.9809712802088E-08),
            new RegionCoefficients(10,  4,   1.0406965210174E-19),
            new RegionCoefficients(10,  10,  -1.0234747095929E-13),
            new RegionCoefficients(10,  14,  -1.0018179379511E-09),
            new RegionCoefficients(16,  29,  -8.0882908646985E-11),
            new RegionCoefficients(16,  50,  1.0693031879409E-01),
            new RegionCoefficients(18,  57,  -3.3662250574171E-01),
            new RegionCoefficients(20,  20,  8.9185845355421E-25),
            new RegionCoefficients(20,  35,  3.0629316876232E-13),
            new RegionCoefficients(20,  48,  -4.2002467698208E-06),
            new RegionCoefficients(21,  21,  -5.9056029685639E-26),
            new RegionCoefficients(22,  53,  3.7826947613457E-06),
            new RegionCoefficients(23,  39,  -1.2768608934681E-15),
            new RegionCoefficients(24,  26,  7.3087610595061E-29),
            new RegionCoefficients(24,  40,  5.5414715350778E-17),
            new RegionCoefficients(24,  58,  -9.4369707241210E-07)
        };
        #endregion
        #region Region3Support
        private IThermoEntry Region3Equation(double temperature, double pressure)
        {
            IThermoEntry
                guess = Region3EquationHelper(temperature, 500);
            const double maxErr = 1e-6;
            // use newton's method to converge
            double totalGuesses = 0;
            while (Math.Abs(guess.Pressure - pressure) > maxErr && totalGuesses < 1e4)
            {
                // used to find derivative of funciton
                IThermoEntry refPoint = Region3EquationHelper(temperature, guess.Density + 1);
                double fx = Math.Abs(guess.Pressure - pressure),
                    fxPrime = (Math.Abs(refPoint.Pressure - pressure) - fx) / (refPoint.Density - guess.Density),
                    density = guess.Density - fx / fxPrime;
                guess = Region3EquationHelper(temperature, density);
                totalGuesses++;
            }
            return guess;
        }

        private IThermoEntry Region3EquationHelper(double temperature, double density)
        {
            double n1 = Region3Coefficients[0].N,
                delta = density / 322,
                tau = 647.096 / temperature,
                phi = n1 * Math.Log(delta),
                phiDelta = n1 / delta,
                phiDeltaDelta = -n1 / Math.Pow(delta, 2),
                phiTau = 0,
                phiTauTau = 0,
                phiDeltaTau = 0;

            for(int i = 1; i < Region3Coefficients.Length; i++)
            {
                RegionCoefficients item = Region3Coefficients[i];
                phi += item.N * Math.Pow(delta, item.I) * Math.Pow(tau, item.J);
                phiDelta += item.N * item.I * Math.Pow(delta, item.I - 1) * Math.Pow(tau, item.J);
                phiDeltaDelta += item.N * item.I * (item.I - 1) * Math.Pow(delta, item.I - 2) * Math.Pow(tau, item.J);
                phiTau += item.N * Math.Pow(delta, item.I) * item.J * Math.Pow(tau, item.J - 1);
                phiTauTau += item.N * Math.Pow(delta, item.I) * item.J * (item.J - 1) * Math.Pow(tau, item.J - 2);
                phiDeltaTau += item.N * item.I * Math.Pow(delta, item.I - 1) * item.J * Math.Pow(tau, item.J - 1);
            }


            double pressure = phiDelta * delta * density * WaterGasConstant * temperature,
                specificVolume = 1.0 / density,
                internalEnergy = tau * phiTau * WaterGasConstant * temperature,
                enthalpy = (tau * phiTau + delta * phiDelta) * WaterGasConstant * temperature,
                entropy = (tau * phiTau - phi) * WaterGasConstant,
                isochoricHeatCapacity = -Math.Pow(tau, 2) * phiTauTau * WaterGasConstant,
                isobaricHeatCapacity = (-Math.Pow(tau, 2) * phiTauTau 
                + Math.Pow(delta * phiDelta - delta * tau * phiDeltaTau, 2) / (2 * delta * phiDelta + Math.Pow(delta, 2) * phiDeltaDelta)) 
                * WaterGasConstant,
                speedOfSound = Math.Sqrt((2 * delta * phiDelta + Math.Pow(delta, 2) * phiDeltaDelta - 
                Math.Pow(delta * phiDelta - delta * tau * phiDeltaTau, 2) / (Math.Pow(tau, 2) * phiTauTau)) * WaterGasConstant * temperature);

            return new ThermoEntry
                (Region.SupercriticalFluid, temperature, pressure,
                specificVolume, internalEnergy, enthalpy,
                entropy, isochoricHeatCapacity, isobaricHeatCapacity, speedOfSound);
        }


        private readonly RegionCoefficients[] Region3Coefficients = new RegionCoefficients[]
        {
            new RegionCoefficients(double.NaN,   double.NaN,   1.0658070028513E+00),
            new RegionCoefficients(0,   0,   -1.5732845290239E+01),
            new RegionCoefficients(0,   1,   2.0944396974307E+01),
            new RegionCoefficients(0,   2,   -7.6867707878716E+00),
            new RegionCoefficients(0,   7,   2.6185947787954E+00),
            new RegionCoefficients(0,   10,  -2.8080781148620E+00),
            new RegionCoefficients(0,   12,  1.2053369696517E+00),
            new RegionCoefficients(0,   23,  -8.4566812812502E-03),
            new RegionCoefficients(1,   2,   -1.2654315477714E+00),
            new RegionCoefficients(1,   6,   -1.1524407806681E+00),
            new RegionCoefficients(1,   15,  8.8521043984318E-01),
            new RegionCoefficients(1,   17,  -6.4207765181607E-01),
            new RegionCoefficients(2,   0,   3.8493460186671E-01),
            new RegionCoefficients(2,   2,   -8.5214708824206E-01),
            new RegionCoefficients(2,   6,   4.8972281541877E+00),
            new RegionCoefficients(2,   7,   -3.0502617256965E+00),
            new RegionCoefficients(2,   22,  3.9420536879154E-02),
            new RegionCoefficients(2,   26,  1.2558408424308E-01),
            new RegionCoefficients(3,   0,   -2.7999329698710E-01),
            new RegionCoefficients(3,   2,   1.3899799569460E+00),
            new RegionCoefficients(3,   4,   -2.0189915023570E+00),
            new RegionCoefficients(3,   16,  -8.2147637173963E-03),
            new RegionCoefficients(3,   26,  -4.7596035734923E-01),
            new RegionCoefficients(4,   0,   4.3984074473500E-02),
            new RegionCoefficients(4,   2,   -4.4476435428739E-01),
            new RegionCoefficients(4,   4,   9.0572070719733E-01),
            new RegionCoefficients(4,   26,  7.0522450087967E-01),
            new RegionCoefficients(5,   1,   1.0770512626332E-01),
            new RegionCoefficients(5,   3,   -3.2913623258954E-01),
            new RegionCoefficients(5,   26,  -5.0871062041158E-01),
            new RegionCoefficients(6,   0,   -2.2175400873096E-02),
            new RegionCoefficients(6,   2,   9.4260751665092E-02),
            new RegionCoefficients(6,   26,  1.6436278447961E-01),
            new RegionCoefficients(7,   2,   -1.3503372241348E-02),
            new RegionCoefficients(8,   26,  -1.4834345352472E-02),
            new RegionCoefficients(9,   2,   5.7922953628084E-04),
            new RegionCoefficients(9,   26,  3.2308904703711E-03),
            new RegionCoefficients(10,  0,   8.0964802996215E-05),
            new RegionCoefficients(10,  1,   -1.6557679795037E-04),
            new RegionCoefficients(11,  26,  -4.4923899061815E-05)
        };
        #endregion
        #region Region5Support
        private IThermoEntry Region5Equation(double temperature, double pressure)
        {
            double pi = pressure / 1.0e6,
            tau = 2000 / temperature;
            return CreateVaporEntry(temperature, pressure, pi, tau, Region5IdealCoefficients, Region5ResidualCoefficients);
        }
        private readonly RegionCoefficients[] Region5IdealCoefficients = new RegionCoefficients[]
        {
            new RegionCoefficients(0,   -1.3179983674201E+01),
            new RegionCoefficients(1,   6.8540841634434E+00),
            new RegionCoefficients(-3,  -2.4805148933466E-02),
            new RegionCoefficients(-2,  3.6901534980333E-01),
            new RegionCoefficients(-1,  -3.1161318213925E+00),
            new RegionCoefficients(2,   -3.2961626538917E-01)
        };
        private readonly RegionCoefficients[] Region5ResidualCoefficients = new RegionCoefficients[]
        {
            new RegionCoefficients(1,   1,   1.5736404855259E-03),
            new RegionCoefficients(1,   2,   9.0153761673944E-04),
            new RegionCoefficients(1,   3,   -5.0270077677648E-03),
            new RegionCoefficients(2,   3,   2.2440037409485E-06),
            new RegionCoefficients(2,   9,   -4.1163275453471E-06),
            new RegionCoefficients(3,   7,   3.7919454822955E-08)
        };
        #endregion
        private IThermoEntry CreateVaporEntry(double temperature, double pressure, double pi, double tau, 
            RegionCoefficients[] idealCoefficients, RegionCoefficients[] residualCoefficients)
        {
            double gamma = Math.Log(pi),
                gammaPi = 1 / pi,
                gammaPiPi = -1 / Math.Pow(pi, 2),
                gammaTau = 0,
                gammaTauTau = 0,
                gammaPiTau = 0;

            foreach (RegionCoefficients item in idealCoefficients)
            {
                gamma += item.N * Math.Pow(tau, item.J);
                gammaTau += item.N * item.J * Math.Pow(tau, item.J - 1);
                gammaTauTau += item.N * item.J * (item.J - 1) * Math.Pow(tau, item.J - 2);
            }
            foreach (RegionCoefficients item in residualCoefficients)
            {
                gamma += item.N * Math.Pow(pi, item.I) * Math.Pow(tau - 0.5, item.J);
                gammaPi += item.N * item.I * Math.Pow(pi, item.I - 1) * Math.Pow(tau - 0.5, item.J);
                gammaPiPi += item.N * item.I * (item.I - 1) * Math.Pow(pi, item.I - 2) * Math.Pow(tau - 0.5, item.J);
                gammaTau += item.N * Math.Pow(pi, item.I) * item.J * Math.Pow(tau - 0.5, item.J - 1);
                gammaTauTau += item.N * Math.Pow(pi, item.I) * item.J * (item.J - 1) * Math.Pow(tau - 0.5, item.J - 2);
                gammaPiTau += item.N * item.I * Math.Pow(pi, item.I - 1) * item.J * Math.Pow(tau - 0.5, item.J - 1);
            }

            Region region = Region.Vapor;
            if(temperature > CriticalTemperature)
            {
                if (pressure > CriticalPressure)
                    region = Region.SupercriticalFluid;
                else
                    region = Region.Gas;
            }

            return CreateThermoEntryWithGibbs(region, temperature, pressure, tau, pi, gamma,
                gammaPi, gammaPiPi, gammaTau, gammaTauTau, gammaPiTau);
        }


        private IThermoEntry CreateThermoEntryWithGibbs(
            Region region,
            double temperature, double pressure,
            double tau, double pi, double gamma, double gammaPi, 
            double gammaPiPi, double gammaTau, double gammaTauTau, double gammaPiTau)
        {
            double
                specificVolume = pi * (gammaPi * WaterGasConstant * temperature) / pressure,
                internalEnergy = WaterGasConstant * temperature * (tau * gammaTau - pi * gammaPi),
                enthalpy = WaterGasConstant * temperature * tau * gammaTau,
                entropy = WaterGasConstant * (tau * gammaTau - gamma),
                isochoricHeatCapacity = WaterGasConstant * (-Math.Pow(-tau, 2) * gammaTauTau + Math.Pow(gammaPi - tau * gammaPiTau, 2) / gammaPiPi),
                isobaricHeatCapacity = WaterGasConstant * -Math.Pow(-tau, 2) * gammaTauTau,
                speedOfSound = Math.Sqrt(WaterGasConstant * -Math.Pow(-tau, 2) * gammaTauTau);

            return new ThermoEntry
                (region, temperature, pressure,
                specificVolume, internalEnergy, enthalpy,
                entropy, isochoricHeatCapacity, isobaricHeatCapacity, speedOfSound);
        }
        private class RegionCoefficients
        {
            public RegionCoefficients(double i, double j, double n)
            {
                I = i;
                J = j;
                N = n;
            }

            public RegionCoefficients(double j, double n) : this(double.NaN, j, n)
            {
            }
            public double I { get; }
            public double J { get; }
            public double N { get; }
        }

        private static readonly double[] nRegion4 = new double[]
        {
            1167.0521452767,
            -724213.16703206,
            -17.073846940092,
            12020.82470247,
            -3232555.0322333,
            14.91510861353,
            -4823.2657361591,
            405113.40542057,
            -0.23855557567849,
            650.17534844798
        };

        private static readonly double[] nBoundary34 = new double[]
        {
            348.05185628969,
            -1.1671859879975,
            0.0010192970039326,
            572.54459862746,
            13.91883977887
        };

        /// <summary>
        /// In J/(kg * K)
        /// </summary>
        private static readonly double WaterGasConstant = 461.526;

        #region IPVTTableMembers

        public double CriticalTemperature => 647.096;

        public double CriticalPressure => 22.06e6;
        public IThermoEntry GetThermoEntryAtEnthapyAndPressure(double enthalpy, double pressure)
        {
            throw new NotImplementedException();
        }

        public IThermoEntry GetThermoEntryAtEntropyAndPressure(double entropy, double pressure)
        {
            throw new NotImplementedException();
        }

        public IThermoEntry GetThermoEntryAtSatPressure(double pressure, SaturationRegion phase)
        {
            throw new NotImplementedException();
        }

        public IThermoEntry GetThermoEntryAtSatTemp(double satTemp, SaturationRegion phase)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="temperature"></param>
        /// <param name="pressure"></param>
        /// <returns>null if out of range</returns>
        public IThermoEntry GetThermoEntryAtTemperatureAndPressure(double temperature, double pressure)
        {
            SteamEquationRegion equationRegion = FindRegion(temperature, pressure);
            IThermoEntry thermoEntry;
            switch (equationRegion)
            {
                case SteamEquationRegion.Region1:
                case SteamEquationRegion.Region4:
                    thermoEntry = Region1Equation(temperature, pressure);
                    break;
                case SteamEquationRegion.Region2:
                    thermoEntry = Region2Equation(temperature, pressure);
                    break;
                case SteamEquationRegion.Region3:
                    thermoEntry = Region3Equation(temperature, pressure);
                    break;
                case SteamEquationRegion.Region5:
                    thermoEntry = Region5Equation(temperature, pressure);
                    break;
                case SteamEquationRegion.OutOfRange:
                default:
                    thermoEntry = null;
                    break;
            }
            return thermoEntry;
        }

        public double MaxPressure { get; } = 100e6;

        public double MaxTemperature { get; } = 2273.15;

        public double MinPressure { get; } = 0;

        public double MinTemperature { get; } = 273.15;

        #endregion
    }
}
