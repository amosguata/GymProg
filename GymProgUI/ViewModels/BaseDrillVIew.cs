using GymProgFramework.Models;
using GymProgUI.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GymProgUI.ViewModels.UIEnums;

namespace GymProgUI.ViewModels
{
    public class BaseDrillVIewModel : BaseModedPage
    {
        public ICollection<MuscleGroupDTO> PossibleMuscleGroups { get; set; }
        public DrillDTO Drill { get; set; }
        public PageModes Mode { get; set; }

        public BaseDrillVIewModel(PageModes pageMode) : base(pageMode)
        {
            MuscleGroupsService muscleGroupsService = new MuscleGroupsService();
            muscleGroupsService.GetAllMuscleGroups().ContinueWith(async task =>
            {
                if (task.Result == null)
                {
                    await App.Current.MainPage.DisplayAlert("Operation Failed", "Muscle groups where unable to load", "OK");
                }
                else
                {
                    PossibleMuscleGroups = task.Result;

                    foreach (MuscleGroupDTO muscleGroup in Drill.MuscleGroups)
                    {
                        this.PossibleMuscleGroups.All(currPossibleMusclGroup =>
                        {
                            if (currPossibleMusclGroup.Id == muscleGroup.Id)
                            {
                                currPossibleMusclGroup.ShouldInclude = true;
                            }

                            return true;
                        });
                    }

                    OnPropertyChanged("PossibleMuscleGroups");
                }
            });
        }

    }
}
