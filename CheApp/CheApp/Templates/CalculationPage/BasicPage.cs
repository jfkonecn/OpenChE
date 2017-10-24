using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CheApp.Templates.CalculationPage
{
    internal class BasicPage
    {
        internal static void BasicInputPage(ContentPage contentPage, NumericFieldData[] fieldData)
        {

            const int ROW_MARIGN = 20;
            const int COL_MARIGN = 20;
            const int ROW_HEIGHT = 50;

            EntryCell cell = new EntryCell();

            Picker picker = new Picker
            {
                Title = "Units:" 
                
            };

            TextCell text = new TextCell
            {
                
            };




            foreach (string str in fieldData[0].ListOfUnitNames)
            {
                picker.Items.Add(str);
            }


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


            contentPage.Content = new ScrollView
            {
                Content = new StackLayout
                {
                    Children =
                    {
                        new TableView
                        {
                            Intent = TableIntent.Form,
                            Root = new TableRoot("Table Title") {
                                new TableSection ("Section 1 Title") {
                                    new TextCell {
                                        Text = "TextCell Text",
                                        Detail = "TextCell Detail"
                                    },
                                    new EntryCell {
                                        Label = "EntryCell:",
                                        Placeholder = "default keyboard",
                                        Keyboard = Keyboard.Default
                                    },
                                    new ViewCell
                                    {
                                        View = new StackLayout
                                        {
                                            Children =
                                            {
                                                picker
                                            }
                                        }
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
                        },
                        new TableView
                        {
                            Intent = TableIntent.Form,
                            Root = new TableRoot("Table Title") {
                                new TableSection ("Section 1 Title") {
                                    new TextCell {
                                        Text = "TextCell Text",
                                        Detail = "TextCell Detail"
                                    },
                                    new EntryCell {
                                        Label = "EntryCell:",
                                        Placeholder = "default keyboard",
                                        Keyboard = Keyboard.Default
                                    },
                                    new ViewCell
                                    {
                                        View = new StackLayout
                                        {
                                            Children =
                                            {
                                                picker
                                            }
                                        }
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
                    }
                }

            };
        }
    }
}
