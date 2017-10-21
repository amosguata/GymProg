using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProgWebApiDAL.Repositories
{
    public class UsersRepository :  BaseRepository<User>
    {
        public UsersRepository() : base()
        {
        }

        public override DbSet<User> getEntitySet()
        {
            return ((GymProgDBEntities)context).Users;
        }
    }
}
