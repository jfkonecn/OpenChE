using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EngineeringMath.Component
{
    public class UnitComposite : Unit
    {
        protected UnitComposite() : base()
        {
            FinishUp();
        }

        public UnitComposite(params UnitCompositeElement[] lookupUnits)
        {
            LookupUnits = lookupUnits;
            FinishUp();
        }





        private void FinishUp()
        {
            List<UnitCompositeElement> elements = BuildUnitElementList();
            FullName = string.Empty;
            Symbol = string.Empty;
            UnitSystem = elements[0].Unit.UnitSystem;
            IsBaseUnit = true;
            // composite units must always be on an absolute scale
            AbsoluteScaleUnit = true;
            UnitCompositeElement preElement = null;
            double localConvertToBaseFactor = 1;
            foreach(UnitCompositeElement element in elements)
            {
                Unit curUnit = element.Unit;
                if (!UnitSystem.TryToFindCommonUnitSystem(UnitSystem, curUnit.UnitSystem, out UnitSystem temp))
                    throw new UnitSystem.NoCommonUnitSystemFoundException();
                UnitSystem = temp;
                if (preElement == null)
                {
                    if(element.Power < 0)
                    {
                        FullName = $"1 / {element.Unit.FullName}";
                    }
                    else
                    {
                        FullName = $"{element.Unit.FullName}";
                    }
                    Symbol = element.UnitSymbol;                    
                }
                else if (element.Power < 0 && preElement.Power >= 0)
                {
                    FullName = $"{FullName} {LibraryResources.Per} {element.FullName}";
                    Symbol = $"{Symbol}*{element.UnitSymbol}";
                }
                else
                {
                    FullName = $"{FullName}*{element.FullName}";
                    Symbol = $"{Symbol}*{element.UnitSymbol}";
                }
                localConvertToBaseFactor *= Math.Pow(curUnit.ConvertToBase(1), element.Power);
                IsBaseUnit = curUnit.IsBaseUnit == true && IsBaseUnit == true;
                preElement = element;
            }
            ConvertToBaseEquation = $"${CurUnitVar} * {localConvertToBaseFactor}";
            ConvertFromBaseEquation = $"${BaseUnitVar} / {localConvertToBaseFactor}";            
        }

        /// <summary>
        /// returns a list where the UnitCompositeElements only represent Units and NOT UnitComposites
        /// </summary>
        /// <returns></returns>
        private List<UnitCompositeElement> BuildUnitElementList()
        {
            List<UnitCompositeElement> elements = new List<UnitCompositeElement>();
            foreach (UnitCompositeElement element in LookupUnits)
            {
                if (element.Unit is UnitComposite unitComp)
                {
                    foreach(UnitCompositeElement subEle in unitComp.BuildUnitElementList())
                    {
                        elements.Add(new UnitCompositeElement( subEle.Unit, element.Power * subEle.Power));
                    }                    
                }
                else
                {
                    elements.Add(element);
                }
            }
            elements.Sort((x, y) =>
            {
                int temp = -x.Power.CompareTo(y.Power);
                if (temp == 0)
                    x.Unit.Parent.Name.CompareTo(y.Unit.Parent.Name);
                return temp;
            });
            return elements;
        }


        protected UnitCompositeElement[] LookupUnits { get; set; }




        /// <summary>
        /// Category of units based on one or more units (ex: enthalpy or Area)
        /// </summary>
        public class UnitCompositeElement
        {

            protected UnitCompositeElement()
            {
                string unitName = UnitName, unitCat = UnitCat;
                if (!UnitIsUserDefined)
                {
                    unitName = (string)typeof(LibraryResources).GetProperty(unitName).GetValue(null, null);
                }
                if (!CatIsUserDefined)
                {
                    unitCat = (string)typeof(LibraryResources).GetProperty(unitCat).GetValue(null, null);
                }
                Unit = MathManager.AllUnits.GetItemByFullName(unitName, unitCat);
            }


            internal UnitCompositeElement(Unit unit, int power)
            {
                UnitName = unit.IsUserDefined ? unit.FullName : unit.LibraryResourceFullName;
                UnitCat = unit.Parent.IsUserDefined ? unit.Parent.Name : unit.Parent.LibraryResourceName;
                Power = power;
                UnitIsUserDefined = unit.IsUserDefined;
                CatIsUserDefined = unit.Parent.IsUserDefined;
                Unit = unit;
            }


            /// <summary>
            /// Unit Name of the type of unit to be used
            /// </summary>
            public string UnitName { get; }

            /// <summary>
            /// Category Name of the type of unit to be used
            /// </summary>
            public string UnitCat { get; }
            /// <summary>
            /// The power the unit should be raise to
            /// </summary>
            public int Power { get; }

            public bool UnitIsUserDefined { get; }

            public bool CatIsUserDefined { get; }

            public Unit Unit { get; }


            private string _FullName = string.Empty;
            /// <summary>
            /// Returns baseUnitName to power indicated in this property
            /// NOTE: this will ignore negative power values
            /// </summary>
            /// <param name="baseUnitName"></param>
            /// <returns></returns>
            public string FullName
            {
                get
                {
                    if (!_FullName.Equals(string.Empty))
                        return _FullName;


                    int num = Math.Abs(Power);
                    switch (num)
                    {
                        case 1:
                            _FullName = Unit.FullName;
                            break;
                        case 2:
                            _FullName = string.Format(LibraryResources.UnitSquared, Unit.FullName);
                            break;
                        case 3:
                            _FullName = string.Format(LibraryResources.UnitCubed, Unit.FullName);
                            break;
                        default:
                            _FullName = string.Format(LibraryResources.UnitToTheNthPower, Unit.FullName, num);
                            break;
                    }
                    return _FullName;
                }

            }
            private string _UnitSymbol = string.Empty;

            public string UnitSymbol
            {
                get
                {
                    if (!_UnitSymbol.Equals(string.Empty))
                        return _UnitSymbol;
                    if (Power == 1)
                    {
                        _UnitSymbol = Unit.Symbol;
                        return _UnitSymbol;
                    }
                        
                    string powStr = Power.ToString();
                    // https://en.wikipedia.org/wiki/Unicode_subscripts_and_superscripts
                    powStr = powStr.Replace('-', '\u207B');
                    powStr = powStr.Replace('0', '\u2070');
                    powStr = powStr.Replace('1', '\u00B9');
                    powStr = powStr.Replace('2', '\u00B2');
                    powStr = powStr.Replace('3', '\u00B3');
                    powStr = powStr.Replace('4', '\u2074');
                    powStr = powStr.Replace('5', '\u2075');
                    powStr = powStr.Replace('6', '\u2076');
                    powStr = powStr.Replace('7', '\u2077');
                    powStr = powStr.Replace('8', '\u2078');
                    powStr = powStr.Replace('9', '\u2079');
                    _UnitSymbol = $"{Unit.Symbol}{powStr}";
                    return _UnitSymbol;
                }
            }
        }
    }
}
