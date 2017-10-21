using GymProgFramework.Models;
using GymProgUI.Convertrs;
using GymProgUI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static GymProgUI.ViewModels.UIEnums;

namespace GymProgUI.ViewModels
{
    public class ProgramViewViewModel : BaseProgramViewModel
    {
        public ICollection<ProgramDrillViewModel> Drills { get; set; }
        public String ProgramName { get; set; }

        public ProgramViewViewModel(ProgramDTO program) : base(PageModes.View)
        {
            Drills = program.Drills
                .Select<ProgramDrillDTO,ProgramDrillViewModel>(currDrill => Converter.Convert<ProgramDrillViewModel>(currDrill))
                .ToList();

            ProgramName = program.Name;
        }
    }
}
