using GymProgFramework.Models;
using GymProgUI.Convertrs;
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
    public class ProgramEditViewModel : BaseProgramViewModel
    {
        public ProgramDTO Program { get; set; }

        public bool ShouldUpdateLocaly { get; set; }

        public ProgramEditViewModel(ProgramDTO program, bool shouldUpdateLocaly=false) : base(UIEnums.PageModes.Edit)
        {
            ShouldUpdateLocaly = shouldUpdateLocaly;
            Program = program;

            Drills = program.Drills.Select<ProgramDrillDTO, ProgramDrillViewModel>(currDrill =>
            {
                return Converter.Convert<ProgramDrillViewModel>(currDrill,true);
            }).ToList();
             
            ProgramName = program.Name; 
        }

        public ICommand UpdateProgram
        {
            get
            {
                return new Command(async () =>
                {
                    ProgramDTO programForUpdate = Converter.Convert<ProgramDTO>((BaseProgramViewModel)this);
                    programForUpdate.Id = Program.Id;
                    programForUpdate.LocalId = Program.LocalId;
                    ProgramsService programService = new ProgramsService();

                    ActionResponse response;

                    if (ShouldUpdateLocaly)
                    {
                        response = programService.SetLocalProgram(programForUpdate);
                    }
                    else
                    {
                        response = await programService.UpdateProgram(programForUpdate);
                    }

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

        public ICommand DeleteProgram
        {
            get
            {
                return new Command(async () =>
                {

                    String userResponse = await App.Current.MainPage.DisplayActionSheet("Are you sure you want to delete the current program", null, null, "I am sure", "cancel");

                    if (userResponse == "I am sure")
                    {
                        ActionResponse response;

                        if (ShouldUpdateLocaly)
                        {
                            response = new ProgramsService().DeleteLocalProgram(Program);
                        }
                        else
                        {
                            response = await new ProgramsService().DeleteProgram(Program);
                        }

                        if (!response.CompletedSuccessfully)
                        {
                            await App.Current.MainPage.DisplayAlert("Operation Failed", response.ErrorMessage, "OK");
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Program deleted succesully", "", "OK");
                            await Application.Current.MainPage.Navigation.PopModalAsync();
                        }
                    }
                });
            }
        }
    }
}
