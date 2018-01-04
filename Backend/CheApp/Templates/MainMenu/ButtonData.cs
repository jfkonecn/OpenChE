using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CheApp.Templates.MainMenu
{
    /// <summary>
    /// Object contains all information required to make a page from a template
    /// </summary>
    internal class ButtonData
    {
        /// <summary>
        /// Groups required data to make the a button link to a different page
        /// </summary>
        /// <param name="navFunctionType">The function which the button should link to on a click event (must be a type of function!)</param>
        /// <param name="text">Text which should be on the button</param>
        internal ButtonData(string text, Type navFunctionType)
        {
            this.Text = text;
            this.NavFunctionType = navFunctionType;
        }

        /// <summary>
        /// The page the button should link to on a click event
        /// </summary>
        internal Type NavFunctionType { get; private set; }


        /// <summary>
        /// Text which should be on the button
        /// </summary>
        internal string Text { get; private set; }
    }
}
