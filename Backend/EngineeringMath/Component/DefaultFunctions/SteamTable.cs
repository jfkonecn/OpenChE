using EngineeringMath.Component.Builder;
using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component.DefaultFunctions
{
    public class SteamTable : Function
    {
        public SteamTable() : base(LibraryResources.SteamTable, LibraryResources.Thermodynamics, false)
        {
            VisitableNodeDirector dir = new VisitableNodeDirector()
            {
                NodeBuilder = PVTTableNodeBuilder.SteamTableBuilder()
            };
            dir.BuildNode(LibraryResources.SteamTable);
            NextNode = dir.Node;
        }
    }
}
