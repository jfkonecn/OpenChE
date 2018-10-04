using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public abstract class ParentlessSetting<T> : NotifyPropertyChangedExtension, ISetting
    {
        private int _SelectedIndex = 0;
        public int SelectedIndex
        {
            get
            {
                return _SelectedIndex;
            }
            set
            {
                _SelectedIndex = value;
                OnIndexChanged();
                OnPropertyChanged(nameof(ItemAtSelectedIndex));
                OnPropertyChanged(nameof(SelectOptionStr));
                OnPropertyChanged();
            }
        }
        protected void OnIndexChanged()
        {
            IndexChanged?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler IndexChanged;

        public abstract T ItemAtSelectedIndex { get; }
        public abstract IList<string> AllOptions { get; }

        public SettingState CurrentState { get; internal set; } = SettingState.Active;

        public abstract string SelectOptionStr { get; }

        public abstract string Name { get; }
    }
}
