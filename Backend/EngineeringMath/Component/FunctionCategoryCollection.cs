using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using EngineeringMath.Resources;
using System.Linq;
using ReplaceableParaBld = EngineeringMath.Component.Builder.ReplaceableParameterBuilder;
using EngineeringMath.Component.Builder;
using System.Threading;
using System.Diagnostics;
using EngineeringMath.Component.DefaultFunctions;

namespace EngineeringMath.Component
{
    public class FunctionCategoryCollection : CategoryCollection<Function>
    {
        protected FunctionCategoryCollection(string name) : base(name)
        {

        }






        public new FunctionCategory GetCategoryByName(string catName)
        {
            return (FunctionCategory)base.GetCategoryByName(catName);

        }




        private static FunctionCategoryCollection _AllFunctions = null;
        internal static FunctionCategoryCollection AllFunctions
        {
            get
            {
                lock (BuildingAllFunctions)
                {
                    if (_AllFunctions == null)
                    {
                        BuildAllFunctions();
                    }
                    return _AllFunctions;
                }                
            }
        }

        private static readonly object LockAllFunctions = new object();

        private static readonly object BuildingAllFunctions = new object();


        internal static void BuildAllFunctions()
        {

            _AllFunctions = new FunctionCategoryCollection(LibraryResources.AllFunctions);
            List<Thread> threads = new List<Thread>();

            IEnumerable<Type> types = from type in typeof(OrificePlate).Assembly.GetTypes()
                                      where type.Namespace != null
                                      where type.Namespace.Equals(typeof(OrificePlate).Namespace)
                                      where type.BaseType != null
                                      where type.BaseType.Equals(typeof(Function))
                                      select type;
            foreach (Type type in types)
            {
                Thread thread = new Thread(() =>
                {
                    Function fun = (Function)Activator.CreateInstance(type);
                    lock (LockAllFunctions)
                    {
                        if(!_AllFunctions.TryGetValue(fun.CategoryName, out Category<Function> funCat))
                        {
                            funCat = new Category<Function>(fun.CategoryName, false);
                            _AllFunctions.Add(funCat);
                        }
                        funCat.Add(fun);
                    }
                })
                {
                    Name = type.Name
                };
                thread.Start();
                threads.Add(thread);
            }
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            _AllFunctions.Search.Execute(null);
        }
    }
}
