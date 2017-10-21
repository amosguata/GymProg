using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProgFramework.Models
{
    public class DrillDTO : FiterableDTO
    {
        public String Description { get; set; }
        public int Id { get; set; }
        public IList<MuscleGroupDTO> MuscleGroups { get; set; }
    }
}
