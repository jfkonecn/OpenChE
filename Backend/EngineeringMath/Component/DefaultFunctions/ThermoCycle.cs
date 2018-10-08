using EngineeringMath.Resources;
using EngineeringMath.Resources.PVTTables;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Linq;
using EngineeringMath.Component.Builder;

namespace EngineeringMath.Component.DefaultFunctions
{
    public class ThermoCycle : Function
    {
        public ThermoCycle() : base(LibraryResources.ThermoCycle, LibraryResources.Thermodynamics, false)
        {            
            SpeciesSettings = new SpeciesSetting();
            BoilerBuilder = CreateBuilder(LibraryResources.Boiler, 
                "boiler", PVTTableNodeBuilder.BuildTempAndPreVisitor);
            PumpBuilder = CreateBuilder(LibraryResources.Pump, 
                "pump", PVTTableNodeBuilder.BuildEnthalpyAndPressureVisitor);
            TurbineBuilder = CreateBuilder(LibraryResources.Turbine, 
                "turbine", PVTTableNodeBuilder.BuildEntropyAndPressureVisitor);
            CondenserBuilder = CreateBuilder( LibraryResources.Condenser, 
                "condenser", PVTTableNodeBuilder.BuildSatPressureVisitor);
            SpeciesSettings.IndexChanged += SpeciesSettings_IndexChanged;
            SpeciesSettings.SelectedIndex = 0;
            AllSettings.Add(SpeciesSettings);
            BuildNextNode();
        }
        [XmlIgnore]
        private SpeciesSetting SpeciesSettings { get; }
        [XmlIgnore]
        private PVTTableNodeBuilder BoilerBuilder { get; }
        [XmlIgnore]
        private PVTTableNodeBuilder PumpBuilder { get; }
        [XmlIgnore]
        private PVTTableNodeBuilder TurbineBuilder { get; }
        [XmlIgnore]
        private PVTTableNodeBuilder CondenserBuilder { get; }
        [XmlIgnore]
        private SIUnitParameter PumpWorkParameter { get; } 
            = new SIUnitParameter($"{LibraryResources.Pump}: {LibraryResources.Work}", "pumpQ", LibraryResources.Enthalpy);
        [XmlIgnore]
        private SIUnitParameter CondenserWorkParameter { get; }
            = new SIUnitParameter($"{LibraryResources.Condenser}: {LibraryResources.Work}", "condQ", LibraryResources.Enthalpy);
        [XmlIgnore]
        private SIUnitParameter TurbineWorkParameter { get; }
            = new SIUnitParameter($"{LibraryResources.Turbine}: {LibraryResources.Work}", "turbQ", LibraryResources.Enthalpy);
        [XmlIgnore]
        private SIUnitParameter BoilerWorkParameter { get; }
            = new SIUnitParameter($"{LibraryResources.Boiler}: {LibraryResources.Work}", "boilQ", LibraryResources.Enthalpy);
        [XmlIgnore]
        private SIUnitParameter NetWorkParameter { get; }
            = new SIUnitParameter($"{LibraryResources.ThermoCycle}: {LibraryResources.Work}", "netQ", LibraryResources.Enthalpy);
        [XmlIgnore]
        private UnitlessParameter PumpEfficiencyParameter { get; } 
            = new UnitlessParameter($"{LibraryResources.Pump}: {LibraryResources.Efficiency}", "pumpEff", minValue: 0, maxValue: 1);
        [XmlIgnore]
        private UnitlessParameter TurbineEfficiencyParameter { get; }
            = new UnitlessParameter($"{LibraryResources.Turbine}: {LibraryResources.Efficiency}", "turbEff", minValue: 0, maxValue: 1);
        [XmlIgnore]
        private UnitlessParameter ThermoEfficiencyParameter { get; }
            = new UnitlessParameter($"{LibraryResources.ThermoCycle}: {LibraryResources.Efficiency}", "thermEff", minValue: 0, maxValue: 1);
        private void SpeciesSettings_IndexChanged(object sender, EventArgs e)
        {
            IPVTTable table = SpeciesSettings.ItemAtSelectedIndex;
            BoilerBuilder.Table = table;
            PumpBuilder.Table = table;
            TurbineBuilder.Table = table;
            CondenserBuilder.Table = table;
        }

