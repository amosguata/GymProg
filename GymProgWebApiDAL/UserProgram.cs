//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GymProgWebApiDAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserProgram
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProgramId { get; set; }
    
        public virtual Program Program { get; set; }
        public virtual User User { get; set; }
    }
}