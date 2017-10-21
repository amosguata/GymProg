using GymProgUI.Services;
using GymProgUI.ViewModels;
using GymProgUI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GymProgUI
{
    public partial class App : Application
    {
        public App()
        {

            InitializeComponent();
            try
            {
                if (Device.RuntimePlatform == "Android")
                {
                    Current.MainPage = new NavigationPage();
                    if (new ProgramsService().GetLocalPrograms().Any())
                    {
                        Current.MainPage.Navigation.PushAsync(new MyTrainingProgramsPage() { Title = "My Training Programs", BindingContext = new TraineeMyTrainingProgramsViewModel() });
                    }
                    else
                    {
                        Current.MainPage.Navigation.PushAsync(new LoginPage() { Title = "Login", BindingContext = new LoginViewModel() });
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}