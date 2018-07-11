using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Calculations.Components.Commands;
using EngineeringMath.Calculations.Components.Group;
using EngineeringMath.Calculations.Components.Parameter;
using EngineeringMath.Resources;

namespace EngineeringMath.Calculations.Components.Functions
{
    /// <summary>
    /// A collection of abstract components which are intended to be used to perform a calculation
    /// </summary>
    public abstract class SimpleFunction : AbstractGroupOfComponents
    {

        internal SimpleFunction() : base()
        {

        }

        /// <summary>
        /// Performs the calculation this function object represents using the current state of the parameter objects
        /// <para>Updates parameters accordingly</para>
        /// </summary>
        /// <returns></returns>
        protected abstract void Calculation();


        /// <summary>
        /// 
        /// </summary>
        internal delegate void SolveHandler();

        /// <summary>
        /// Called after the solve function is called
        /// </summary>
        internal event SolveHandler OnSolve;


        /// <summary>
        /// Find Parameter by its ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public SimpleParameter GetParameter(int ID)
        {
            return ComponentCollection.Single((x) => x.ID == ID && x as SimpleParameter != null) as SimpleParameter;
        }

        public ObservableCollection<SimpleParameter> AllParameters
        {
            get
            {
                ObservableCollection<SimpleParameter> temp = new ObservableCollection<SimpleParameter>();
                foreach (AbstractComponent comp in ComponentCollection)
                {
                    SimpleParameter para = comp as SimpleParameter;
                    if(para != null)
                    {
                        temp.Add(para);
                    }
                }
                return temp;
            }
        }


        public ButtonComponent SolveButton { get; private set; }


        /// <summary>
        /// Solves function based on what the current output parameter is
        /// </summary>
        public void Solve(object parameter)
        {
            Calculation();
            OnSolve?.Invoke();
        }

        public bool CanSolve(object parameter)
        {
            return true;
        }



        internal override void OnReset(AbstractComponent sender, OnResetEventArgs e)
        {
            foreach (AbstractComponent obj in ComponentCollection)
            {
                obj.OnReset();
            }
        }

        /// <summary>
        /// Returns the default collection of abstract object not including the OutputSelection picker
        /// </summary>
        /// <returns></returns>
        protected abstract ObservableCollection<AbstractComponent> CreateRemainingDefaultComponentCollection();

        protected override ObservableCollection<AbstractComponent> CreateDefaultComponentCollection()
        {
            SolveButton = new ButtonComponent(Solve, CanSolve)
            {
                Title = LibraryResources.Calculate
            };
            ObservableCollection<AbstractComponent> temp = CreateRemainingDefaultComponentCollection();
            temp.Add(SolveButton);
            return temp;
        }
    }
}
