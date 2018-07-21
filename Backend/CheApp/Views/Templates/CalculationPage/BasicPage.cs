using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using EngineeringMath.Units;
using System.Diagnostics;
using EngineeringMath.Calculations;
using EngineeringMath.Calculations.Components.Functions;
using EngineeringMath.Resources;
using EngineeringMath.Calculations.Components;
using EngineeringMath.Calculations.Components.Parameter;
using EngineeringMath.Calculations.Components.Selectors;
using CheApp.Views.Templates;

namespace CheApp.Views.Templates.CalculationPage
{
    public class BasicPage : ContentPage
    {

        /// <summary>
        /// Sets up a basic page which handles a single function
        /// <para>Solve for data defaults to having last element in the solve for picker being selected</para>
        /// </summary>
        /// <param name="componetType">The type of componet which the page will represent</param>
        public BasicPage(Type componetType) : this(ComponentFactory.BuildComponent(componetType))
        {
            
        }

        public BasicPage(AbstractComponent component)
        {
            this.Title = component.Title;
            View content = Template.SelectTemplate(component, null).CreateContent() as View;
            content.Margin = new Thickness(15);
            content.BindingContext = component;
            this.Content = new ScrollView()
            {
                Content = content
            };

        }

        private readonly AbstractComponentTemplateSelector Template = new AbstractComponentTemplateSelector();

        /// <summary>
        /// Creates a view out of an abstract component
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        private View CreateView(AbstractComponent component)
        {
            if (component as SimpleFunction != null)
            {
                return CreateView((SimpleFunction)component);
            }
            else if (component as SimpleParameter != null)
            {
                /*component.OnErrorEvent += delegate (Exception e)
                {
                    this.DisplayAlert(LibraryResources.ErrorMessageTitle,
                        $"{ ((SimpleParameter)component).Title }: { e.Message }",
                        LibraryResources.Okay);
                };*/
                //return new ParameterView((SimpleParameter)component);
                return null;
            }
            else if (component as SimplePicker<SimpleParameter> != null
                || component as SimplePicker<int> != null
                || component as FunctionPicker != null)
            {
                return PickerSelectionFrame(component);
            }
            else if (component as FunctionSubber != null)
            {
                return CreateFunctionSubberFrame((FunctionSubber)component);
            }
            else
            {
                Debug.WriteLine("BasicPage: That type of abstract component is not handled in CreateView!");
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Creates a list view to represent a typeof simple function
        /// </summary>
        /// <param name="fun"></param>
        /// <returns></returns>
        private ScrollView CreateView(SimpleFunction fun)
        {
            StackLayout layout = new StackLayout()
            {
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            /*
            // abstract component rows
            foreach (AbstractComponent component in fun)
            {
                layout.Children.Add(CreateView(component));
            }
            */
            // Calculate button row
            layout.Children.Add(CreateCalculateFrame(fun));


            

            return new ScrollView()
            {
                Content = new ListView()
                {
                    SeparatorVisibility = SeparatorVisibility.Default,
                    ItemsSource = fun.ComponentCollection,
                    ItemTemplate = new DataTemplate(() =>
                    {
                        return new ViewCell()
                        {
                            //View = CreateView(component)
                        };
                    })
                }
            };
        }

        /// <summary>
        /// Create a grid to represent the user choosing the parameter to be solved for
        /// <para>outputSelection cannot be null!</para>
        /// <para>selector must be a type of PickerSelection</para>
        /// </summary>
        /// <returns></returns>
        private Frame PickerSelectionFrame(object selector)
        {
            /*Grid solveForGrid = BasicGrids.SimpleGrid(2, 1, 0, 0);

            Label label = new Label()
            {
                Style = (Style)Application.Current.Resources["minorHeaderStyle"]
            };
            label.SetBinding(Label.TextProperty, new Binding("Title"));
            label.BindingContext = selector;

            Picker solveForPicker = new Picker();
            solveForPicker.SetBinding(Picker.ItemsSourceProperty, new Binding("PickerList"));
            solveForPicker.SetBinding(Picker.SelectedIndexProperty, new Binding("SelectedIndex"));
            solveForPicker.SetBinding(Picker.IsEnabledProperty, new Binding("IsEnabled"));
            solveForPicker.BindingContext = selector;

            solveForGrid.Children.Add(label, 1, 1);
            solveForGrid.Children.Add(solveForPicker, 1, 2);
            
            return new Frame
            {
                Content = solveForGrid,
                Style = (Style)Application.Current.Resources["neutralParameterStyle"]
            };*/
            return null;
        }

        /// <summary>
        /// Creates the calcualte button for this page
        /// </summary>
        /// <param name="myFun"></param>
        /// <returns></returns>
        private Frame CreateCalculateFrame(SimpleFunction fun)
        {
            return new ButtonFrame(LibraryResources.Calculate, delegate (System.Object o, System.EventArgs e) 
            {
                
                try
                {
                    //fun.Solve();
                }
                catch (Exception err)
                {
                    this.DisplayAlert(LibraryResources.ErrorMessageTitle,
                        string.Format(LibraryResources.UnexpectedException, err.GetType(), err.Message),
                        LibraryResources.Okay);
                }
            });
        }

        /// <summary>
        /// Creates the done button for this page
        /// </summary>
        /// <returns></returns>
        private Frame CreateDoneFrame()
        {
            return new ButtonFrame(LibraryResources.Done, async delegate (System.Object o, System.EventArgs e)
            { await this.Navigation.PopAsync(); });
        }

        /// <summary>
        /// Create subber function frame
        /// </summary>
        /// <returns></returns>
        private Frame CreateFunctionSubberFrame(FunctionSubber funSubber)
        {
            Grid subberGrid = new Grid
            {
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition { Height = new GridLength(0, GridUnitType.Absolute) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(0, GridUnitType.Absolute) }
                },
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = new GridLength(0, GridUnitType.Absolute) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(0, GridUnitType.Absolute) }
                }
            };

            subberGrid.Children.Add(CreateView(funSubber.AllFunctions), 1, 1);

            View funView = CreateView(funSubber.AllFunctions.SubFunction);
            subberGrid.Children.Add(funView, 1, 2);
            /*
            funSubber.AllFunctions.OnSelectedIndexChanged += delegate ()
            {
                subberGrid.Children.Remove(funView);
                funView = CreateView(funSubber.AllFunctions.SubFunction);
                subberGrid.Children.Add(funView, 1, 2);
            };
            */

            return new Frame
            {
                Content = subberGrid,
                Style = (Style)Application.Current.Resources["neutralParameterStyle"]
            };
        }

    }
}
