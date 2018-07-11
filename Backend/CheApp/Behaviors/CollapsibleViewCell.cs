using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CheApp.Behaviors
{
    public class CollapsibleViewCell : Behavior<ViewCell>
    {
        protected override void OnAttachedTo(ViewCell cell)
        {
            cell.Tapped += Cell_Tapped;
        }

        protected override void OnDetachingFrom(ViewCell cell)
        {
            cell.Tapped -= Cell_Tapped;
        }

        private void Cell_Tapped(object sender, EventArgs e)
        {
            ViewCell cell = sender as ViewCell;
            if(cell != null)
            {

            }
            throw new ArgumentException();
        }
    }
}
