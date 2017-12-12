using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using EngineeringMath.Calculations;

namespace CheApp.Templates.ObjectStyleBinders
{
    /// <summary>
    /// Object intended to be binded with a button for the purpose of page navigation
    /// </summary>
    public class PageLinkStyle : BindableObject
    {

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="currentPage">The page where this button will be located</param>
        /// <param name="pageMaker"></param>
        public PageLinkStyle(Page currentPage, PageFactory pageMaker)
        {
            this.PageMaker = pageMaker;
            CurrentPage = currentPage;
        }

        public static readonly BindableProperty IsEnabledProperty =
BindableProperty.Create("IsEnabled", typeof(bool),
      typeof(PageLinkStyle),
      default(bool));
        /// <summary>
        /// To be binded with picker
        /// </summary>
        public bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set
            {
                SetValue(IsEnabledProperty, value);
                OnPropertyChanged("IsEnabled");
            }
        }


        private PageFactory _PageMaker;
        /// <summary>
        /// Builds a page which the binded button will link to
        /// </summary>
        /// <returns></returns>
        public delegate Page PageFactory();

        public PageFactory PageMaker
        {
            get
            {
                return _PageMaker;
            }
            private set
            {
                _PageMaker = value;
            }
        }


        public Page CurrentPage
        {
            get; private set;
        }

        /// <summary>
        /// Function to be added to a button's OnClick event
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        public async void OnClickFunction(System.Object o, System.EventArgs e)
        {
            await CurrentPage.Navigation.PushAsync(PageMaker());
        }


        private Style _ButtonStyle = (Style)Application.Current.Resources["buttonStyle"];
        /// <summary>
        /// This is the style for the button
        /// </summary>
        public Style ButtonStyle
        {
            get
            {
                return _ButtonStyle;
            }
            private set
            {
                _ButtonStyle = value;
                OnPropertyChanged("ButtonStyle");
            }
        }

        private string _Text;
        /// <summary>
        /// This is the text for the button
        /// </summary>
        public string Text
        {
            get
            {
                return _Text;
            }
            set
            {
                _Text = value;
                OnPropertyChanged("Text");
            }
        }




        
    }
}
