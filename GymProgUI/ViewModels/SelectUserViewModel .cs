using GymProgFramework.Models;
using GymProgUI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProgUI.ViewModels
{
    public class SelectUserViewModel : BaseSelectionViewModel<UserDTO>
    {

        public override void InitData()
        {
            base.InitData();

            new UsersService().GetAllUsers().ContinueWith(async task =>
            {
                if (task.Result == null)
                {
                    await App.Current.MainPage.DisplayAlert("Operation Failed", "Unable to load Users", "OK");
                }

                Items = task.Result;
                OnPropertyChanged("Items");
            });
        }

        public SelectUserViewModel(Action<UserDTO> selectExecution) : base(selectExecution)
        {

        }

    }
}
