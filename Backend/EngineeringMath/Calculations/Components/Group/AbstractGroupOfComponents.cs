using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Calculations.Components.Functions;

namespace EngineeringMath.Calculations.Components.Group
{
    /// <summary>
    /// Used when a AbstractComponent which HAS A collection of abstract objects which vary in size
    /// ex: a function may have parameters, pickers and graphs to help perform the calculation
    /// </summary>
    public abstract class AbstractGroupOfComponents : AbstractComponent
    {
        internal AbstractGroupOfComponents()
        {
            
        }

        /// <summary>
        /// Builds the Parameter collection
        /// MUST BE CALLED BY CHILDREN!!!
        /// </summary>
        protected void BuildComponentCollection()
        {
            // TODO: Add loading user settings
            ComponentCollection = CreateDefaultComponentCollection();
        }


        /// <summary>
        /// Builds the default settings of ParameterCollection
        /// </summary>
        protected abstract ObservableCollection<AbstractComponent> CreateDefaultComponentCollection();

        private ObservableCollection<AbstractComponent> _ComponentCollection;
        /// <summary>
        /// Yields all componets in this object
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<AbstractComponent> ComponentCollection
        {
            get
            {
                return _ComponentCollection;
            }
            protected set
            {
                _ComponentCollection = value;
                OnPropertyChanged();
            }

        }
    }
}
