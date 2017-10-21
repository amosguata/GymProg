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
    public class SelectDrillViewModel : BaseSelectionViewModel<DrillDTO>
    {
        public bool TakeCurrentUsersDrills { get; set; }
        public override void InitData()
        {
            base.InitData();

            Task<ICollection<DrillDTO>> drillTask;

            if (TakeCurrentUsersDrills)
            {
                drillTask = new DrillsService().GetUserDrills();
            }
            else
            {
                drillTask = new DrillsService().GetAllDrills();
            }

            drillTask.ContinueWith(async task =>
            {
                if (task.Result == null)
                {
                    await App.Current.MainPage.DisplayAlert("Operation Failed", "Unable to load drills", "OK");
                }

                Items = task.Result;
                OnPropertyChanged("Items");
            });
        }

        public SelectDrillViewModel  (Action<DrillDTO> exectueAfterAdd, bool takeCurrentUsersDrills = false) : base(exectueAfterAdd)
        {
            TakeCurrentUsersDrills = takeCurrentUsersDrills;
        }
    }
}
