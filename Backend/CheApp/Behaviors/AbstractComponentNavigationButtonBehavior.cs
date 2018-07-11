using EngineeringMath.Calculations.Components;
using EngineeringMath.Calculations.Components.Functions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CheApp.Behaviors
{
    /// <summary>
    /// Navigates to an abstract component
    /// </summary>
    public class AbstractComponentNavigationButtonBehavior : Behavior<Button>
    {

        public static readonly BindableProperty ComponentProperty =
BindableProperty.Create("Component", typeof(AbstractComponent), typeof(AbstractComponentNavigationButtonBehavior), null);

        /// <summary>
        /// The page, as a component, which will be navigated to
        /// </summary>
        public AbstractComponent Component
        {
            get { return (AbstractComponent)GetValue(ComponentProperty); }
            set { SetValue(ComponentProperty, value); }
        }



        protected override void OnAttachedTo(Button bindable)
        {
            base.OnAttachedTo(bindable);

            if (bindable.BindingContext != null)
                BindingContext = bindable.BindingContext;

            bindable.BindingContextChanged += Bindable_BindingContextChanged;

            bindable.Clicked += Bindable_Clicked;
        }


        protected override void OnDetachingFrom(Button bindable)
        {
            base.OnDetachingFrom(bindable);

            bindable.BindingContextChanged -= Bindable_BindingContextChanged;

            bindable.Clicked -= Bindable_Clicked;
        }

        private async void Bindable_Clicked(object sender, EventArgs e)
        {
            MasterDetailPage mainPage = App.Current.MainPage as MasterDetailPage;
            if (mainPage == null)
            {
                throw new NotImplementedException();
            }
            await mainPage.Detail.Navigation.PushAsync(new Views.Templates.CalculationPage.BasicPage(Component));
            
        }


        void Bindable_BindingContextChanged(object sender, EventArgs e)
        {
            base.OnBindingContextChanged();

            if (!(sender is BindableObject bindable))
                return;

            BindingContext = bindable.BindingContext;
        }
    }
}
