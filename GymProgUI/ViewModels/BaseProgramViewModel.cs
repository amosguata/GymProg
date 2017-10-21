using GymProgFramework.Models;
using GymProgUI.Views;
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
    public abstract class BaseProgramViewModel : BaseModedPage
    { 

        public ICollection<ProgramDrillViewModel> Drills { get; set; }
        public String ProgramName { get; set; }

        public BaseProgramViewModel(PageModes mode) : base(mode)
        {
        }

        public ICommand RemoveDrill
        {
            get
            {
                return new Command(() =>
                {
                    Drills = Drills.Where(currDrill => currDrill.Id != SelectedItem.Id).ToList(); 
                });
            }
        }

        private ProgramDrillViewModel _selectedItem;

        public ProgramDrillViewModel SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                OnPropertyChanged("CanDrillBeViewed");
                OnPropertyChanged("CanDrillBeEdited");
            }
        }

        public bool CanDrillBeViewed   
        {
            get
            {
                return IsInViewOrEditMode && SelectedItem != null;
            }
        }

        public bool CanDrillBeEdited
        {
            get
            {
                return IsInEditMode && SelectedItem != null;
            }
        }

        public ICommand ViewSelectedDrill
        {
            get
            {
                return new Command(async () =>
                {
                    await Application.Current.MainPage.Navigation.PushModalAsync(
                          new DrillPage()
                          {
                              Title = "Dill details",
                              BindingContext = new DrillViewViewModel(SelectedItem.Drill)
                          });
                });
            }
        }

        public ICommand AddDrill
        {
            get
            {
                return new Command(async () =>
                {
                    await Application.Current.MainPage.Navigation.PushModalAsync(
                            new SelectionPage()
                            {
                                Title = "Select A drill",
                                BindingContext = new SelectDrillViewModel((DrillDTO SelectedDrill) =>
                                {

                                    ProgramDrillViewModel newDrillForProgram = new ProgramDrillViewModel()
                                    {
                                        Name = SelectedDrill.Name,
                                        Drill = SelectedDrill,
                                        Sets = new List<SetViewModel>()
                                    {
                                        new SetViewModel(new SetDTO() { Repetitions = 0, Weight = 0}) {IsEditable = true, IsVisable = true},
                                        new SetViewModel(new SetDTO() { Repetitions = 0, Weight = 0}) {IsEditable = true, IsVisable = true},
                                        new SetViewModel(new SetDTO() { Repetitions = 0, Weight = 0}) {IsEditable = true, IsVisable = true},
                                        new SetViewModel(new SetDTO() { Repetitions = 0, Weight = 0}) {IsEditable = true, IsVisable = true},
                                        new SetViewModel(new SetDTO() { Repetitions = 0, Weight = 0}) {IsEditable = true, IsVisable = true},
                                    }
                                            };

                                    ICollection<ProgramDrillViewModel> newList = new List<ProgramDrillViewModel>(Drills);
                                    newList.Add(newDrillForProgram);

                                    Drills = newList;
                                    OnPropertyChanged("Drills");
                                },false)
                            });
                });
            }
        }

    }
}
