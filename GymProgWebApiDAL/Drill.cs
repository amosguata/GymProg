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
    
    public partial class Drill
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Drill()
        {
            this.DrillMuscleGroups = new HashSet<DrillMuscleGroup>();
            this.ProgramDrills = new HashSet<ProgramDrill>();
        }
    
        public int DrillId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CreatorUserId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DrillMuscleGroup> DrillMuscleGroups { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProgramDrill> ProgramDrills { get; set; }
    }
}