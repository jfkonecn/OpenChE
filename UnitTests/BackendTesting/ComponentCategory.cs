using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using EngineeringMath;
using EngineeringMath.Component;
using System.Linq;

namespace BackendTesting
{
    [TestClass]
    public class ComponentCategory
    {
        private readonly Random Rnd = new Random(0);
        [TestMethod]
        public void Search()
        {
            // just make sure the search function doesn't crash
            UnitCategoryCollection coll = MathManager.AllUnits;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            for (int i = 0; i < 1000; i++)
            {
                coll.SearchKeyword = new string(Enumerable.Repeat(chars, Rnd.Next(10))
                  .Select(s => s[Rnd.Next(s.Length)]).ToArray());
                coll.Search.Execute(null);
            }
        }
    }
}
