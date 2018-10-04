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
            BoilerBuilder = CreateBuilder(SpeciesSettings.ItemAtSelectedIndex, LibraryResources.Boiler, "boiler");
            PumpBuilder = CreateBuilder(SpeciesSettings.ItemAtSelectedIndex, LibraryResources.Pump, "pump");
            TurbineBuilder = CreateBuilder(SpeciesSettings.ItemAtSelectedIndex, LibraryResources.Turbine, "turbine");
            CondenserBuilder = CreateBuilder(SpeciesSettings.ItemAtSelectedIndex, LibraryResources.Condenser, "");
            SpeciesSettings.IndexChanged += SpeciesSettings_IndexChanged;
            SpeciesSettings.SelectedIndex = 0;
            AllSettings.Add(SpeciesSettings);
            NextNode = new FunctionBranch(LibraryResources.ThermoCycle)
            {
                Children =
                {
                    new FunctionNullNode(LibraryResources.CarnotCycle),
                    new FunctionNullNode(LibraryResources.RankineCycle),
                    new FunctionNullNode(LibraryResources.RegenerativeCycle)
                }
            };
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

        private void SpeciesSettings_IndexChanged(object sender, EventArgs e)
        {
            IPVTTable table = SpeciesSettings.ItemAtSelectedIndex;
            BoilerBuilder.Table = table;
            PumpBuilder.Table = table;
            TurbineBuilder.Table = table;
            CondenserBuilder.Table = table;
        }

        private PVTTableNodeBuilder CreateBuilder(IPVTTable table, string name, string varNamePrefix)
        {
            PVTTableNodeBuilder builder = new PVTTableNodeBuilder(table, name, varNamePrefix);
            ChangeParameterSetting(ref builder, nameof(IThermoEntry.Region), false);
            ChangeParameterSetting(ref builder, nameof(IThermoEntry.VaporMassFraction), false);
            ChangeParameterSetting(ref builder, nameof(IThermoEntry.Region), false);
            ChangeParameterSetting(ref builder, nameof(IThermoEntry.Region), false);
            ChangeParameterSetting(ref builder, nameof(IThermoEntry.Region), false);
            ChangeParameterSetting(ref builder, nameof(IThermoEntry.Region), false);
            ChangeParameterSetting(ref builder, nameof(IThermoEntry.Region), false);
            ChangeParameterSetting(ref builder, nameof(IThermoEntry.Region), false);
            ChangeParameterSetting(ref builder, nameof(IThermoEntry.Region), false);
            ChangeParameterSetting(ref builder, nameof(IThermoEntry.Region), false);
            ChangeParameterSetting(ref builder, nameof(IThermoEntry.Region), false);
            ChangeParameterSetting(ref builder, nameof(IThermoEntry.Region), false);
            ChangeParameterSetting(ref builder, nameof(IThermoEntry.Region), false);
            ChangeParameterSetting(ref builder, nameof(IThermoEntry.Region), false);
            ChangeParameterSetting(ref builder, nameof(IThermoEntry.Region), false);
            ChangeParameterSetting(ref builder, nameof(IThermoEntry.Region), false);
            return builder;
        }

        private void ChangeParameterSetting(ref PVTTableNodeBuilder builder, string parameterSettingKey, bool useThisParameter)
        {
            NodeBuilderParameterSetting setting = builder.NodeSettings[parameterSettingKey];
            setting.UseThisParameter = useThisParameter;
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
