using GymProgFramework.Models;
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
    public class SetViewModel : BaseViewModel
    {
        private const double WEIGHT_MIN = 0.5;
        private const double WEIGHT_MAX = 300;
        private const double REP_MIN = 0;
        private const double REP_MAX = 50;

        public static double WeightInterval { get { return 0.5; } }
        public static double RepInterval { get { return 1; } }
        public int Id { get; set; }
        public SetViewModel(SetDTO Set)
        {
            Weight = Set.Weight;
            Repetitions = Set.Repetitions;
            Id = Set.Id;

            IsVisable = Set.Weight != 0 || Set.Repetitions != 0;
        }
        public SetViewModel()
        {
            Weight = 0;
            Repetitions = 0;
            IsVisable = false;
            IsEditable = false;
        }

        public double Repetitions { get; set; }
        public double Weight { get; set; }
        public bool IsEditable { get; set; }
        public bool IsVisable { get; set; }

        public ICommand WeightIncreasement
        {
            get
            {
                return new Command(() =>
                {
                    if (Weight < WEIGHT_MAX)
                    {
                        Weight += WeightInterval;
                        OnPropertyChanged("Weight");
                    }
                });
            }
        }

        public ICommand RepetitionsIncreasement
        {
            get
            {
                return new Command(() =>
                {
                    if (Repetitions < REP_MAX)
                    {
                        Repetitions += RepInterval;
                        OnPropertyChanged("Repetitions");
                    }

                });
            }
        }

        public ICommand WeightDecreasement
        {
            get
            {
                return new Command(() =>
                {
                    if (Weight > WEIGHT_MIN)
                    {
                        Weight -= WeightInterval;
                        OnPropertyChanged("Weight");
                    }
                });
            }
        }

        public ICommand RepetitionsDecreasement
        {
            get
            {
                return new Command(() =>
                {
                    if (Repetitions > REP_MIN)
                    {
                        Repetitions -= RepInterval;
                        OnPropertyChanged("Repetitions");
                    }
                });
            }
        }

    }
}

