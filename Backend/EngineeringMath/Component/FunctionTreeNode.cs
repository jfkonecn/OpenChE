using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EngineeringMath.Component
{
    public abstract class FunctionTreeNode : ChildItem<IParameterContainerNode>, IParameterContainerNode
    {
        protected FunctionTreeNode()
        {
            Parameters = new NotifyPropertySortedList<Parameter, IParameterContainerNode>(this);
        }

        protected FunctionTreeNode(string name) : this()
        {
            Name = name;
        }

        private string _Name;

        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged();
            }
        }

        public string Key
        {
            get
            {
                return Name;
            }
        }

        [XmlIgnore]
        private IParameterContainerNode ParentObject { get; set; }



        public override IParameterContainerNode Parent
        {
            get
            {
                return this.ParentObject;
            }
            internal set
            {
                this.ParentObject = value;
            }
        }

        private NotifyPropertySortedList<Parameter, IParameterContainerNode> _Parameters;
        public NotifyPropertySortedList<Parameter, IParameterContainerNode> Parameters
        {
            get { return _Parameters; }
            set
            {
                _Parameters = value;
                OnPropertyChanged();
            }
        }

        public double GetBaseUnitValue(string paraName)
        {
            if (this.Parameters.TryGetValue(paraName, out Parameter para))
            {
                return para.BaseUnitValue;
            }
            return ParentObject.GetBaseUnitValue(paraName);
        }

        public void SetBaseUnitValue(string paraName, double num)
        {
            if (this.Parameters.TryGetValue(paraName, out Parameter para))
            {
                para.BaseUnitValue = num;
                return;
            }
            ParentObject.SetBaseUnitValue(paraName, num);
        }

        public abstract void Calculate();

        /// <summary>
        /// 
        /// </summary>
        public abstract void Invalidate();
    }
}
