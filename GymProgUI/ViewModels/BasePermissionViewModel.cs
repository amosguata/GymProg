using GymProgFramework.Enums;
using GymProgFramework.Models;
using GymProgUI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProgUI.ViewModels
{
    public class BasePermissionViewModel : BaseViewModel
    {
        public BasePermissionViewModel(bool doesRequireLogin): base(doesRequireLogin)
        {

        }

        public bool IsTrainee
        {
            get
            {
                UserDTO usr = new UsersService().GetUser(null).Result;
                return usr.Permission == (int)Enums.PermissionsType.Trainee;
            }
        }
        public bool IsTrainer
        {
            get
            {
                UserDTO usr = new UsersService().GetUser(null).Result;
                return usr.Permission == (int)Enums.PermissionsType.Trainer;
            }
        }
        public bool IsAdmin
        {
            get
            {
                UserDTO usr = new UsersService().GetUser(null).Result;
                return usr.Permission == (int)Enums.PermissionsType.Admin;
            }
        }

        public bool isTrainerOrAdmin
        {
            get
            {
                return IsAdmin || IsTrainer;
            }
        }
    }
}
