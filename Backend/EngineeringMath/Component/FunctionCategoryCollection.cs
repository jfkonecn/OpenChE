using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using EngineeringMath.Resources;

namespace EngineeringMath.Component
{
    public class FunctionCategoryCollection : NotifyPropertyChangedExtension, IList<FunctionCategory>
    {
        protected FunctionCategoryCollection(string name)
        {
            Name = name;
            Children = new NotifyPropertySortedList<FunctionCategory, FunctionCategoryCollection>(this);
        }

        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            protected set
            {
                _Name = value;
                OnPropertyChanged();
            }
        }


        private NotifyPropertySortedList<FunctionCategory, FunctionCategoryCollection> _Children;
        public NotifyPropertySortedList<FunctionCategory, FunctionCategoryCollection> Children
        {
            get { return _Children; }
            set
            {
                _Children = value;
                OnPropertyChanged();
            }
        }

        private static FunctionCategoryCollection _AllFunctions = null;
        internal static FunctionCategoryCollection AllFunctions
        {
            get
            {
                if (_AllFunctions == null)
                {
                    BuildAllFunctions();
                }
                return _AllFunctions;
            }
        }

        public int Count => Children.Count;

        public bool IsReadOnly => Children.IsReadOnly;

        public FunctionCategory this[int index] { get => Children[index]; set => Children[index] = value; }

        internal static void BuildAllFunctions()
        {
            _AllFunctions = new FunctionCategoryCollection(LibraryResources.AllFunctions)
            {
                new FunctionCategory(LibraryResources.FluidDynamics)
                {
                    new Function(LibraryResources.OrificePlate)
                    {
                        NextNode = new FunctionBranch(LibraryResources.ChangeOutputs)
                        {
                            Parameters =
                            {
                                new UnitlessParameter(LibraryResources.DischargeCoefficient, "dc", 0, 1),
                                new SIUnitParameter(LibraryResources.Density, "rho", LibraryResources.Density, minSIValue:0),
                                new SIUnitParameter(LibraryResources.InletPipeArea, "pArea", LibraryResources.Area, minSIValue:0),
                                new SIUnitParameter(LibraryResources.OrificeArea, "oArea", LibraryResources.Area, minSIValue:0),
                                new SIUnitParameter(LibraryResources.PressureDrop, "deltaP", LibraryResources.Pressure, minSIValue:0),
                                new SIUnitParameter(LibraryResources.VolumetricFlowRate, "Q", LibraryResources.VolumetricFlowRate, minSIValue:0)
                            },
                            Children =
                            {
                                new FunctionLeaf("$Q / ($pArea * Sqrt((2 * $deltaP) / ($rho * ($pArea ^ 2 / $oArea ^ 2 - 1))))", "dc"),
                                new FunctionLeaf("(2 * $deltaP) / ((($Q  / ($dc * $pArea)) ^ 2) * ($pArea ^ 2 / $oArea ^ 2 - 1))", "rho"),
                                new FunctionLeaf("Sqrt(1 / ((1 / $oArea ^ 2) - ((2 * $deltaP * $dc ^ 2) / ($Q ^ 2 * $rho))))", "pArea"),
                                new FunctionLeaf("Sqrt(1 / ((1 / $pArea ^ 2) + ((2 * $deltaP * $dc ^ 2) / ($Q ^ 2 * $rho))))", "oArea"),
                                new FunctionLeaf("(($Q / ($dc * $pArea)) ^ 2 * ($rho * ($pArea ^ 2 / $oArea ^ 2 - 1))) / 2", "deltaP"),
                                new FunctionLeaf("$dc * $pArea * Sqrt((2 * $deltaP) / ($rho * ($pArea ^ 2 / $oArea ^ 2 - 1)))", "Q")
                            }
                        }
                    }
                }
            };
        }

        public bool TryGetValue(string key, out FunctionCategory value)
        {
            return Children.TryGetValue(key, out value);
        }

        public int IndexOf(FunctionCategory item)
        {
            return Children.IndexOf(item);
        }

        public void Insert(int index, FunctionCategory item)
        {
            Children.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Children.RemoveAt(index);
        }

        public void Add(FunctionCategory item)
        {
            Children.Add(item);
        }

        public void Clear()
        {
            Children.Clear();
        }

        public bool Contains(FunctionCategory item)
        {
            return Children.Contains(item);
        }

        public void CopyTo(FunctionCategory[] array, int arrayIndex)
        {
            Children.CopyTo(array, arrayIndex);
        }

        public bool Remove(FunctionCategory item)
        {
            return Children.Remove(item);
        }

        public IEnumerator<FunctionCategory> GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Children.GetEnumerator();
        }
    }
}
