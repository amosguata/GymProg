using GymProgFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GymProgUI.ViewModels.UIEnums;

namespace GymProgUI.ViewModels
{
    public class DrillViewViewModel : BaseDrillVIewModel
    {
        public DrillViewViewModel(DrillDTO drill) : base(PageModes.View)
        {
            this.Drill = drill;
        }
    }
}
