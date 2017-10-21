using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProgWebApiDAL.Repositories
{
   public class DrillsRepository : BaseRepository<Drill>
    {
        public DrillsRepository() : base()
        {
        }

        public override DbSet<Drill> getEntitySet()
        {
            return ((GymProgDBEntities)context).Drills;
        }
    }
}
