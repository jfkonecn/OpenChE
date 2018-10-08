using EngineeringMath.Resources;
using EngineeringMath.Resources.PVTTables;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EngineeringMath.Component.Builder
{
    public class PVTTableNodeBuilder : VisitableNodeBuilder
    {
        public static PVTTableNodeBuilder SteamTableBuilder()
        {
            return new PVTTableNodeBuilder(SteamTable.Table, LibraryResources.SteamTable, "steamTable");
        }


        public PVTTableNodeBuilder(IPVTTable table, string header, string varNamePrefix)
        {
            Table = table;
            Header = header;
            PrefixVarName = varNamePrefix;
            SatRegionBuilder = new PickerParameterSetting<SaturationRegion>(BuildDisplayName(LibraryResources.SatRegion), 
                BuildVarName("satRegion"), BuildSaturationRegionOptions());
            ParameterSettingsDic = new Dictionary<string, NodeBuilderParameterSetting>()
            {
                { nameof(IThermoEntry.Region),
                    new PickerParameterSetting<Region>(BuildDisplayName(LibraryResources.ThermoRegion), BuildVarName("region"), BuildRegionOptions()) },
                { nameof(IThermoEntry.VaporMassFraction),
                    new UnitlessParameterSetting(BuildDisplayName(LibraryResources.VaporFraction), BuildVarName("xv"), 0, 1) },
                { nameof(IThermoEntry.LiquidMassFraction),
                    new UnitlessParameterSetting(BuildDisplayName(LibraryResources.LiquidFraction), BuildVarName("xl"), 0, 1) },
                { nameof(IThermoEntry.SolidMassFraction),
                    new UnitlessParameterSetting(BuildDisplayName(LibraryResources.SolidFraction), BuildVarName("xs"), 0, 1) },
                { nameof(IThermoEntry.Temperature),
                    new SIParameterSetting(BuildDisplayName(LibraryResources.Temperature), BuildVarName("T"), LibraryResources.Temperature, 
                    Table.MinTemperature, Table.MaxTemperature) },
                { nameof(IThermoEntry.Pressure),
                    new SIParameterSetting(BuildDisplayName(LibraryResources.Pressure), BuildVarName("P"), LibraryResources.Pressure, 
                    Table.MinPressure, Table.MaxPressure) },
                { nameof(IThermoEntry.SpecificVolume),
                    new SIParameterSetting(BuildDisplayName(LibraryResources.SpecificVolume), BuildVarName("Vs"), LibraryResources.SpecificVolume) },
                { nameof(IThermoEntry.InternalEnergy),
                    new SIParameterSetting(BuildDisplayName(LibraryResources.InternalEnergy), BuildVarName("U"), LibraryResources.Energy) },
                { nameof(IThermoEntry.Enthalpy),
                    new SIParameterSetting(BuildDisplayName(LibraryResources.Enthalpy), BuildVarName("H"), LibraryResources.Enthalpy) },
                { nameof(IThermoEntry.Entropy),
                    new SIParameterSetting(BuildDisplayName(LibraryResources.Entropy), BuildVarName("S"), LibraryResources.Entropy) },
                { nameof(IThermoEntry.IsobaricHeatCapacity),
                    new SIParameterSetting(BuildDisplayName(LibraryResources.IsobaricHeatCapacity), BuildVarName("cp"), LibraryResources.Entropy) },
                { nameof(IThermoEntry.IsochoricHeatCapacity),
                    new SIParameterSetting(BuildDisplayName(LibraryResources.IsochoricHeatCapacity), BuildVarName("cv"), LibraryResources.Entropy) },
                { nameof(IThermoEntry.SpeedOfSound),
                    new SIParameterSetting(BuildDisplayName(LibraryResources.Velocity), BuildVarName("u"), LibraryResources.Velocity) },
                { nameof(IThermoEntry.Density),
                    new SIParameterSetting(BuildDisplayName(LibraryResources.Density), BuildVarName("rho"), LibraryResources.Density) }
            };
        }

        private string BuildDisplayName(string detail)
        {
            return $"{Header}: {detail}";
        }
        private string BuildVarName(string baseVarName)
        {
            return $"{PrefixVarName}_{baseVarName}";
        }
        public override void BuildNode(string name)
        {
            if(BuildVisitorFunction != null)
            {
                Node = new FunctionVisitableNodeLeaf(name, BuildVisitorFunction(this));
                return;
            }
            FunctionSelectableVisitableNode visNode = new FunctionSelectableVisitableNode(name);
            BuildVisitorOptions(ref visNode);
            Node = visNode;
        }

        public override void BuildParameters()
        {
            foreach (NodeBuilderParameterSetting setting in ParameterSettingsDic.Values)
            {
                if (setting.UseThisParameter && setting.AutoBuildParameter)
                    Node.Parameters.Add(setting.BuildParameter());
            }
            if (CheckIfBeingUsed(nameof(Region)))
            {
                SatRegionParameter = SatRegionBuilder.BuildParameter() as PickerParameter<SaturationRegion>;
                Node.Parameters.Add(SatRegionParameter);
            }
            if(Node is FunctionSelectableVisitableNode selNode)
            {
                selNode.VisitorOptions.SelectedIndex = 0;
            }
                
        }

        private void BuildVisitorOptions(ref FunctionSelectableVisitableNode node)
        {
            node.VisitorOptions = 
                new SelectableList<FunctionVisitor, IParameterContainerNode>(BuildDisplayName(LibraryResources.FindThermoDataWith), node);

            bool buildTempAndPre = CheckIfBeingUsed(nameof(IThermoEntry.Temperature), nameof(IThermoEntry.Pressure)),
                buildSatPre = CheckIfBeingUsed(nameof(IThermoEntry.Region), nameof(IThermoEntry.Pressure)),
                buildSatTemp = CheckIfBeingUsed(nameof(IThermoEntry.Region), nameof(IThermoEntry.Temperature)),
                buildSPreTemp = CheckIfBeingUsed(nameof(IThermoEntry.Entropy), nameof(IThermoEntry.Pressure)),
                buildHPreTemp = CheckIfBeingUsed(nameof(IThermoEntry.Enthalpy), nameof(IThermoEntry.Pressure));

            if (buildTempAndPre)
                node.VisitorOptions.Add(BuildTempAndPreVisitor(this));
            if (buildSatPre)
                node.VisitorOptions.Add(BuildSatPressureVisitor(this));
            if (buildSatTemp)
                node.VisitorOptions.Add(BuildSatTempVisitor(this));
            if (buildSPreTemp)
                node.VisitorOptions.Add(BuildEntropyAndPressureVisitor(this));
            if (buildHPreTemp)
                node.VisitorOptions.Add(BuildEnthalpyAndPressureVisitor(this));

        }

        public static FunctionVisitor BuildTempAndPreVisitor(PVTTableNodeBuilder builder)
        {
            return new FunctionVisitor(LibraryResources.TemperatureAndPressure,
                (ctx) =>
                {
                    NumericParameter T = (NumericParameter)builder.FindParameter(nameof(IThermoEntry.Temperature)),
                    P = (NumericParameter)builder.FindParameter(nameof(IThermoEntry.Pressure));
                    IThermoEntry entry = builder.Table.GetThermoEntryAtTemperatureAndPressure(T.BaseValue, P.BaseValue);
                    builder.SetOutputParameters(ctx, entry, T, P);
                },
                builder.DetermineState(
                    new NodeBuilderParameterSetting[] 
                    {
                        builder.ParameterSettingsDic[nameof(IThermoEntry.Temperature)],
                        builder.ParameterSettingsDic[nameof(IThermoEntry.Pressure)]
                    },
                    new NodeBuilderParameterSetting[]
                    {
                        builder.SatRegionBuilder
                    }
                ));
        }


        public static FunctionVisitor BuildSatPressureVisitor(PVTTableNodeBuilder builder)
        {
            return new FunctionVisitor(LibraryResources.SatPressure,
                (ctx) =>
                {
                    NumericParameter P = (NumericParameter)builder.FindParameter(nameof(IThermoEntry.Pressure));
                    IThermoEntry entry = builder.Table.GetThermoEntryAtSatPressure(P.BaseValue, builder.SatRegionParameter.ItemAtSelectedIndex);
                    builder.SetOutputParameters(ctx, entry, P, builder.SatRegionParameter);
                },
                builder.DetermineState(
                    new NodeBuilderParameterSetting[]
                    {
                        builder.SatRegionBuilder,
                        builder.ParameterSettingsDic[nameof(IThermoEntry.Pressure)]
                    },
                    new NodeBuilderParameterSetting[]
                    {
                        builder.ParameterSettingsDic[nameof(IThermoEntry.Region)]
                    }
                ));
        }

        public static FunctionVisitor BuildSatTempVisitor(PVTTableNodeBuilder builder)
        {
            return new FunctionVisitor(LibraryResources.SatTemperature,
                (ctx) =>
                {
                    NumericParameter T = (NumericParameter)builder.FindParameter(nameof(IThermoEntry.Temperature));
                    IThermoEntry entry = builder.Table.GetThermoEntryAtSatTemp(T.BaseValue, builder.SatRegionParameter.ItemAtSelectedIndex);
                    builder.SetOutputParameters(ctx, entry, T, builder.SatRegionParameter);
                },
                builder.DetermineState(
                    new NodeBuilderParameterSetting[]
                    {
                        builder.SatRegionBuilder,
                        builder.ParameterSettingsDic[nameof(IThermoEntry.Temperature)]
                    },
                    new NodeBuilderParameterSetting[]
                    {
                        builder.ParameterSettingsDic[nameof(IThermoEntry.Region)]
                    }));
        }

        public static FunctionVisitor BuildEntropyAndPressureVisitor(PVTTableNodeBuilder builder)
        {
            return new FunctionVisitor(LibraryResources.EntropyAndPressure,
                (ctx) =>
                {
                    NumericParameter S = (NumericParameter)builder.FindParameter(nameof(IThermoEntry.Entropy)),
                    P = (NumericParameter)builder.FindParameter(nameof(IThermoEntry.Pressure));
                    IThermoEntry entry = builder.Table.GetThermoEntryAtEntropyAndPressure(S.BaseValue, P.BaseValue);
                    builder.SetOutputParameters(ctx, entry, S, P);
                },
                builder.DetermineState(
                    new NodeBuilderParameterSetting[]
                    {
                        builder.ParameterSettingsDic[nameof(IThermoEntry.Entropy)],
                        builder.ParameterSettingsDic[nameof(IThermoEntry.Pressure)]
                    },
                    new NodeBuilderParameterSetting[]
                    {
                        builder.SatRegionBuilder
                    }
                ));
        }

        public static FunctionVisitor BuildEnthalpyAndPressureVisitor(PVTTableNodeBuilder builder)
        {
            return new FunctionVisitor(LibraryResources.EnthalpyAndPressure,
                (ctx) =>
                {
                    NumericParameter H = (NumericParameter)builder.FindParameter(nameof(IThermoEntry.Enthalpy)),
                    P = (NumericParameter)builder.FindParameter(nameof(IThermoEntry.Pressure));
                    IThermoEntry entry = builder.Table.GetThermoEntryAtEnthalpyAndPressure(H.BaseValue, P.BaseValue);
                    builder.SetOutputParameters(ctx, entry, H, P);
                },
                builder.DetermineState(
                    new NodeBuilderParameterSetting[]
                    {
                        builder.ParameterSettingsDic[nameof(IThermoEntry.Enthalpy)],
                        builder.ParameterSettingsDic[nameof(IThermoEntry.Pressure)]
                    },
                    new NodeBuilderParameterSetting[]
                    {
                        builder.SatRegionBuilder
                    }
                ));
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
                if(!ParameterSettingsDic[name].UseThisParameter)
                    return false;
            }
            return true;
        }


        private Func<IParameterContainerNode, string, ParameterState> DetermineState(
            NodeBuilderParameterSetting[] inputArr,
            NodeBuilderParameterSetting[] inactiveArr)
        {
            return (ctx, varName) =>             
            {
                foreach(NodeBuilderParameterSetting setting in inputArr)
                {
                    if (setting.VarName.Equals(varName))
                        return ParameterState.Input;
                }
                foreach(NodeBuilderParameterSetting setting in inactiveArr)
                {
                    if (setting.VarName.Equals(varName))
                        return ParameterState.Inactive;
                }
                NodeBuilderParameterSetting temp 
                    = ParameterSettings.SingleOrDefault((x) => { return x.VarName.Equals(varName); });
                if(temp == null)
                    return ParameterState.Inactive;
                return ParameterState.Output;
            };           
        }

        private bool IsInput(NodeBuilderParameterSetting setting, string[] nodeSettingsKeysForInputs)
        {
            foreach (string name in nodeSettingsKeysForInputs)
            {
                if (ParameterSettingsDic[name].Equals(setting))
                {
                    return true;
                }

            }
            return false;
        }        


        private IParameter FindParameter(string nodeSettingsKey)
        {
            string varName = ParameterSettingsDic[nodeSettingsKey].VarName;
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
            if (null == entry)
                entry = new ThermoEntry(Region.SupercriticalFluid, 
                    double.NaN, double.NaN, double.NaN, 
                    double.NaN, double.NaN, double.NaN, 
                    double.NaN, double.NaN, double.NaN, 
                    0, 0, 0);
            foreach (KeyValuePair<string, NodeBuilderParameterSetting> pair in ParameterSettingsDic)
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
            else if (parameter is PickerParameter<SaturationRegion> satRegPara && setting is PickerParameterSetting<SaturationRegion> satSet)
            {
                satRegPara.ItemAtSelectedIndex = (SaturationRegion)entry.GetType().GetProperty(propName).GetValue(entry, null);
            }
            else if (parameter is PickerParameter<Region> regPara && setting is PickerParameterSetting<Region> regSet)
            {
                regPara.ItemAtSelectedIndex = (Region)entry.GetType().GetProperty(propName).GetValue(entry, null);
            }
            else
            {
                throw new Exception("Unknown type!");
            }
        }

        public override Dictionary<string, IParameter> BuildNonautoBuildParameters()
        {
            Dictionary<string, IParameter> dic = base.BuildNonautoBuildParameters();
            if (dic.ContainsKey(ParameterSettingsDic[nameof(IThermoEntry.Region)].VarName))
                dic.Add($"{SatRegionBuilder.VarName}", SatRegionBuilder.BuildParameter());
            return dic;
        }

        public IPVTTable Table { get; internal set; }
        public string Header { get; }
        public string PrefixVarName { get; }
        /// <summary>
        /// If this is null the builder will auto-build a selection of visitor else only the visitor created by this function will be used
        /// </summary>
        public Func<PVTTableNodeBuilder, FunctionVisitor> BuildVisitorFunction { get; set; } = null;


        public PickerParameterSetting<SaturationRegion> SatRegionBuilder { get; } 
        private PickerParameter<SaturationRegion> SatRegionParameter { get; set; }


        /// <summary>
        /// Where the key is the property name taken from IThermoEntry
        /// </summary>
        public Dictionary<string, NodeBuilderParameterSetting> ParameterSettingsDic { get; }

        public override IEnumerable<NodeBuilderParameterSetting> ParameterSettings => ParameterSettingsDic.Values;

        private static PickerParameterOption<Region>[] BuildRegionOptions()
        {
            return new PickerParameterOption<Region>[]
            {
                new PickerParameterOption<Region>(LibraryResources.SupercriticalFluid, Region.SupercriticalFluid),
                new PickerParameterOption<Region>(LibraryResources.Gas, Region.Gas),
                new PickerParameterOption<Region>(LibraryResources.Vapor, Region.Vapor),
                new PickerParameterOption<Region>(LibraryResources.Liquid, Region.Liquid),
                new PickerParameterOption<Region>(LibraryResources.Solid, Region.Solid),
                new PickerParameterOption<Region>(LibraryResources.SolidLiquid, Region.SolidLiquid),
                new PickerParameterOption<Region>(LibraryResources.LiquidVapor, Region.LiquidVapor),
                new PickerParameterOption<Region>(LibraryResources.SolidVapor, Region.SolidVapor),
                new PickerParameterOption<Region>(LibraryResources.SolidLiquidVapor, Region.SolidLiquidVapor)
            };
        }

        private static PickerParameterOption<SaturationRegion>[] BuildSaturationRegionOptions()
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
