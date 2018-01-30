using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace EngineeringMath.Calculations.Components
{
    public abstract class AbstractComponent : INotifyPropertyChanged
    {

        public int ID { get; protected set; }

        private string _ErrorMessage;
        /// <summary>
        /// String intended to be shown to the user when a bad input is given
        /// <para>A null ErrorMessage value implies that there is no error</para>
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                return _ErrorMessage;
            }
            internal set
            {
                _ErrorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }

        /// <summary>
        /// On success handler
        /// </summary>
        public delegate void OnSuccessEventHandler();

        /// <summary>
        /// Called when operation is successful within this component
        /// </summary>
        public event OnSuccessEventHandler OnSuccessEvent;

        /// <summary>
        /// Call on success event handler
        /// </summary>
        internal virtual void OnSuccess()
        {
            ErrorMessage = null;
            OnSuccessEvent?.Invoke();
        }


        string _Title;
        /// <summary>
        /// Title of the group of functions
        /// </summary>
        public virtual string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                _Title = value;
                OnPropertyChanged("Title");
            }

        }

        /// <summary>
        /// Called when a property is changed in the the field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _Field_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(e.PropertyName);
        }


        /// <summary>
        /// On error handler
        /// </summary>
        public delegate void OnErrorEventHandler(Exception e);

        /// <summary>
        /// Called when error occurs within this component
        /// </summary>
        public event OnErrorEventHandler OnErrorEvent;

        /// <summary>
        /// Call on error event handler
        /// </summary>
        internal virtual void OnError(Exception e)
        {
            ErrorMessage = e.Message;
            OnErrorEvent?.Invoke(e);
        }


        /// <summary>
        /// On rest handler
        /// </summary>
        public delegate void OnRestEventHandler();

        /// <summary>
        /// Called when a rest occurs within this component
        /// </summary>
        public event OnRestEventHandler OnResetEvent;

        /// <summary>
        /// Call on rest event handler
        /// </summary>
        internal virtual void OnReset()
        {
            ErrorMessage = null;
            OnResetEvent?.Invoke();
        }

        /// <summary>
        /// Returns the type of object this object should be cast as
        /// for the purposes of building the UI
        /// </summary>
        /// <returns></returns>
        public virtual Type CastAs()
        {
            return this.GetType();
        }


        protected virtual void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
