using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymProgFramework;
using GymProgFramework.Models;
using Xamarin.Forms;
using GymProgUI.Services;
using GymProgUI.Views;
using System.Windows.Input;
using GymProgFramework.Enums;

namespace GymProgUI.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Page RedirectToPage { get; set; }
        public LoginViewModel(Page redirectToPage=null) : base(false)
        {
            RedirectToPage = redirectToPage;
            User = new UserDTO();
        }

        public UserDTO User { get; set; }

        public ICommand Login
        {
            get
            {
                return new Command(async () =>
               {
                   UserDTO foundUser = await new UsersService().GetUser(this.User, false);
                   if (foundUser == null)
                   {
                       await App.Current.MainPage.DisplayAlert("Operation Failed", "Wrong user name or password", "OK");
                   }
                   else
                   {
                       if (RedirectToPage != null)
                       {
                           await Application.Current.MainPage.Navigation.PopModalAsync();
                           PushPage(RedirectToPage);
                       }
                       else if (foundUser.Permission == (int)Enums.PermissionsType.Trainer)
                       {
                           PushPage(
                            new MyTrainingProgramsPage() { Title = "Main Menu", BindingContext = new TrainerMyTrainingProgramsViewModel() });

                       }
                       else if (foundUser.Permission == (int)Enums.PermissionsType.Trainee)
                       {
                           PushPage(
                            new MyTrainingProgramsPage() { Title = "My Programs", BindingContext = new TraineeMyTrainingProgramsViewModel() });
                       }
                       else if (foundUser.Permission == (int)Enums.PermissionsType.Admin)
                       {
                           PushPage(
                            new MyTrainingProgramsPage() { Title = "Main Menu", BindingContext = new AdminMyTrainingPageViewModel() });
                       }
                   }
               });
            }
        }

        public ICommand CreateNewAccount
        {
            get
            {
                return new Command(async () =>
                {

                    PushPage(
                        new NewAccountPage() { Title = "Create new Account", BindingContext = new NewAccountPageViewModel() });
                });
            }
        }
    }
}
