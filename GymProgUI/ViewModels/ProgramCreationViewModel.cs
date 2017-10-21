using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymProgFramework.Models;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using GymProgUI.Views;
using GymProgUI.Services;
using GymProgWebApiBL.Models;

namespace GymProgUI.ViewModels
{
    public class ProgramCreationViewModel : BaseProgramViewModel
    {
        public ProgramCreationViewModel()  : base(UIEnums.PageModes.Create)
        {
            Drills = new List<ProgramDrillViewModel>();
           
        }

        public ICommand AddProgram
        {
            get
            {
                return new Command(async () =>
                {
                    if (Drills.Any(currDrill => currDrill.Sets.All(currSet => currSet.Weight == 0 && currSet.Repetitions == 0)))
                    {
                        await App.Current.MainPage.DisplayAlert("Invalid Program", "The program contains a drill with no repetitions and no weight", "OK");
                    }
                    else
                    {
                        ProgramDTO program = new ProgramDTO()
                        {
                            Name = ProgramName,
                            Drills = Drills.Select<ProgramDrillViewModel, ProgramDrillDTO>(currProgramDrill =>
                            {
                                return new ProgramDrillDTO()
                                {
                                    Drill = currProgramDrill.Drill,
                                    Sets = currProgramDrill.Sets.Where(CurrSet => CurrSet.Repetitions != 0 || CurrSet.Weight != 0).Select<SetViewModel, SetDTO>
                                    (currSet =>
                                    {
                                        return new SetDTO()
                                        {
                                            Repetitions = currSet.Repetitions,
                                            Weight = currSet.Weight
                                        };
                                    }
                                    ).ToList()
                                };
                            }).ToList()
                        };
                        ActionResponse response = await new ProgramsService().AddNewProgram(program);

                        if (!response.CompletedSuccessfully)
                        {
                            await App.Current.MainPage.DisplayAlert("Operation Failed", response.ErrorMessage, "OK");
                        }
                        else
                        {
                            await Application.Current.MainPage.Navigation.PopAsync();
                        }
                    }
                });
            }
        }
    }
}
