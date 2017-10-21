using GymProgFramework.Models;
using GymProgUI.Services;
using GymProgWebApiBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GymProgUI.ViewModels
{
    public class EditUserViewModel : BaseViewModel
    {
        public String UserName { get { return User.Name; } }
        public String Password { get; set; }
        public UserDTO User { get; set; }

        public EditUserViewModel(UserDTO user) : base()
        {
            User = user;
        }

        public ICommand Delete
        {
            get
            {
                return new Command(async () =>
                {
                    String userResponse = await App.Current.MainPage.DisplayActionSheet("Are you sure you want to delete the current user", null, null, "I am sure", "cancel");
                    
                    if (userResponse == "I am sure")
                    {
                        ActionResponse response = await new UsersService().DeleteUser(User  );

                        if (!response.CompletedSuccessfully)
                        {
                            await App.Current.MainPage.DisplayAlert("Operation Failed", response.ErrorMessage, "OK");
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("User deleted succesully", "", "OK");
                        }
                    }

                });
            }
        }

        public ICommand Update
        {
            get
            {
                return new Command(async () =>
                {
                    UserDTO userForUpdate = new UserDTO() { UserId = User.UserId, Name = User.Name };
                    ActionResponse response = await new UsersService().UpdateUser(userForUpdate);

                    if (!response.CompletedSuccessfully)
                    {
                        await App.Current.MainPage.DisplayAlert("Operation Failed", response.ErrorMessage, "OK");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Program updated succesully", "", "OK");
                    }
                });
            }
        }

    }
}
