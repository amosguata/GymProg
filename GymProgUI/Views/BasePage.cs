using GymProgUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GymProgUI.Views
{
    public abstract class BasePage : ContentPage
    {
        public BasePage()
        {
        }

        protected override void OnAppearing()
        {
            BaseViewModel pageBase = this.BindingContext as BaseViewModel;
            pageBase.InitData();
        }
    }
}
