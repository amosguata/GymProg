using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProgFramework.Models
{
    public class ProgramDrillDTO
    {
        public DrillDTO Drill { get; set; }
        public ICollection<SetDTO> Sets { get; set; }
        public int Id { get; set; }
    }
}
