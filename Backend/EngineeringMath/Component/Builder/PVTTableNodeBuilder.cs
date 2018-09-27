using EngineeringMath.Resources;
using EngineeringMath.Resources.PVTTables;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component.Builder
{
    public class PVTTableNodeBuilder : VisitableNodeBuilder
    {
        public static PVTTableNodeBuilder SteamTableBuilder()
        {
            return new PVTTableNodeBuilder(SteamTable.Table, LibraryResources.SteamTable);
        }


        public PVTTableNodeBuilder(IPVTTable table, string name)
        {
            Table = table;
            Header = name;
            NodeSettings = new Dictionary<string, NodeBuilderParameterSetting>()
            {
                { nameof(IThermoEntry.Region),
                    new PickerParameterSetting<SaturationRegion>(BuildDisplayName(LibraryResources.ThermoRegion), "region", BuildRegionOptions()) },
                { nameof(IThermoEntry.VaporFraction),
                    new UnitlessParameterSetting(BuildDisplayName(LibraryResources.VaporFraction), "xv", 0, 1) },
                { nameof(IThermoEntry.LiquidFraction),
                    new UnitlessParameterSetting(BuildDisplayName(LibraryResources.LiquidFraction), "xl", 0, 1) },
                { nameof(IThermoEntry.SolidFraction),
                    new UnitlessParameterSetting(BuildDisplayName(LibraryResources.SolidFraction), "xs", 0, 1) },
                { nameof(IThermoEntry.Temperature),
                    new SIParameterSetting(BuildDisplayName(LibraryResources.Temperature), "T", LibraryResources.Temperature, 
                    Table.MinTemperature, Table.MaxTemperature) },
                { nameof(IThermoEntry.Pressure),
                    new SIParameterSetting(BuildDisplayName(LibraryResources.Pressure), "P", LibraryResources.Pressure, 
                    Table.MinPressure, Table.MaxPressure) },
                { nameof(IThermoEntry.SpecificVolume),
                    new SIParameterSetting(BuildDisplayName(LibraryResources.SpecificVolume), "Vs", LibraryResources.SpecificVolume) },
                { nameof(IThermoEntry.InternalEnergy),
                    new SIParameterSetting(BuildDisplayName(LibraryResources.InternalEnergy), "U", LibraryResources.Energy) },
                { nameof(IThermoEntry.Enthalpy),
                    new SIParameterSetting(BuildDisplayName(LibraryResources.Enthalpy), "H", LibraryResources.Enthalpy) },
                { nameof(IThermoEntry.Entropy),
                    new SIParameterSetting(BuildDisplayName(LibraryResources.Entropy), "S", LibraryResources.Entropy) },
                { nameof(IThermoEntry.IsobaricHeatCapacity),
                    new SIParameterSetting(BuildDisplayName(LibraryResources.IsobaricHeatCapacity), "cp", LibraryResources.Entropy) },
                { nameof(IThermoEntry.IsochoricHeatCapacity),
                    new SIParameterSetting(BuildDisplayName(LibraryResources.IsochoricHeatCapacity), "cv", LibraryResources.Entropy) },
                { nameof(IThermoEntry.SpeedOfSound),
                    new SIParameterSetting(BuildDisplayName(LibraryResources.Velocity), "u", LibraryResources.Velocity) },
                { nameof(IThermoEntry.Density),
                    new SIParameterSetting(BuildDisplayName(LibraryResources.Density), "rho", LibraryResources.Density) }
            };
        }

        private string BuildDisplayName(string detail)
        {
            return $"{Header}: {detail}";
        }

        public override void BuildParameters()
        {
            foreach (NodeBuilderParameterSetting setting in NodeSettings.Values)
            {
                if (setting.UseThisParameter && setting.AutoBuildParameter)
                    Node.Parameters.Add(setting.BuildParameter());
            }
        }

        public override void BuildVisitorOptions()
        {
            Node.VisitorOptions = 
                new SelectableList<FunctionVisitor, IParameterContainerNode>(BuildDisplayName(LibraryResources.FindThermoDataWith), Node);

            bool buildTempAndPre = CheckIfBeingUsed(nameof(IThermoEntry.Temperature), nameof(IThermoEntry.Pressure)),
                buildSatPre = CheckIfBeingUsed(nameof(IThermoEntry.Region), nameof(IThermoEntry.Pressure)),
                buildSatTemp = CheckIfBeingUsed(nameof(IThermoEntry.Region), nameof(IThermoEntry.Temperature)),
                buildSPreTemp = CheckIfBeingUsed(nameof(IThermoEntry.Entropy), nameof(IThermoEntry.Pressure)),
                buildHPreTemp = CheckIfBeingUsed(nameof(IThermoEntry.Enthalpy), nameof(IThermoEntry.Pressure));

            if (buildTempAndPre)
                Node.VisitorOptions.Add(BuildTempAndPreVisitor());
            if (buildSatPre)
                Node.VisitorOptions.Add(BuildSatPressureVisitor());
            if (buildSatTemp)
                Node.VisitorOptions.Add(BuildSatTempVisitor());
            if (buildSPreTemp)
                Node.VisitorOptions.Add(BuildEntropyAndPressureVisitor());
            if (buildHPreTemp)
                Node.VisitorOptions.Add(BuildEnthalpyAndPressureVisitor());

        }

        private FunctionVisitor BuildTempAndPreVisitor()
        {
            return new FunctionVisitor(LibraryResources.TemperatureAndPressure,
                (ctx) =>
                {
                    NumericParameter T = (NumericParameter)FindParameter(nameof(IThermoEntry.Temperature)),
                    P = (NumericParameter)FindParameter(nameof(IThermoEntry.Pressure));
                    IThermoEntry entry = Table.GetThermoEntryAtTemperatureAndPressure(T.BaseValue, P.BaseValue);
                    SetOutputParameters(ctx, entry, T, P);
                },
                IsOutput(nameof(IThermoEntry.Temperature), nameof(IThermoEntry.Pressure)));
        }


        private FunctionVisitor BuildSatPressureVisitor()
        {
            return new FunctionVisitor(LibraryResources.SatPressure,
                (ctx) =>
                {
                    NumericParameter P = (NumericParameter)FindParameter(nameof(IThermoEntry.Pressure));
                    PickerParameter<SaturationRegion> reg = (PickerParameter<SaturationRegion>)FindParameter(nameof(IThermoEntry.Region));
                    IThermoEntry entry = Table.GetThermoEntryAtSatPressure(P.BaseValue, reg.ItemAtSelectedIndex);
                    SetOutputParameters(ctx, entry, P, reg);
                },
                IsOutput(nameof(IThermoEntry.Region), nameof(IThermoEntry.Pressure)));
        }

        private FunctionVisitor BuildSatTempVisitor()
        {
            return new FunctionVisitor(LibraryResources.SatTemperature,
                (ctx) =>
                {
                    NumericParameter T = (NumericParameter)FindParameter(nameof(IThermoEntry.Temperature));
                    PickerParameter<SaturationRegion> reg = (PickerParameter<SaturationRegion>)FindParameter(nameof(IThermoEntry.Region));
                    IThermoEntry entry = Table.GetThermoEntryAtSatPressure(T.BaseValue, reg.ItemAtSelectedIndex);
                    SetOutputParameters(ctx, entry, T, reg);
                },
                IsOutput(nameof(IThermoEntry.Region), nameof(IThermoEntry.Temperature)));
        }

        private FunctionVisitor BuildEntropyAndPressureVisitor()
        {
            return new FunctionVisitor(LibraryResources.EntropyAndPressure,
                (ctx) =>
                {
                    NumericParameter S = (NumericParameter)FindParameter(nameof(IThermoEntry.Entropy)),
                    P = (NumericParameter)FindParameter(nameof(IThermoEntry.Pressure));
                    IThermoEntry entry = Table.GetThermoEntryAtEntropyAndPressure(S.BaseValue, P.BaseValue);
                    SetOutputParameters(ctx, entry, S, P);
                },
                IsOutput(nameof(IThermoEntry.Entropy), nameof(IThermoEntry.Pressure)));
        }

        private FunctionVisitor BuildEnthalpyAndPressureVisitor()
        {
            return new FunctionVisitor(LibraryResources.EnthalpyAndPressure,
                (ctx) =>
                {
                    NumericParameter H = (NumericParameter)FindParameter(nameof(IThermoEntry.Enthalpy)),
                    P = (NumericParameter)FindParameter(nameof(IThermoEntry.Pressure));
                    IThermoEntry entry = Table.GetThermoEntryAtEnthalpyAndPressure(H.BaseValue, P.BaseValue);
                    SetOutputParameters(ctx, entry, H, P);
                },
                IsOutput(nameof(IThermoEntry.Enthalpy), nameof(IThermoEntry.Pressure)));
        }


        /// <summary>
        /// Looks up each item using the in NodeSettings using the keys passed and checks the UseThisParameters
        /// </summary>
        /// <param name="nodeSettingsKeys">Keys in NodeSettings for each item to be checked</param>
        /// <returns>true if all UseThisParameters are true</returns>
        private bool CheckIfBeingUsed(params string[] nodeSettingsKeys)
        {
            foreach(string name in nodeSettingsKeys)
            {
                if(!NodeSettings[name].UseThisParameter)
                    return false;
            }
            return true;
        }


        private Func<IParameterContainerNode, string, bool> IsOutput(params string[] nodeSettingsKeysForInputs)
        {
            return (ctx, varName) =>             
            {
                foreach (KeyValuePair<string, NodeBuilderParameterSetting> pair in NodeSettings)
                {
                    if (pair.Value.VarName.Equals(varName))
                    {
                        return !IsInput(pair.Value, nodeSettingsKeysForInputs);
                    }
                        
                }
                return false;
            };           
        }

        private bool IsInput(NodeBuilderParameterSetting setting, string[] nodeSettingsKeysForInputs)
        {
            foreach (string name in nodeSettingsKeysForInputs)
            {
                if (NodeSettings[name].Equals(setting))
                {
                    return true;
                }

            }
            return false;
        }        


        private IParameter FindParameter(string nodeSettingsKey)
        {
            string varName = NodeSettings[nodeSettingsKey].VarName;
            return Node.FindParameter(varName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="inputParameters">Will not set these parameter since they are inputs</param>
        /// <returns></returns>
        private void SetOutputParameters(IParameterContainerNode ctx, IThermoEntry entry, params IParameter[] inputParameters)
        {
            foreach (KeyValuePair<string, NodeBuilderParameterSetting> pair in NodeSettings)
            {
                if (!pair.Value.UseThisParameter)
                    continue;

                bool isInput = false;
                foreach (IParameter input in inputParameters)
                {
                    if (input.VarName.Equals(pair.Value.VarName))
                    {
                        isInput = true;
                        break;
                    }                        
                }
                if (isInput)
                    continue;

                IParameter parameter = ctx.FindParameter(pair.Value.VarName);
                SetOutputParameter(parameter, entry, pair.Key, pair.Value);
            }
        }

        private void SetOutputParameter(IParameter parameter, IThermoEntry entry, string propName, NodeBuilderParameterSetting setting)
        {
            if (parameter is NumericParameter numPara && setting is NumericParameterSetting numSet)
            {
                numPara.BaseValue = (double)entry.GetType().GetProperty(propName).GetValue(entry, null);
            }
            else if (parameter is PickerParameter<SaturationRegion> regPara && setting is PickerParameterSetting<SaturationRegion> satSet)
            {
                regPara.ItemAtSelectedIndex = (SaturationRegion)entry.GetType().GetProperty(propName).GetValue(entry, null);
            }
            else
            {
                throw new Exception("Unknown type!");
            }
        }

        public IPVTTable Table { get; }
        private string Header { get; }
        /// <summary>
        /// Where the key is the property name taken from IThermoEntry
        /// </summary>
        public Dictionary<string, NodeBuilderParameterSetting> NodeSettings { get; }


        private static PickerParameterOption<SaturationRegion>[] BuildRegionOptions()
        {
            return new PickerParameterOption<SaturationRegion>[]
            {
                new PickerParameterOption<SaturationRegion>(LibraryResources.Vapor, SaturationRegion.Vapor),
                new PickerParameterOption<SaturationRegion>(LibraryResources.Liquid, SaturationRegion.Liquid),
                new PickerParameterOption<SaturationRegion>(LibraryResources.Solid, SaturationRegion.Solid)
            };
        }
    }
}
