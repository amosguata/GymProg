using GymProgFramework.Models;
using GymProgUI.Services;
using GymProgWebApiBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GymProgUI.ViewModels
{
    public class NewAccountPageViewModel : BaseViewModel
    {
        public NewAccountPageViewModel()
        {
            NewUser = new UserDTO();
        }

        public UserDTO NewUser { get; set; }

        private Command _createNewUser { get; set; }
        public Command CreateNewUser
        {
            get
            {
                return _createNewUser ?? (_createNewUser = new Command(async () =>
                {
                    ActionResponse response = await new UsersService().CreateNewUser(NewUser);
                    if (!response.CompletedSuccessfully)
                    {
                        await App.Current.MainPage.DisplayAlert("Operation Failed", response.ErrorMessage, "OK");
                    }
                    else
                    {
                        await Application.Current.MainPage.Navigation.PopAsync();
                    }
                }));
            }

        }

    }
}
