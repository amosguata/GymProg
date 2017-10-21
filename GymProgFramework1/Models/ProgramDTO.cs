using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProgFramework.Models
{
    public class ProgramDTO : FiterableDTO
    {
        public ICollection<ProgramDrillDTO> Drills { get; set; }
        public int Id { get; set; }
        public int LocalId { get; set; }
    }
}
