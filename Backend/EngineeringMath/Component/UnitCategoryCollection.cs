using EngineeringMath.Component.DefaultUnits;
using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EngineeringMath.Component
{
    public class UnitCategoryCollection : CategoryCollection<Unit>
    {
        protected UnitCategoryCollection(string name) : base(name)
        {
        }
        
        

        // https://en.wikipedia.org/wiki/Unicode_subscripts_and_superscripts

        private static UnitCategoryCollection _AllUnits = null;
        internal static UnitCategoryCollection AllUnits
        {
            get
            {
                if(_AllUnits == null)
                {
                    BuildAllUnits();
                }
                lock (LockAllUnits)
                {
                    return _AllUnits;
                }                
            }
        }


        private static readonly object LockAllUnits = new object();

        public new UnitCategory GetCategoryByName(string catName)
        {
            return (UnitCategory)base.GetCategoryByName(catName);

        }

        internal static void BuildAllUnits()
        {
            _AllUnits = new UnitCategoryCollection(LibraryResources.AllUnits);
            List<Thread> list = new List<Thread>();

            IEnumerable<Type> types = from type in typeof(Length).Assembly.GetTypes()
                                      where type.Namespace != null
                                      where type.Namespace.Equals(typeof(Length).Namespace)
                                      where type.BaseType != null
                                      where type.BaseType.Equals(typeof(UnitCategory))
                                      select type;
            foreach(Type type in types)
            {
                Thread thread = new Thread(() =>
                {
                    UnitCategory unitCat = (UnitCategory)Activator.CreateInstance(type);
                    lock (LockAllUnits)
                    {
                        _AllUnits.Add(unitCat);
                    }                    
                })
                {
                    Name = type.Name
                };
                thread.Start();
                list.Add(thread);
            }
            foreach (Thread thread in list)
            {
                thread.Join();
            }
        }


        public class UnitCategoryNotFoundException : ArgumentException
        {
            public UnitCategoryNotFoundException(string unitCatName) : base(unitCatName) { }
        }
        public class UnitCategoriesWithSameNameException : ArgumentException
        {
            public UnitCategoriesWithSameNameException(string unitCatName) : base(unitCatName) { }
        }
    }
}
