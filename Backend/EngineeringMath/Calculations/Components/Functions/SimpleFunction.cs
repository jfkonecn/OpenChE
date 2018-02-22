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

        /// <summary>
        /// Creates a collection of abstract components which 
        /// </summary>
        /// <param name="allParameters"></param>
        internal SimpleFunction(SimpleParameter[] allParameters)
        {
            AllParameters = allParameters.ToDictionary(x => x.ID);
        }

        private readonly Dictionary<int, SimpleParameter> AllParameters;

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
        public SimpleParameter GetParameter(int ID)
        {
            return AllParameters[ID];
        }

        /// <summary>
        /// Solves function based on what the current output parameter is
        /// </summary>
        public void Solve()
        {
            Calculation();
            OnSolve?.Invoke();
        }




        /// <summary>
        /// Yields all parameters in this object
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<SimpleParameter> ParameterCollection()
        {
            foreach (SimpleParameter para in AllParameters.Values)
            {
                yield return para;
            }
        }

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
