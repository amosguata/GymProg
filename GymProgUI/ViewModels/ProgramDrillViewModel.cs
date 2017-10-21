using GymProgFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProgUI.ViewModels
{
    public class ProgramDrillViewModel
    {
        public ProgramDrillViewModel()
        {

        }

        public IList<SetViewModel> Sets { get; set; }
        public String Name { get; set; }
        public DrillDTO Drill { get; set; }
        public int Id { get; set; }
        
        private SetViewModel getSetModelAtPosition(int index)
        {
            SetViewModel wantedSet = Sets.ElementAtOrDefault(index);
            if (wantedSet == null)
            {
                return new SetViewModel();
            }

            return wantedSet;
        }

        public SetViewModel Set1
        {
            get
            {
                return getSetModelAtPosition(0);
            }
        }

        public SetViewModel Set2
        {
            get
            {
                return getSetModelAtPosition(1);
            }
        }

        public SetViewModel Set3
        {
            get
            {
                return getSetModelAtPosition(2);
            }
        }

        public SetViewModel Set4
        {
            get
            {
                return getSetModelAtPosition(3);
            }
        }

        public SetViewModel Set5
        {
            get
            {
                return getSetModelAtPosition(4);
            }
        }


    }
}
