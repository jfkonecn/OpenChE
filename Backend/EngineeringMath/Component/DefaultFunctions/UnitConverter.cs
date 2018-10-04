using EngineeringMath.Component.CustomEventArgs;
using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EngineeringMath.Component.DefaultFunctions
{
    public class UnitConverter : Function
    {
        public UnitConverter() : base(LibraryResources.UnitConverter, LibraryResources.Utility, false)
        {
            Options.IndexChanged += Options_IndexChanged;
            AllSettings.Add(Options);
            // ensure the settings properly update
            Options.SelectedIndex = 0;
            NextNode = new FunctionLeaf("$in", "out")
            {
                Parameters =
                {
                    InPara,
                    OutPara
                }
            };
        }

        private void Options_IndexChanged(object sender, EventArgs e)
        {
            if (Options.SelectedIndex < 0)
                return;
            InPara.UnitCategoryName = Options.SelectOptionStr;
            OutPara.UnitCategoryName = Options.SelectOptionStr;
        }
        [XmlIgnore]
        private UnitOptions Options { get; } = new UnitOptions();
        [XmlIgnore]
        private SIUnitParameter InPara { get; } = new SIUnitParameter(LibraryResources.ConvertFrom, "in", LibraryResources.Length);
        [XmlIgnore]
        private SIUnitParameter OutPara { get; } = new SIUnitParameter(LibraryResources.ConvertTo, "out", LibraryResources.Length);

        private class UnitOptions : ParentlessSetting<UnitCategory>
        {
            public override UnitCategory ItemAtSelectedIndex
            {
                get
                {
                    return MathManager.AllUnits.GetCategoryByName(SelectOptionStr);
                }
            }
            public override IList<string> AllOptions => MathManager.AllUnits.Children.Select((x) => x.Name).ToList();

            public override string SelectOptionStr => AllOptions[SelectedIndex];

            public override string Name => LibraryResources.UnitType;
        }
    }
}
