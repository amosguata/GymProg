using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProgWebApiDAL.Repositories
{
    public class ProgramRepository : BaseRepository<Program>
    {
        public ProgramRepository() : base()
        {
        }

        public override DbSet<Program> getEntitySet()
        {
            return ((GymProgDBEntities)context).Programs;
        }

    }
}
