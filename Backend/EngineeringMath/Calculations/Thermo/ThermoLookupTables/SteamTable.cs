using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Resources;

namespace EngineeringMath.Calculations.Thermo.ThermoLookupTables
{
    public class SteamTable : AbstractTable
    {
        public SteamTable() : base(Resources.LookupTables.SteamTable.Table)
        {
            this.Title = LibraryResources.SteamTable;
        }
    }
}
