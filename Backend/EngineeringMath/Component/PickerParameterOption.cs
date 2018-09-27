using EngineeringMath.Component.CustomEventArgs;
using EngineeringMath.Resources;
using EngineeringMath.Resources.PVTTables;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EngineeringMath.Component
{
    public interface IPickerParameterOption<T> : IChildItem<IPickerParameter>, ISettingOption
    {
        string DisplayName { get; }
        T ObjectRepresented { get; }
    }

    public class PickerParameterOption<T> : IPickerParameterOption<T>
    {
        protected PickerParameterOption()
        {

        }

        public PickerParameterOption(string displayName, T objectRepresented)
        {
            DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
            if (objectRepresented == null)
                throw new ArgumentNullException(nameof(objectRepresented));
            ObjectRepresented = objectRepresented;
        }

        public string DisplayName { get; }

        public T ObjectRepresented { get; protected set; }

        [XmlIgnore]
        private IPickerParameter _Parent;

        IPickerParameter IChildItem<IPickerParameter>.Parent { get => Parent; set => Parent = value; }
        public IPickerParameter Parent
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
        string IChildItem<IPickerParameter>.Key => DisplayName;

        string ISettingOption.Name => DisplayName;

        public event EventHandler<ParentChangedEventArgs> ParentChanged;
    }

}
