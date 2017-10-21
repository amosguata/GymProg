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
    public class DrillEditViewModel : BaseDrillVIewModel
    {

        public DrillEditViewModel(DrillDTO drill) : base(UIEnums.PageModes.Edit)
        {
            Drill = drill;
        }


        public ICommand Update
        {
            get
            {
                return  new Command(async () =>
                {

                    Drill.MuscleGroups = PossibleMuscleGroups.Where(currMuscleGroup => currMuscleGroup.ShouldInclude).ToList();

                    ActionResponse response = await new DrillsService().UpdateDrill(Drill);

                    if (!response.CompletedSuccessfully)
                    {
                        await App.Current.MainPage.DisplayAlert("Operation Failed", response.ErrorMessage, "OK");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Drill updated succesully", "", "OK");
                    }
                });
            }
        }

        public ICommand Delete
        {
            get
            {
                return new Command(async () =>
                {
                    String userResponse = await App.Current.MainPage.DisplayActionSheet("Are you sure you want to delete the current drill", null, null, "I am sure", "cancel");

                    if (userResponse == "I am sure")
                    {
                        ActionResponse response = await new DrillsService().DeleteDrill(Drill);

                        if (!response.CompletedSuccessfully)
                        {
                            await App.Current.MainPage.DisplayAlert("Operation Failed", response.ErrorMessage, "OK");
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Program deleted succesully", "", "OK");
                        }
                    }
                });
            }
        }
    }
}
