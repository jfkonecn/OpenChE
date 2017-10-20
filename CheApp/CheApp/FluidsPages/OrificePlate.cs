using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace CheApp.FluidsPages
{
    /*
     * Help:
     https://developer.xamarin.com/api/type/Xamarin.Forms.Picker/
         
         */


    public class OrificePlate : ContentPage
    {


        public OrificePlate()
        {
            const int ROW_MARIGN = 20;
            const int COL_MARIGN = 20;
            const int ROW_HEIGHT = 50;

            TableView tableView = new TableView
            {
                Intent = TableIntent.Form,
                Root = new TableRoot
                {
                    new TableSection
                    {
                        new EntryCell
                        {
                            Label = "EntryCell:",
                            Placeholder = "Type Text Here"
                        }
                    }
                }
            };


            Content = new ScrollView
            {
                Content = new TableView
                {
                    Intent = TableIntent.Form,
                    Root = new TableRoot("Table Title") {
                        new TableSection ("Section 1 Title") {
                            new TableSection("Subsection")
                            {
                                                            new TextCell {
                                Text = "TextCell Text",
                                Detail = "TextCell Detail"
                            }
                            },
                            new TextCell {
                                Text = "TextCell Text",
                                Detail = "TextCell Detail"
                            },
                            new EntryCell {
                                Label = "EntryCell:",
                                Placeholder = "default keyboard",
                                Keyboard = Keyboard.Default
                            }
                        },
                        new TableSection ("Section 2 Title") {
                            new EntryCell {
                                Label = "Another EntryCell:",
                                Placeholder = "phone keyboard",
                                Keyboard = Keyboard.Numeric
                            },
                            new SwitchCell {
                                Text = "SwitchCell:"
                            }
                        }
                    }
                }
                
            };
        }
    }
}