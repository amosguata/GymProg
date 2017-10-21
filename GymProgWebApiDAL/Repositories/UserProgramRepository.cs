using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProgWebApiDAL.Repositories
{
    public class UserProgramRepository : BaseRepository<UserProgram>
    {
        public UserProgramRepository() : base()
        {
        }

        public override DbSet<UserProgram> getEntitySet()
        {
            return ((GymProgDBEntities)context).UserPrograms;
        }
    }
}
