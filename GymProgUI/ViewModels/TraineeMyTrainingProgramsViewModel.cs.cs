using GymProgFramework.Models;
using GymProgUI.Services;
using GymProgUI.Views;
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
    public class TraineeMyTrainingProgramsViewModel : BaseViewModel
    {
        public bool CanAddDrill { get { return false; } }
        public bool CanEditDrill { get { return false; } }
        public ICollection<ProgramDTO> Programs { get; set; }

        public bool IsTrainee { get { return true; } }
        public bool IsTrainer { get { return false; } }
        public bool IsAdmin { get { return false; } }
        public bool isTrainerOrAdmin { get { return false; } }

        protected void addPropertyToBeChangedAfterSelection(String propertyName)
        {
            _propertyChangedAfterSelection.Add(propertyName);
        }

        public List<String> _propertyChangedAfterSelection = new List<string>();
        public ProgramDTO _selectedItem;

        public ProgramDTO SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                foreach (String propertyName in _propertyChangedAfterSelection)
                {
                    OnPropertyChanged(propertyName);
                }
            }
        }

        public override void InitData()
        {
            base.InitData();
            Programs = new ProgramsService().GetLocalPrograms();
            if (Programs == null)
            {
                App.Current.MainPage.DisplayAlert("Operation Failed", "Users Programs were unable to load", "OK");
            }
            OnPropertyChanged("Programs");
            addPropertyToBeChangedAfterSelection("CanViewProgram");
        }

        public TraineeMyTrainingProgramsViewModel() : base(false)
        {
        }

        public bool CanViewProgram
        {
            get
            {
                return IsTrainee && SelectedItem != null;
            }
        }

        public ICommand ViewProgram
        {
            get
            {
                return new Command(async () =>
                {

                    await Application.Current.MainPage.Navigation.PushModalAsync(
                        new ProgramPage()
                        {
                            Title = "Select A Program",
                            BindingContext = new ProgramViewViewModel(this.SelectedItem)
                        });
                });
            }
        }

        public ICommand EditProgram
        {
            get
            {
                return new Command(async () =>
                {

                    await Application.Current.MainPage.Navigation.PushModalAsync(
                        new ProgramPage()
                        {
                            Title = "Select A Program",
                            BindingContext = new ProgramEditViewModel(this.SelectedItem,true)
                        });
                });
            }
        }

        public ICommand AddProgram
        {
            get
            {
                return new Command(async () =>
                {

                    PushPage(
                        new SelectionPage()
                        {
                            Title = "Select A Program",
                            BindingContext = new SelectProgramViewModel(false, (ProgramDTO program) =>
                            {
                                Programs.Add(program); 
                                new ProgramsService().SetLocalPrograms(Programs);
                                Application.Current.MainPage.Navigation.PopAsync();
                            })
                        });
                });
            }
        }

    }
}
