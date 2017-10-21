using GymProgFramework.Models;
using GymProgUI.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GymProgUI.ViewModels
{
    public class SelectProgramViewModel : BaseSelectionViewModel<ProgramDTO>
    {
        
        public SetViewModel Set { get; set; }
        public bool TakeCurrentUserPrograms { get; set; }

        public override void InitData()
        {
            base.InitData();

            Task<ICollection<ProgramDTO>> programsTask;

            if (TakeCurrentUserPrograms)
            {
                programsTask = new ProgramsService().GetUserPrograms();
            }
            else
            {
                programsTask = new ProgramsService().GetAllPrograms();
            }

            programsTask.ContinueWith(async task =>
            {
                if (task.Result == null)
                {
                    await App.Current.MainPage.DisplayAlert("Operation Failed", "Unable to load drills", "OK");
                }

                Items = task.Result;
                OnPropertyChanged("Items");
            });
        }

        public SelectProgramViewModel(bool takeCurrentUserPrograms,  Action<ProgramDTO>  exectueAfterAdd) : base(exectueAfterAdd) 
        {
            TakeCurrentUserPrograms = takeCurrentUserPrograms;
        }
    }
}
