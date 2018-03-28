using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Calculations.Components.Parameter;

namespace EngineeringMath.Calculations.Components.Functions
{
    /// <summary>
    /// A collection of abstract components which are intended to be used to perform a calculation
    /// </summary>
    public abstract class SimpleFunction : AbstractComponent, IEnumerable
    {

        public virtual IEnumerator GetEnumerator()
        {
            foreach (AbstractComponent obj in ParameterCollection())
            {
                yield return obj;
            }
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
        public abstract SimpleParameter GetParameter(int ID);

        /// <summary>
        /// Solves function based on what the current output parameter is
        /// </summary>
        public void Solve()
        {
            Calculation();
            OnSolve?.Invoke();
        }

        /// <summary>
        /// Called right after this object is built is built
        /// </summary>
        internal virtual void FinishUp()
        {
            // left empty just in case we need to add something
        }



        /// <summary>
        /// Yields all parameters in this object
        /// </summary>
        /// <returns></returns>
        internal abstract IEnumerable<SimpleParameter> ParameterCollection();

        internal override void OnReset()
        {
            foreach (AbstractComponent obj in this)
            {
                obj.OnReset();
            }
        }

        public override Type CastAs()
        {
            return typeof(SimpleFunction);
        }
    }
}
