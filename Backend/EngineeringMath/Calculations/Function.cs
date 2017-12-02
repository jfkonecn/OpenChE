using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace EngineeringMath.Calculations
{
    public abstract class Function : INotifyPropertyChanged
    {

        /// <summary>
        /// Store all paramter
        /// <para>int represents the id of the parameters</para>
        /// </summary>
        public Dictionary<int, Parameter> FieldDic;

        /// <summary>
        /// Stores the output parameter the user selected (intended to be binded with a picker)
        /// </summary>
        public PickerSelection<Parameter> OutputSelection;

        /// <summary>
        /// Solves function based on what the current output parameter is
        /// </summary>
        public void Solve()
        {
            int outputID = getOutputID();
            FieldDic[outputID].SetValue(Calculation(outputID));
        }

        /// <summary>
        /// Call this function after creating the function object to finish building the object
        /// </summary>
        internal void FinishSetup()
        {
            foreach (Parameter obj in FieldDic.Values)
            {
                obj.OnMadeOuput += new Parameter.MadeOuputHandler(UpdateAllParametersInputOutput);
            }

            OutputSelection = new PickerSelection<Parameter>(
                FieldDic.Values.ToDictionary(x => x.Title, x => x)
                );
            OutputSelection.OnSelectedIndexChanged += OutputSelection_OnSelectedIndexChanged;
        }

        private void OutputSelection_OnSelectedIndexChanged()
        {
            OutputSelection.SelectedObject.isOutput = true;
        }

        /// <summary>
        /// Makes every parameter an input except for the parameter with the current outputID 
        /// </summary>
        /// <param name="outputID">ID of parameter which is the new output</param>
        private void UpdateAllParametersInputOutput(int outputID)
        {
            foreach (Parameter obj in FieldDic.Values)
            {
                if (obj.ID != outputID)
                {
                    obj.isInput = true;
                }
            }
        }

        string _Title;
        /// <summary>
        /// Title of the function
        /// </summary>
        public string Title {
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
        ///  of the parameter which is the output
        /// </summary>
        /// <returns>ID of the output parameter</returns>
        private int getOutputID()
        {
            foreach (Parameter obj in FieldDic.Values)
            {
                if (obj.isOutput)
                {
                    return obj.ID;
                }
            }

            throw new Exception("No Output Found in function!");
        }


        /// <summary>
        /// Performs the calculation this function object represents using the current state of the parameter objects
        /// </summary>
        /// <param name="outputID">ID of the parameter which is to be solved for</param>
        /// <returns></returns>
        protected abstract double Calculation(int outputID);



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
