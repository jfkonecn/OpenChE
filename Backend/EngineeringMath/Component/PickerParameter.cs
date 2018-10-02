using EngineeringMath.Component.CustomEventArgs;
using EngineeringMath.Resources;
using EngineeringMath.Resources.PVTTables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace EngineeringMath.Component
{
    public interface IPickerParameter : IParameter, ISetting
    {


    }
    public class PickerParameter<T> : NotifyPropertyChangedExtension, IPickerParameter
    {


        protected PickerParameter()
        {
            FinishUp();
        }

        public PickerParameter(string displayName, string varName, params IPickerParameterOption<T>[] options)
        {
            DisplayName = displayName;
            VarName = varName;
            OptionsList = new SelectableList<IPickerParameterOption<T>, IPickerParameter>(string.Empty, null as IPickerParameter);
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            foreach (IPickerParameterOption<T> obj in options)
            {
                if (obj == null)
                    throw new ArgumentNullException($"Item from {nameof(options)}");
                OptionsList.Add(obj);
            }
            FinishUp();
        }

        private void FinishUp()
        {
            OptionsList.IndexChanged += OptionsList_IndexChanged;
        }

        private void OptionsList_IndexChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(SelectedIndex));
            OnPropertyChanged(nameof(ItemAtSelectedIndex));
            OnPropertyChanged(nameof(DisplayDetail));
            OnPropertyChanged(nameof(SelectOptionStr));
        }

        protected SelectableList<IPickerParameterOption<T>, IPickerParameter> OptionsList;



        #region IPickerParameterMembers

        public string DisplayName { get; }

        public string DisplayDetail
        {
            get
            {
                return OptionsList.SelectOptionStr;
            }
        }

        public string VarName { get; }

        private void OnStateChanged()
        {
            StateChanged?.Invoke(this, EventArgs.Empty);
        }
        private ParameterState _CurrentState = ParameterState.Inactive;
        public ParameterState CurrentState
        {
            get
            {
                return _CurrentState;
            }
            set
            {
                if (CurrentState == value)
                    return;
                _CurrentState = value;
                OnStateChanged();
                OnPropertyChanged();
            }
        }
        [XmlIgnore]
        private IParameterContainerNode _Parent;

        IParameterContainerNode IChildItem<IParameterContainerNode>.Parent { get => Parent; set => Parent = value; }
        public IParameterContainerNode Parent
        {
            get
            {
                return _Parent;
            }
            internal set
            {
                IChildItemDefaults.DefaultSetParent(ref _Parent, OnParentChanged, value, Parent_ParentChanged);
            }
        }

        protected virtual void OnParentChanged(ParentChangedEventArgs e)
        {
            ParentChanged?.Invoke(this, e);
        }
        private void Parent_ParentChanged(object sender, ParentChangedEventArgs e)
        {
            OnParentChanged(e);
        }


        public int SelectedIndex
        {
            get
            {
                return OptionsList.SelectedIndex;
            }
            set
            {
                OptionsList.SelectedIndex = value;
                // property change handled in OptionsList.IndexChanged event
            }
        }


        public T ItemAtSelectedIndex
        {
            get
            {
                if (OptionsList.ItemAtSelectedIndex == null)
                    return default(T);
                return OptionsList.ItemAtSelectedIndex.ObjectRepresented;
            }
            set
            {
                foreach(IPickerParameterOption<T> obj in OptionsList)
                {
                    if (obj != null && obj.ObjectRepresented.Equals(value))
                        OptionsList.ItemAtSelectedIndex = obj;
                }
                // property change handled in OptionsList.IndexChanged event
            }
        }

        public IList<string> AllOptions { get { return OptionsList.AllOptions; } }

        public string SelectOptionStr { get { return OptionsList.SelectOptionStr; } }

        string IChildItem<IParameterContainerNode>.Key => VarName;

        SettingState ISetting.CurrentState => OptionsList.CurrentState;

        string ISetting.Name => DisplayName;

        public event EventHandler<EventArgs> StateChanged;
        public event EventHandler<ParentChangedEventArgs> ParentChanged;
        #endregion
    }
    public class PickerParamterPTVRegion : PickerParameter<Region>
    {
        protected PickerParamterPTVRegion() : base()
        {

        }
        public PickerParamterPTVRegion(string displayName, string varName) : base(displayName, varName, GetPickerParameterOptions())
        {

        }


        private static IPickerParameterOption<Region>[] GetPickerParameterOptions()
        {
            List<IPickerParameterOption<Region>> list = new List<IPickerParameterOption<Region>>();
            foreach (Region region in Enum.GetValues(typeof(Region)))
            {
                list.Add(new PickerParameterOption<Region>(RegionDisplayName(region), region));
            }
            return list.ToArray();
        }

        private static string RegionDisplayName(Region region)
        {
            switch (region)
            {
                case Region.SupercriticalFluid:
                    return LibraryResources.SupercriticalFluid;
                case Region.Gas:
                    return LibraryResources.Gas;
                case Region.Vapor:
                    return LibraryResources.Vapor;
                case Region.Liquid:
                    return LibraryResources.Liquid;
                case Region.Solid:
                    return LibraryResources.Solid;
                case Region.SolidLiquid:
                    return LibraryResources.SolidLiquid;
                case Region.LiquidVapor:
                    return LibraryResources.LiquidVapor;
                case Region.SolidVapor:
                    return LibraryResources.SolidVapor;
                case Region.SolidLiquidVapor:
                    return LibraryResources.SolidLiquidVapor;
                default:
                    return string.Empty;
            }
        }
    }
}
