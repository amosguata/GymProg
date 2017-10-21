using System;
using System.Collections.Generic;
using System.Linq;

namespace GymProgFramework.Models
{
    public class UserDTO : FiterableDTO
    {
        public int Permission { get; set; }
        public int UserId { get; set; }
        public String Password { get; set; }
    }
}