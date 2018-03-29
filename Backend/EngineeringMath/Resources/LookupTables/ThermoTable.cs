using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using EngineeringMath.Resources.LookupTables.ThermoTableElements;
using System.Collections;
using EngineeringMath.NumericalMethods.FiniteDifferenceFormulas;
using System.Diagnostics;

namespace EngineeringMath.Resources.LookupTables
{

    /// <summary>
    /// Stores data related to thermodynamic properties of a species at different pressures and temperatures
    /// </summary>
    public abstract class ThermoTable
    {

        protected ThermoTable(string resourceName)
        {
            Assembly assembly = Assembly.Load(new AssemblyName("EngineeringMath"));
            

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string[] firstLine = reader.ReadLine().Split(',');
                    // skip next line
                    reader.ReadLine().Split(',');
                    CreateTableElementsList(
                        firstLine, 
                        reader.ReadLine().Split(','), 
                        reader.ReadLine().Split(','));
                    while (!reader.EndOfStream)
                    {
                        string[] line = reader.ReadLine().Split(',');
                        int i = 0, j = 0;
                        while (i < TableElements.Count)
                        {
                            j = 6 * i;
                            TableElements[i].AddThermoEntry(CreateEntry(line, j, TableElements[i]));
                            i++;
                        }
                    }
                }
            }
            FinishUp();
        }

        /// <summary>
        /// Derivative ids used in the finishup function
        /// </summary>
        enum derivatives
        {
            dVdTp,
            dVdPt,
            dHdTp
        }


        /// <summary>
        /// finds beta, kappa, cp and cv for each thermo entry
        /// </summary>
        private void FinishUp()
        {
            // find beta, kappa, cp and cv for each thermo entry
            for (int i = 0; i < TableElements.Count; i++)
            {
                for (int j = 0; j < TableElements[i].Count; j++)
                {
                    ThermoEntry curPoint = TableElements[i][j];
                    Dictionary<UInt16, double> allDerivatives = new Dictionary<UInt16, double>();

                    // handle constant temperature
                    allDerivatives = allDerivatives.Concat(FindDerivative(i,
                        delegate (int idx) { return TableElements[idx][j]; },
                        TableElements.Count,
                        WithRespectToProperty.Pressure,
                        new Dictionary<ushort, GetProperty> {
                            { (UInt16)derivatives.dVdPt, delegate(ThermoEntry entry){ return entry.V; } }
                        })).ToDictionary(x => x.Key, x => x.Value);

                    // handle constant pressure
                    allDerivatives = allDerivatives.Concat(FindDerivative(j,
                        delegate (int idx) { return TableElements[i][idx]; },
                        TableElements[i].Count,
                        WithRespectToProperty.Temperature,
                        new Dictionary<ushort, GetProperty> {
                            { (UInt16)derivatives.dHdTp, delegate(ThermoEntry entry){ return entry.H; } },
                            { (UInt16)derivatives.dVdTp, delegate(ThermoEntry entry){ return entry.V; } }
                        })).ToDictionary(x => x.Key, x => x.Value);


                    TableElements[i][j].FinishUp(
                        allDerivatives[(UInt16)derivatives.dVdTp], 
                        allDerivatives[(UInt16)derivatives.dVdPt], 
                        allDerivatives[(UInt16)derivatives.dHdTp]);
                }
            }
        }

        /// <summary>
        /// Gets ThermoEntry given an index
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        private delegate ThermoEntry GetEntryAt(int idx);

        /// <summary>
        /// Gets the desired property a ThermEntry and returns it
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private delegate double GetProperty(ThermoEntry entry);

        /// <summary>
        /// Gets a thermodynamic entry at a saturation point
        /// </summary>
        /// <param name="num">either temperature or pressure</param>
        /// <param name="phase"></param>
        /// <returns></returns>
        private delegate ThermoEntry SatPointFun(ThermoEntry entry, ThermoEntry.Phase phase);

        /// <summary>
        /// Gets a thermodynamic entry at a non-saturation point
        /// </summary>
        /// <param name="entry">Reference Entry point</param>
        /// <param name="step">step away from the entry for a paraticular property(determined within the delegate)</param>
        /// <returns></returns>
        private delegate ThermoEntry NonSatPointFun(ThermoEntry entry, double step);

        /// <summary>
        /// Calculates the derviative of a property of an ThermoEntry
        /// </summary>
        /// <param name="getProperty"></param>
        /// <returns></returns>
        private delegate double CalculateDerivative(GetProperty getProperty);

        /// <summary>
        /// Use when calculating 
        /// </summary>
        enum WithRespectToProperty
        {
            Pressure,
            Temperature
        }


        private void GetRespectToFunctions(WithRespectToProperty withRespectTo, out GetProperty getProperty, 
            out SatPointFun satPointFun, out NonSatPointFun nonSatPointFun)
        {
            switch (withRespectTo)
            {
                case WithRespectToProperty.Pressure:
                    getProperty = delegate (ThermoEntry entry) { return entry.Pressure; };
                    satPointFun = delegate (ThermoEntry entry, ThermoEntry.Phase phase) { return GetThermoEntryAtSatPressure(entry.Pressure, phase); };
                    nonSatPointFun = 
                        delegate (ThermoEntry entry, double step)
                        {
                            return GetThermoEntryAtTemperatureAndPressure(entry.Temperature, entry.Pressure + step);
                        };
                    break;
                case WithRespectToProperty.Temperature:
                    getProperty = delegate (ThermoEntry entry) { return entry.Temperature; };
                    satPointFun = delegate (ThermoEntry entry, ThermoEntry.Phase phase) { return GetThermoEntryAtSatTemp(entry.Temperature, phase); };
                    nonSatPointFun =
                        delegate (ThermoEntry entry, double step)
                        {
                            return GetThermoEntryAtTemperatureAndPressure(entry.Temperature + step, entry.Pressure);
                        };
                    break;
                default:
                    throw new NotImplementedException();
            }
        }


        /// <summary>
        /// Finds the derivative for a thermo entry
        /// </summary>
        /// <param name="curIdx">Index of collection being used to calculate derivative</param>
        /// <param name="getEntryAt">function which gets the entry at the passed index of the collection being used</param>
        /// <param name="totalElements">total number of elements in the collection being used</param>
        /// <param name="withRespectTo">Is pressure or temperature being changed?</param>
        /// <param name="propFuns">Gets the property being used in the derivative 
        /// (ie this function would return entry.V for change in V with respect to pressure)</param>
        /// <returns></returns>
        private Dictionary<UInt16, double> FindDerivative(int curIdx, GetEntryAt getEntryAt, 
            int totalElements, WithRespectToProperty withRespectTo, Dictionary<UInt16, GetProperty> propFuns)
        {
            ThermoEntry curPoint = getEntryAt(curIdx);
            // stores the derivatives
            Dictionary<UInt16, double> allDerivatives = new Dictionary<UInt16, double>();

            GetRespectToFunctions(withRespectTo, out GetProperty getStepProperty, out SatPointFun satPointFun, out NonSatPointFun nonSatPointFun);

            CalculateDerivative calculateDerivative = null;

            if ((curIdx == totalElements - 1 && curPoint.EntryPhase != getEntryAt(curIdx - 1).EntryPhase) ||
                (curIdx == 0 && curPoint.EntryPhase != getEntryAt(curIdx + 1).EntryPhase))
            {
                // we don't have enough data to estimate a derivative
                calculateDerivative = delegate(GetProperty getProperty) { return 0; };
            }
            else if (totalElements - 1 == curIdx || (curPoint.EntryPhase != getEntryAt(curIdx + 1).EntryPhase))
            {
                ThermoEntry curPointMinus2, curPointMinus1;
                if (curIdx == 1)
                {
                    curPointMinus2 = getEntryAt(curIdx - 1);
                }
                else
                {
                    curPointMinus2 = getEntryAt(curIdx - 2);
                }


                if (curPointMinus2.EntryPhase != curPoint.EntryPhase)
                {
                    // there was a phase change
                    curPointMinus2 = satPointFun(curPoint, curPoint.EntryPhase);
                }

                // get next point at the midpoint
                double step = (getStepProperty(curPoint) - getStepProperty(curPointMinus2)) / 2;
                curPointMinus1 = nonSatPointFun(curPoint, -step);

                calculateDerivative = 
                    delegate (GetProperty getProperty) 
                    {
                        if (step == 0)
                            return 0;
                        return FirstDerivative.ThreePointBackward(getProperty(curPointMinus2), getProperty(curPointMinus1), getProperty(curPoint), step);
                    };

            }
            else if (curIdx == 0 || (curPoint.EntryPhase != getEntryAt(curIdx - 1).EntryPhase))
            {

                ThermoEntry curPointPlus1, curPointPlus2;
                if (curIdx == totalElements - 2)
                {
                    curPointPlus2 = getEntryAt(curIdx + 1);
                }
                else
                {
                    curPointPlus2 = getEntryAt(curIdx + 2);
                }

                if (curPointPlus2.EntryPhase != curPoint.EntryPhase)
                {
                    // there was a phase change
                    curPointPlus2 = satPointFun(curPoint, curPoint.EntryPhase);
                }

                // get next point at the midpoint
                double step = (getStepProperty(curPointPlus2) - getStepProperty(curPoint)) / 2;
                curPointPlus1 = nonSatPointFun(curPoint, step);

                calculateDerivative =
                    delegate (GetProperty getProperty)
                    {
                        if (step == 0)
                            return 0;
                        return FirstDerivative.ThreePointForward(getProperty(curPoint), getProperty(curPointPlus1), getProperty(curPointPlus2), step);
                    };
            }
            else
            {
                ThermoEntry curPointMinus1 = getEntryAt(curIdx - 1), curPointPlus1 = getEntryAt(curIdx + 1);

                if (curPointMinus1.EntryPhase != curPoint.EntryPhase)
                {
                    curPointMinus1 = satPointFun(curPoint, curPoint.EntryPhase);
                }

                if (curPointPlus1.EntryPhase != curPoint.EntryPhase)
                {
                    curPointPlus1 = satPointFun(curPoint, curPoint.EntryPhase);
                }

                double step = (getStepProperty(curPointPlus1) - getStepProperty(curPoint));

                if (step > (getStepProperty(curPoint) - getStepProperty(curPointMinus1)))
                {
                    step = getStepProperty(curPoint) - getStepProperty(curPointMinus1);
                    curPointPlus1 = nonSatPointFun(curPoint, step);
                }
                else
                {
                    curPointMinus1 = nonSatPointFun(curPoint, -step);
                }

                calculateDerivative =
                    delegate (GetProperty getProperty)
                    {
                        if (step == 0)
                            return 0;
                        return FirstDerivative.TwoPointCentral(getProperty(curPointMinus1), getProperty(curPointPlus1), step);
                    };
            }

            // calculate derivatives
            foreach (KeyValuePair<UInt16, GetProperty> dicEntry in propFuns)
            {
                allDerivatives.Add(dicEntry.Key, calculateDerivative(dicEntry.Value));
            }

            return allDerivatives;

        }


        /// <summary>
        /// Creates all elements in this table
        /// <para>Assumes pressure units in the string array are in MPa and temperature units are in C</para>
        /// </summary>
        /// <param name="firstLine">First line of the csv file assumed to have the pressure and saturated temperature</param>
        /// <param name="thirdLine">Third line of the csv file assumed to be saturated liquid thermo entry</param>
        /// <param name="fourthLine">Fourth line of the csv file assumed to be saturated vapor thermo entry</param>
        private void CreateTableElementsList(string[] firstLine, string[] thirdLine, string[] fourthLine)
        {
            int i = 0;

            while(i < firstLine.Length)
            {
                // assume pressure is in units of MPa
                double pressure = double.Parse(firstLine[i + 1]) * 1e6,                
                    // assume temperature is in units of C
                    satTemp = double.Parse(firstLine[i + 3]);
                ThermoEntry satLiq = CreateEntry(thirdLine, i, satTemp, pressure, ThermoEntry.Phase.liquid, true),
                    satVap = CreateEntry(fourthLine, i, satTemp, pressure, ThermoEntry.Phase.vapor, true);

                TableElements.Add(new ThermoConstPressureTable(pressure, satTemp, satLiq, satVap));
                i += 6;
            }
        }

        /// <summary>
        /// Creates a thermo entry based on a passed line of the csv file the current index being examined
        /// </summary>
        /// <param name="line"></param>
        /// <param name="i"></param>
        /// <param name="temperature">in C</param>
        /// <param name="pressure">in Pa</param>
        /// <param name="phase">The phase of the entry about to be created</param>
        /// <param name="isSaturated">True if saturated</param>
        /// <returns></returns>
        private ThermoEntry CreateEntry(string[] line, int i, double temperature, double pressure, ThermoEntry.Phase phase, bool isSaturated)
        {
            return new ThermoEntry(temperature, pressure,
                    // Specific Volume Assume m3/Mg
                    double.Parse(line[i + 1]) * 1e-3,
                    // Enthalpy Assume kJ/kg 
                    double.Parse(line[i + 3]),
                    // Entropy Assume kJ/(kg * K)
                    double.Parse(line[i + 4]), phase, isSaturated);
        }

        /// <summary>
        /// Creates a thermo entry based on a passed line of the csv file the current index being examined
        /// Intended for non-satruation point entries
        /// </summary>
        /// <param name="line"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private ThermoEntry CreateEntry(string[] line, int i, ThermoConstPressureTable ele)
        {
            ThermoEntry.Phase phase;
            double temperature = double.Parse(line[i]);
            //TODO: we could have more than one saturation point
            if (ele.SatVaporEntry.Temperature < temperature)
            {
                phase = ThermoEntry.Phase.vapor;
            }
            else if(ele.SatLiquidEntry.Temperature > temperature)
            {
                phase = ThermoEntry.Phase.liquid;
            }
            else
            {
                throw new Exception("Is this entry at a saturation point?");
            }

            return CreateEntry(line, i, temperature, ele.Pressure, phase, false);
        }

        /// <summary>
        /// Gets ThermoEntry at passed pressure and passed temperature. Null when no entry found.
        /// </summary>
        /// <param name="temperature">Desired Temperture (C)</param>
        /// <param name="pressure">Desired Pressure (Pa)</param>
        /// <returns></returns>
        public ThermoEntry GetThermoEntryAtTemperatureAndPressure(double temperature, double pressure)
        {
            return ThermoEntry.Interpolation<ThermoConstPressureTable>.InterpolationThermoEntryFromList(
                pressure,
                TableElements,
                delegate (ThermoEntry entry)
                {
                    return entry.Pressure;
                },
                delegate (ThermoConstPressureTable obj)
                {
                    return obj.GetThermoEntryAtTemperature(temperature);
                },
                delegate (ThermoEntry entry, ThermoEntry.Phase phase) 
                {
                    return GetThermoEntryAtSatTemp(entry.Temperature, phase);
                });
        }

        /// <summary>
        /// Gets ThermoEntry for saturated liquid or vapor at passed pressure. Null when no entry found.
        /// </summary>
        /// <param name="pressure">Desired Pressure (Pa)</param>
        /// <returns></returns>
        public ThermoEntry GetThermoEntryAtSatPressure(double pressure, ThermoEntry.Phase phase)
        {
            ThermoEntry.Interpolation<ThermoConstPressureTable>.ObjectToThermoEntry satFun;
            if (phase == ThermoEntry.Phase.vapor)
            {
                satFun = delegate (ThermoConstPressureTable obj)
                {
                    return obj.SatVaporEntry;
                };
            }
            else if (phase == ThermoEntry.Phase.liquid)
            {
                satFun = delegate (ThermoConstPressureTable obj)
                {
                    return obj.SatLiquidEntry;
                };
            }
            else
            {
                throw new NotImplementedException("Solid phase hasn't been coded yet");
            }

            return ThermoEntry.Interpolation<ThermoConstPressureTable>.InterpolationThermoEntryFromList(
                pressure,
                TableElements,
                delegate (ThermoEntry obj)
                {
                    return obj.Pressure;
                },
                satFun,
                delegate (ThermoEntry entry, ThermoEntry.Phase myPhase)
                {
                    return GetThermoEntryAtSatPressure(entry.Pressure, myPhase);
                });
        }

        /// <summary>
        /// Gets ThermoEntry and Pressure for saturated liquid or vapor at passed satTemp. Null when no entry found.
        /// </summary>
        /// <param name="satTemp">Desired saturation temperature</param>
        /// <param name="isVapor">True if desired entry is for saturated vapor else returns data for saturated liquid</param>
        /// <returns></returns>
        public ThermoEntry GetThermoEntryAtSatTemp(double satTemp, ThermoEntry.Phase phase)
        {
            double pressure = double.NaN;
            ThermoEntry entry = null;

            if (satTemp == MinSatTableTemperature)
            {
                pressure = MinTablePressure;
                entry = GetThermoEntryAtSatPressure(pressure, phase);
            }
            else if (satTemp == MaxSatTableTemperature)
            {
                pressure = MaxTablePressure;
                entry = GetThermoEntryAtSatPressure(pressure, phase);
            }
            else if (satTemp > MinSatTableTemperature && satTemp < MaxSatTableTemperature)
            {
                    // max tries allowed
                 int  maxTries = (int)1e6,
                    // current amount of tries attempted
                    curTries = 0;
                // max difference allowed between desired satTemp and actual satTemp
                double maxDelta = 0.0001,
                    // The minimum pressure in the range being searched in
                    minP = MinTablePressure,
                    // The maximum pressure in the range being searched in
                    maxP = MaxTablePressure;
                // binary search to find pressure of sat temp        
                while(curTries < maxTries)
                {
                    pressure = (maxP - minP) / 2 + minP;
                    entry = GetThermoEntryAtSatPressure(pressure, phase);
                    if(Math.Abs(entry.Temperature - satTemp) <= maxDelta)
                    {
                        break;
                    }
                    else if(entry.Temperature < satTemp)
                    {
                        minP = pressure;
                    }
                    else
                    {
                        maxP = pressure;
                    }
                    curTries++;
                }
            }
            return entry;
        }

        /// <summary>
        /// Gets the ThermoEntry which matches the entropy and pressure passed
        /// </summary>
        /// <param name="entropy">kJ/(kg*K)</param>
        /// <param name="pressure">Pa</param>
        /// <returns></returns>
        public ThermoEntry GetThermoEntryAtEntropyAndPressure(double entropy, double pressure)
        {
            return GetThermoEntryAtPropertyAndPressure(
                delegate (ThermoEntry entry) { return entry.S; }, entropy, pressure);
        }


        /// <summary>
        /// Gets the ThermoEntry which matches the entropy and pressure passed
        /// </summary>
        /// <param name="entropy">kJ/(kg*K)</param>
        /// <param name="pressure">Pa</param>
        /// <returns></returns>
        public ThermoEntry GetThermoEntryAtEnthapyAndPressure(double enthalpy, double pressure)
        {
            return GetThermoEntryAtPropertyAndPressure(
                delegate(ThermoEntry entry) { return entry.H; }, enthalpy, pressure);
        }


        delegate double GetEntryProperty(ThermoEntry entry);
        /// <summary>
        /// Gets the ThermoEntry which matches the property and pressure passed
        /// </summary>
        /// <param name="fun">function used to extracted the desired match property</param>
        /// <param name="matchNum">The value the property needs to match</param>
        /// <param name="pressure">Pa</param>
        /// <returns></returns>
        private ThermoEntry GetThermoEntryAtPropertyAndPressure(GetEntryProperty fun, double matchNum, double pressure)
        {
            ThermoEntry entry = null,
            minEntry = GetThermoEntryAtTemperatureAndPressure(MinTableTemperature, pressure),
            maxEntry = GetThermoEntryAtTemperatureAndPressure(MaxTableTemperature, pressure);

            if (minEntry == null || maxEntry == null)
            {
                return entry;
            }

            // max tries allowed
            int maxTries = (int)1e3,
               // current amount of tries attempted
               curTries = 0;
            // max difference allowed between desired satTemp and actual satTemp
            double maxDelta = 0.0001;
            // binary search to find pressure of sat temp        
            while (curTries < maxTries)
            {
                entry = GetThermoEntryAtTemperatureAndPressure(
                (maxEntry.Temperature + minEntry.Temperature) / 2,
                pressure);
                if (entry == null)
                {
                    break;
                }
                else if (Math.Abs(matchNum - fun(entry)) <= maxDelta)
                {
                    break;
                }
                else if (fun(entry) < matchNum)
                {
                    minEntry = entry;
                }
                else
                {
                    maxEntry = entry;
                }
                curTries++;
            }

            return entry;
        }

        /// <summary>
        /// returns a thermoEntry which results after isentropic expansion
        /// </summary>
        /// <param name="inletVaporTemperature">temperature of the vapor entering the turbine</param>
        /// <param name="inletVaporPressure">pressure of vapor entering the turbine</param>
        /// <param name="outletPressure">pressure of vapor leaving the turbine</param>
        /// <param name="vaporQuality">fraction of vapor in the stream</param>
        /// <returns></returns>
        internal ThermoEntry IsentropicExpansion(double inletVaporTemperature, double inletVaporPressure, double outletPressure, out double vaporQuality)
        {
            ThermoEntry inletVaporConditions = GetThermoEntryAtTemperatureAndPressure(inletVaporTemperature, inletVaporPressure),
                testEntry = GetThermoEntryAtEntropyAndPressure(inletVaporConditions.S, outletPressure),
                satVapor = GetThermoEntryAtSatPressure(outletPressure, ThermoEntry.Phase.vapor),
                satLiquid = GetThermoEntryAtSatPressure(outletPressure, ThermoEntry.Phase.liquid);

            if(testEntry.S >= satVapor.S)
            {
                vaporQuality = 1;
                return testEntry;
            }

            vaporQuality = (inletVaporConditions.S - satLiquid.S)
                 / (satVapor.S - satLiquid.S);
            return ThermoEntry.WetVapor(satVapor, satLiquid, vaporQuality);
        }


        /// <summary>
        /// Returns the smallest temperature stored (C)
        /// </summary>
        public double MinTableTemperature
        {
            get
            {
                return TableElements.Min(x => x.MinTemperature);
            }
        }

        /// <summary>
        /// Returns the largest temperature stored (C)
        /// </summary>
        public double MaxTableTemperature
        {
            get
            {
                return TableElements.Max(x => x.MaxTemperature);
            }
        }

        /// <summary>
        /// Returns the smallest saturation temperature stored (C)
        /// </summary>
        public double MinSatTableTemperature
        {
            get
            {
                return GetThermoEntryAtSatPressure(MinTablePressure, ThermoEntry.Phase.vapor).Temperature;
            }
        }

        /// <summary>
        /// Returns the largest saturation temperature stored (C)
        /// </summary>
        public double MaxSatTableTemperature
        {
            get
            {
                return GetThermoEntryAtSatPressure(MaxTablePressure, ThermoEntry.Phase.vapor).Temperature;
            }
        }

        /// <summary>
        /// Returns the smallest pressure stored (Pa)
        /// </summary>
        public double MinTablePressure
        {
            get
            {
                return TableElements.Min(x => x.Pressure);
            }
        }

        /// <summary>
        /// Returns the largest pressure stored (Pa)
        /// </summary>
        public double MaxTablePressure
        {
            get
            {
                return TableElements.Max(x => x.Pressure);
            }
        }


        /// <summary>
        /// Returns the smallest Enthalpy stored (kj/kg)
        /// </summary>
        public double MinTableEnthalpy
        {
            get
            {
                return GetThermoEntryAtTemperatureAndPressure(MinTableTemperature, MinTablePressure).H;
            }
        }

        /// <summary>
        /// Returns the largest Enthalpy stored (kj/kg)
        /// </summary>
        public double MaxTableEnthalpy
        {
            get
            {
                return GetThermoEntryAtTemperatureAndPressure(MaxTableTemperature, MaxTablePressure).H;
            }
        }

        /// <summary>
        /// Returns the smallest Entropy stored (kj/kg)
        /// </summary>
        public double MinTableEntropy
        {
            get
            {
                return GetThermoEntryAtTemperatureAndPressure(MinTableTemperature, MinTablePressure).S;
            }
        }

        /// <summary>
        /// Returns the largest Entropy stored (kj/kg)
        /// </summary>
        public double MaxTableEntropy
        {
            get
            {
                return GetThermoEntryAtTemperatureAndPressure(MaxTableTemperature, MaxTablePressure).S;
            }
        }

        private List<ThermoConstPressureTable> TableElements = new List<ThermoConstPressureTable>();
    }
}