        private void BuildNextNode()
        {
            FunctionQueueNode node = new FunctionQueueNode()
            {
                Parameters =
                {
                    PumpWorkParameter,
                    PumpEfficiencyParameter,
                    CondenserWorkParameter,
                    TurbineWorkParameter,
                    BoilerWorkParameter,
                    NetWorkParameter,
                    TurbineEfficiencyParameter,
                    ThermoEfficiencyParameter
                },
                Children =
                {
                    new PriorityFunctionBranch(1, CreateVisitableNode(BoilerBuilder)),
                    new PriorityFunctionBranch(1, CreateCondenserRegLeaf()),
                    new PriorityFunctionBranch(2, CreateEqualToLeaf(nameof(IThermoEntry.Entropy), TurbineBuilder, BoilerBuilder)),
                    new PriorityFunctionBranch(2, CreateEqualToLeaf(nameof(IThermoEntry.Pressure), CondenserBuilder, TurbineBuilder)),
                    new PriorityFunctionBranch(2, CreateEqualToLeaf(nameof(IThermoEntry.Pressure), PumpBuilder, BoilerBuilder)),
                    new PriorityFunctionBranch(3, CreateVisitableNode(TurbineBuilder)),
                    new PriorityFunctionBranch(3, CreateVisitableNode(CondenserBuilder)),
                    new PriorityFunctionBranch(4, CreatePumpWorkLeaf()),                    
                    new PriorityFunctionBranch(5, CreatePumpEnthalpyLeaf()),
                    new PriorityFunctionBranch(6, CreateBoilerWorkLeaf()),
                    new PriorityFunctionBranch(6, CreateTurbineWorkLeaf()),
                    new PriorityFunctionBranch(7, CreateVisitableNode(PumpBuilder)),
                    new PriorityFunctionBranch(8, CreateCondenserWorkLeaf()),
                    new PriorityFunctionBranch(9, CreateNetWorkLeaf()),
                    new PriorityFunctionBranch(9, CreateThermEffLeaf())
                }
            };
            node.Parameters.AddRange(BoilerBuilder.BuildNonautoBuildParameters().Values);
            node.Parameters.AddRange(PumpBuilder.BuildNonautoBuildParameters().Values);
            node.Parameters.AddRange(TurbineBuilder.BuildNonautoBuildParameters().Values);
            node.Parameters.AddRange(CondenserBuilder.BuildNonautoBuildParameters().Values);
            NextNode = node;
        }

        private FunctionLeaf CreateEqualToLeaf(
            string thermoEntyPropName, 
            PVTTableNodeBuilder setThis, 
            PVTTableNodeBuilder equalTothis)
        {
            string set = setThis.ParameterSettingsDic[thermoEntyPropName].VarName,
                get = equalTothis.ParameterSettingsDic[thermoEntyPropName].VarName;
            return new FunctionLeaf($"${get}", $"{set}");
        }

        private FunctionLeaf CreatePumpWorkLeaf()
        {
            string condVol = CondenserBuilder.ParameterSettingsDic[nameof(IThermoEntry.SpecificVolume)].VarName,
                turPre = TurbineBuilder.ParameterSettingsDic[nameof(IThermoEntry.Pressure)].VarName,
                boilPre = BoilerBuilder.ParameterSettingsDic[nameof(IThermoEntry.Pressure)].VarName,
                pumpEff = PumpEfficiencyParameter.VarName;
            return new FunctionLeaf($"${condVol} * (${boilPre} - ${turPre}) / ${pumpEff}", $"{PumpWorkParameter.VarName}");
        }

        private FunctionLeaf CreatePumpEnthalpyLeaf()
        {
            string pumpQ = PumpWorkParameter.VarName,
                pumpH = PumpBuilder.ParameterSettingsDic[nameof(IThermoEntry.Enthalpy)].VarName,
                condH = CondenserBuilder.ParameterSettingsDic[nameof(IThermoEntry.Enthalpy)].VarName;
            return new FunctionLeaf($"${pumpQ} + ${condH}", $"{pumpH}");
        }

        private FunctionLeaf CreateBoilerWorkLeaf()
        {
            string boilH = BoilerBuilder.ParameterSettingsDic[nameof(IThermoEntry.Enthalpy)].VarName,
                pumpH = PumpBuilder.ParameterSettingsDic[nameof(IThermoEntry.Enthalpy)].VarName,
                boilQ = BoilerWorkParameter.VarName;
            return new FunctionLeaf($"${boilH} - ${pumpH}", $"{boilQ}");
        }

        private FunctionLeaf CreateTurbineWorkLeaf()
        {
            string turQ = TurbineWorkParameter.VarName,
                turEff = TurbineEfficiencyParameter.VarName,
                boilH = BoilerBuilder.ParameterSettingsDic[nameof(IThermoEntry.Enthalpy)].VarName,
                turbH = TurbineBuilder.ParameterSettingsDic[nameof(IThermoEntry.Enthalpy)].VarName;
            return new FunctionLeaf($"(${boilH} - ${turbH}) * ${turEff}", $"{turQ}");
        }

