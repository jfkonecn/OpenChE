using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EngineeringMath.Component
{
    public class FunctionRoot : FunctionTreeNode
    {
        private string _Category;

        public string Category
        {
            get { return _Category; }
            set
            {
                _Category = value;
                OnPropertyChanged();
            }
        }

        FunctionTreeNode _NextNode;
        
        public FunctionTreeNode NextNode
        {
            get { return _NextNode; }
            set
            {
                _NextNode = value;
                OnPropertyChanged();
            }
        }

    }
}
