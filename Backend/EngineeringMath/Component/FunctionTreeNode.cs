using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EngineeringMath.Component
{
    public abstract class FunctionTreeNode : NotifyPropertyChangedExtension, 
        IChildItem<IParameterContainerNode>, IParameterContainerNode
    {
        protected FunctionTreeNode()
        {
            Parameters = new NotifyPropertySortedList<IParameter, IParameterContainerNode>(this);
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
        public IParameterContainerNode ParentObject { get; internal set; }



        IParameterContainerNode IChildItem<IParameterContainerNode>.Parent
        {
            get
            {
                return this.ParentObject;
            }
            set
            {
                this.ParentObject = value;
            }
        }

        private NotifyPropertySortedList<IParameter, IParameterContainerNode> _Parameters;
        public NotifyPropertySortedList<IParameter, IParameterContainerNode> Parameters
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
            if (this.Parameters.TryGetValue(paraName, out IParameter para))
            {
                return para.BaseUnitValue;
            }
            return ParentObject.GetBaseUnitValue(paraName);
        }

        public void SetBaseUnitValue(string paraName, double num)
        {
            if (this.Parameters.TryGetValue(paraName, out IParameter para))
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
