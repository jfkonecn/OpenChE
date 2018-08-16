using System;
using System.Collections.Generic;
using System.Text;
using EngineeringMath.Resources;

namespace EngineeringMath.Component
{
    public class FunctionCategoryCollection : NotifyPropertyChangedExtension, ICategoryCollection
    {
        protected FunctionCategoryCollection()
        {
            Children = new NotifyPropertySortedList<FunctionCategory, FunctionCategoryCollection>(this);
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
        internal static FunctionCategoryCollection AllUnits
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


        internal static void BuildAllFunctions()
        {
            _AllFunctions = new FunctionCategoryCollection()
            {
                Children =
                {
                    new FunctionCategory(LibraryResources.FluidDynamics)
                    {
                        Children =
                        {
                            new Function(LibraryResources.OrificePlate)
                            {
                                NextNode = new FunctionBranch(LibraryResources.ChangeOutputs)
                                {
                                    Parameters =
                                    {
                                        new UnitlessParameter(LibraryResources.DischargeCoefficient, 0, 1),
                                        new SIUnitParameter(LibraryResources.Density, LibraryResources.Density, minSIValue:0),
                                        new SIUnitParameter(LibraryResources.InletPipeArea, LibraryResources.Area, minSIValue:0),
                                        new SIUnitParameter(LibraryResources.OrificeArea, LibraryResources.Area, minSIValue:0),
                                        new SIUnitParameter(LibraryResources.PressureDrop, LibraryResources.Pressure, minSIValue:0),
                                        new SIUnitParameter(LibraryResources.VolumetricFlowRate, LibraryResources.VolumetricFlowRate, minSIValue:0)
                                    },
                                    Children =
                                    {
                                        new FunctionLeaf($"",$"{LibraryResources.DischargeCoefficient}")
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
