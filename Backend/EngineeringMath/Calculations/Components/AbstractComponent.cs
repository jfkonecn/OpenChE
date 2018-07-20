using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EngineeringMath.Calculations.Components
{
    public abstract class AbstractComponent : INotifyPropertyChanged
    {

        public int ID { get; protected set; }

        private string _ErrorMessage = string.Empty;
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
                OnPropertyChanged();
            }
        }

        private bool IsEventHandlerRegistered(Delegate eventHandler, Delegate prospectiveHandler)
        {
            if (eventHandler != null)
            {
                foreach (Delegate existingHandler in eventHandler.GetInvocationList())
                {
                    if (existingHandler == prospectiveHandler)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        /// <summary>
        /// On success handler
        /// </summary>
        public delegate void OnSuccessEventHandler(AbstractComponent sender, OnSuccessEventArgs e);

        /// <summary>
        /// Called when operation is successful within this component
        /// </summary>
        public event OnSuccessEventHandler OnSuccessEvent;

        /// <summary>
        /// Call on success event handler
        /// </summary>
        internal virtual void OnSuccess(AbstractComponent sender, OnSuccessEventArgs e = null)
        {
            ErrorMessage = string.Empty;
            OnSuccessEvent?.Invoke(sender, e);
            CurrentComponentState = ComponentState.Success;
        }

        internal void OnSuccess(OnSuccessEventArgs e = null)
        {
            OnSuccessEvent?.Invoke(this, e);
        }

        public bool IsSuccessEventHandlerRegistered(OnSuccessEventHandler prospectiveHandler)
        {
            return IsEventHandlerRegistered(OnSuccessEvent, prospectiveHandler);
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
                OnPropertyChanged();
            }

        }

        /// <summary>
        /// On error handler
        /// </summary>
        public delegate void OnErrorEventHandler(AbstractComponent sender, Exception e);

        /// <summary>
        /// Called when error occurs within this component
        /// </summary>
        public event OnErrorEventHandler OnErrorEvent;

        /// <summary>
        /// Call on error event handler
        /// </summary>
        internal virtual void OnError(AbstractComponent sender, Exception e)
        {
            ErrorMessage = $"{this.Title}: {e.Message}";
            OnErrorEvent?.Invoke(sender, e);
            CurrentComponentState = ComponentState.Error;
        }

        internal virtual void OnError(Exception e)
        {
            OnError(this, e);
        }

        public bool IsErrorEventHandlerRegistered(OnErrorEventHandler prospectiveHandler)
        {
            return IsEventHandlerRegistered(OnErrorEvent, prospectiveHandler);
        }

        /// <summary>
        /// On rest handler
        /// </summary>
        public delegate void OnRestEventHandler(AbstractComponent sender, OnResetEventArgs e);

        /// <summary>
        /// Called when a rest occurs within this component
        /// </summary>
        public event OnRestEventHandler OnResetEvent;

        /// <summary>
        /// Call on rest event handler
        /// </summary>
        internal virtual void OnReset(AbstractComponent sender, OnResetEventArgs e)
        {
            ErrorMessage = string.Empty;
            OnResetEvent?.Invoke(sender, e);
            CurrentComponentState = ComponentState.Reset;
        }

        internal void OnReset(OnResetEventArgs e = null)
        {
            OnReset(this, e);
        }

        public bool IsResetEventHandlerRegistered(OnRestEventHandler prospectiveHandler)
        {
            return IsEventHandlerRegistered(OnResetEvent, prospectiveHandler);
        }

        /// <summary>
        /// States the component can be in
        /// </summary>
        public enum ComponentState
        {
            Success,
            Reset,
            Error
        }
        private ComponentState _CurrentComponentState = ComponentState.Reset;
        public ComponentState CurrentComponentState
        {
            get
            {
                return _CurrentComponentState;
            }
            private set
            {
                _CurrentComponentState = value;
                OnPropertyChanged();
            }
        }


        protected virtual void OnPropertyChanged([CallerMemberName]string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
