using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CheApp.CustomUI
{
    /// <summary>
    /// View Cell which displays a regular view when expanded
    /// When collapsed the the cell becomes just a label with specified placeholder text
    /// The collapsed state is toggled by tapping the cell
    /// </summary>
    public class CollapsibleViewCell : ViewCell
    {

        public CollapsibleViewCell(View expandedView) : base()
        {
            ExpandedView = expandedView;
            CollapsedView = CreateCollapsedView();
            ViewContainer = new Grid()
            {
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) }
                },
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) }
                }
            };
            this.View = ViewContainer;
            // TODO: make this collapsible!!!
            UpdateView();
        }

        protected override void OnTapped()
        {
            //base.OnTapped();
            IsCollapsed = !IsCollapsed;
        }

        private View CreateCollapsedView()
        {
            Label placeHolderLabel = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                VerticalOptions = LayoutOptions.Center,
                TextColor = Color.Black
            };
            placeHolderLabel.SetBinding(Label.TextProperty, "PlaceHolder");
            return new StackLayout()
            {
                BindingContext = this,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    placeHolderLabel
                }
            };
        }

        /// <summary>
        /// Updates the current cell view
        /// </summary>
        private void UpdateView()
        {
            while (ViewContainer.Children.Count > 0)
            {
                ViewContainer.Children.RemoveAt(0);
            }

            this.ViewContainer.Children.Add(IsCollapsed ? CollapsedView : ExpandedView, 0, 0);
        }

        /// <summary>
        /// View when collapsed
        /// </summary>
        private View CollapsedView { get; set; }

        /// <summary>
        /// The view when this view cell is expanded
        /// </summary>
        public View ExpandedView { get; private set; }

        /// <summary>
        /// Stores the collapsed or expanded view
        /// </summary>
        private Grid ViewContainer { get; set; }

        private string _PlaceHolder = string.Empty;
        /// <summary>
        /// Text displayed with the cell is collapsed
        /// </summary>
        public string PlaceHolder
        {
            get
            {
                return _PlaceHolder;
            }
            set
            {
                _PlaceHolder = value;
                OnPropertyChanged("PlaceHolder");
            }
        }

        private bool _IsCollapsed = true;
        /// <summary>
        /// true with the cell is collapsed
        /// </summary>
        public bool IsCollapsed
        {
            get
            {
                return _IsCollapsed;
            }
            set
            {
                _IsCollapsed = value;
                OnPropertyChanged("IsCollapsed");
                UpdateView();
            }
        }
    }
}
