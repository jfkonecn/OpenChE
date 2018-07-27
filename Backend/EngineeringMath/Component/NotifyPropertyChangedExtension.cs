using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EngineeringMath.Component
{
    /// <summary>
    /// Implements INotifyPropertyChanged
    /// </summary>
    public abstract class NotifyPropertyChangedExtension : INotifyPropertyChanged
    {
        protected void OnPropertyChanged([CallerMemberName]string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
