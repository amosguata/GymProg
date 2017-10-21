using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GymProgUI.ViewModels.UIEnums;

namespace GymProgUI.ViewModels
{
    public abstract class BaseModedPage: BaseViewModel
    {
        public BaseModedPage(PageModes mode) : base(true)
        {
            Mode = mode;
        }

        private PageModes Mode { get; set; }

        public bool IsInCreateMode
        {
            get { return Mode == PageModes.Create; }

        }

        public bool IsInEditMode
        {
            get { return Mode == PageModes.Edit; }

        }

        public bool IsInViewMode
        {
            get { return Mode == PageModes.View; }

        }

        public bool IsInCreateOrEditMode
        {
            get
            {
                return IsInCreateMode || IsInEditMode;
            }
        }

        public bool IsInViewOrEditMode
        {
            get
            {
                return IsInViewMode || IsInEditMode;
            }
        }
    }
}
