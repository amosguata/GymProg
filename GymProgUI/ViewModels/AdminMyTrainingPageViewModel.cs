using GymProgFramework.Models;
using GymProgUI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GymProgUI.ViewModels
{
    public class AdminMyTrainingPageViewModel : BaseMyTrainingProgramViewModel
    {
        public AdminMyTrainingPageViewModel() : base(true)
        {

        }

        public ICommand SelectUser
        {
            get
            {
                return new Command(async () =>
                {
                    PushPage(
                        new SelectionPage()
                        {
                            Title = "Select A User",
                            BindingContext = new SelectUserViewModel((UserDTO selectedUser) =>
                            {
                                PushPage(
                                        new UserPage() { Title = "Edit User", BindingContext = new EditUserViewModel(selectedUser) });
                            })
                        });
                });
            }
        }


    }
}
