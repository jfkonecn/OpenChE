using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Resources.LookupTables
{
    /// <summary>
    /// Source: https://www.nist.gov/sites/default/files/documents/srd/NISTIR5078-Tab3.pdf
    /// </summary>
    public class SteamTable : ThermoTable
    {
        public static readonly SteamTable Table = new SteamTable();
        private SteamTable() : base("EngineeringMath.Resources.LookupTables.SteamTable.csv")
        {

        }
    }
}
