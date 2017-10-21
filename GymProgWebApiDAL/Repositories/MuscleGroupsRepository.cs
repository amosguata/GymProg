using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProgWebApiDAL.Repositories
{
    public class MuscleGroupsRepository : BaseRepository<MuscleGroup>
    {

        public MuscleGroupsRepository() : base()
        {
        }

        public override DbSet<MuscleGroup> getEntitySet()
        {
            return ((GymProgDBEntities)context).MuscleGroups;
        }

    }
}
