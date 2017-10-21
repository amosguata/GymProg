using GymProgFramework.Models;
using GymProgUI.Services;
using GymProgWebApiBL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static GymProgUI.ViewModels.UIEnums;

namespace GymProgUI.ViewModels
{
    public class DrillCreationViewModel : BaseDrillVIewModel
    {
        public void ItemSelectionDisable(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        public DrillCreationViewModel() : base(PageModes.Create)
        {
            Drill = new DrillDTO();
        }

        public ICommand Add
        {
            get
            {
                return new Command(async () =>
                {
                    Drill.MuscleGroups = PossibleMuscleGroups.Where(currMuscleGroup => currMuscleGroup.ShouldInclude).ToList();

                    DrillsService service = new DrillsService();

                    ActionResponse response = await service.AddnewDrill(Drill);

                    if (!response.CompletedSuccessfully)
                    {
                        await App.Current.MainPage.DisplayAlert("Operation Failed", response.ErrorMessage, "OK");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Drill Created Successfully","" , "OK");
                        Drill = new DrillDTO();
                        OnPropertyChanged("NewDrill");
                    }
                });
            }
        }
    }
}