        private FunctionLeaf CreateCondenserWorkLeaf()
        {
            string turQ = TurbineWorkParameter.VarName,
                condH = CondenserBuilder.ParameterSettingsDic[nameof(IThermoEntry.Enthalpy)].VarName,
                boilH = BoilerBuilder.ParameterSettingsDic[nameof(IThermoEntry.Enthalpy)].VarName,
                condQ = CondenserWorkParameter.VarName;
            return new FunctionLeaf($"${boilH} - ${condH} - ${turQ}", $"{condQ}");
        }

        private FunctionLeaf CreateNetWorkLeaf()
        {
            string pumpQ = PumpWorkParameter.VarName,
                turbQ = TurbineWorkParameter.VarName,
                netQ = NetWorkParameter.VarName;
            return new FunctionLeaf($"${turbQ} - ${pumpQ}", $"{netQ}");
        }

        private FunctionLeaf CreateThermEffLeaf()
        {
            string turQ = TurbineWorkParameter.VarName,
                turEff = TurbineEfficiencyParameter.VarName,
                boilQ = BoilerWorkParameter.VarName,
                thermEff = ThermoEfficiencyParameter.VarName;
            return new FunctionLeaf($"Abs(${turQ} / ${turEff}) / ${boilQ}", $"{thermEff}");
        }

        private FunctionSetterLeaf CreateCondenserRegLeaf()
        {
            string satReg = CondenserBuilder.SatRegionBuilder.VarName;
            return new FunctionSetterLeaf(string.Empty, satReg, 
                (para) => 
                {
                    ((PickerParameter<SaturationRegion>)para).ItemAtSelectedIndex = SaturationRegion.Liquid;
                });
        }

        private FunctionVisitableNodeLeaf CreateVisitableNode(PVTTableNodeBuilder builder)
        {
            VisitableNodeDirector director = new VisitableNodeDirector()
            {
                NodeBuilder = builder
            };
            director.BuildNode(builder.Header);
            return director.Node;
        }

        private PVTTableNodeBuilder CreateBuilder(string name, string varNamePrefix, 
            Func<PVTTableNodeBuilder, FunctionVisitor> visBuilder = null)
        {
            PVTTableNodeBuilder builder = new PVTTableNodeBuilder(SpeciesSettings.ItemAtSelectedIndex, name, varNamePrefix)
            {
                BuildVisitorFunction = visBuilder
            };
            ChangeParameterSetting(ref builder, nameof(IThermoEntry.Region), true);
            ChangeParameterSetting(ref builder, nameof(IThermoEntry.VaporMassFraction), true);
            ChangeParameterSetting(ref builder, nameof(IThermoEntry.LiquidMassFraction), true);
            ChangeParameterSetting(ref builder, nameof(IThermoEntry.SolidMassFraction), true);
            ChangeParameterSetting(ref builder, nameof(IThermoEntry.Temperature), true);
            ChangeParameterSetting(ref builder, nameof(IThermoEntry.Pressure), true);
            ChangeParameterSetting(ref builder, nameof(IThermoEntry.SpecificVolume), true);
            ChangeParameterSetting(ref builder, nameof(IThermoEntry.InternalEnergy), false);
            ChangeParameterSetting(ref builder, nameof(IThermoEntry.Enthalpy), true);
            ChangeParameterSetting(ref builder, nameof(IThermoEntry.Entropy), true);
            ChangeParameterSetting(ref builder, nameof(IThermoEntry.IsochoricHeatCapacity), false);
            ChangeParameterSetting(ref builder, nameof(IThermoEntry.IsobaricHeatCapacity), false);
            ChangeParameterSetting(ref builder, nameof(IThermoEntry.SpeedOfSound), false);
            ChangeParameterSetting(ref builder, nameof(IThermoEntry.Density), false);
            return builder;
        }

        private void ChangeParameterSetting(ref PVTTableNodeBuilder builder, string parameterSettingKey, bool useThisParameter)
        {
            NodeBuilderParameterSetting setting = builder.ParameterSettingsDic[parameterSettingKey];
            setting.UseThisParameter = useThisParameter;
            setting.AutoBuildParameter = false;
        }

        private class SpeciesSetting : ParentlessSetting<IPVTTable>
        {
            [XmlIgnore]
            private Dictionary<string, IPVTTable> SpeciesDic { get; } = new Dictionary<string, IPVTTable>()
            {
                { LibraryResources.Water, Resources.PVTTables.SteamTable.Table }
            };

            public override IPVTTable ItemAtSelectedIndex => SpeciesDic.Values.ToList()[SelectedIndex];

            public override IList<string> AllOptions => SpeciesDic.Keys.ToList();

            public override string SelectOptionStr => AllOptions[SelectedIndex];

            public override string Name => LibraryResources.Species;
        }

    }
}
