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
    public abstract class BaseMyTrainingProgramViewModel : BasePermissionViewModel
    {
        public BaseMyTrainingProgramViewModel(bool doesRequireLogin=true) : base(doesRequireLogin)
        {

        }


        public bool CanViewProgram
        {
            get
            {
                return false;
            }
        }

        public ICommand AddProgram
        {
            get
            {
                return new Command(async () =>
                {
                    PushPage(
                             new ProgramPage() { Title = "New Program", BindingContext = new ProgramCreationViewModel() });
                });
            }
        }

        public ICommand AddDrill
        {
            get
            {
                return new Command(async () =>
                {
                    PushPage(
                             new DrillPage() { Title = "New Drill", BindingContext = new DrillCreationViewModel() });
                });
            }
        }

        public ICommand SelectProgram
        {
            get
            {
                return new Command(async () =>
                {
                    PushPage(
                         new SelectionPage()
                         {
                             Title = "Select a program",
                             BindingContext = new SelectProgramViewModel(true, (ProgramDTO program) =>
                             {
                                 PushPage(
                                    new ProgramPage() { Title = "Edit Program", BindingContext = new ProgramEditViewModel(program) });
                             })
                         });
                });
            }
        }

        public ICommand SelectDrill
        {
            get
            {
                return new Command(async () =>
                {
                    PushPage(
                        new SelectionPage()
                        {
                            Title = "Select A drill",
                            BindingContext = new SelectDrillViewModel((DrillDTO SelectedDrill) =>
                            {
                                PushPage(
                                        new DrillPage() { Title = "Edit Drill", BindingContext = new DrillEditViewModel(SelectedDrill) });
                            },true)
                        });
                });
            }
        }
    }
}
