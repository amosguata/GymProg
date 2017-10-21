using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProgFramework.Models
{
    public class MuscleGroupDTO
    {
        public String Name { get; set; }
        public int Id { get; set; } 
        public bool ShouldInclude { get; set; }
    }
}
