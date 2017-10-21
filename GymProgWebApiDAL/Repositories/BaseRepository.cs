using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProgWebApiDAL.Repositories
{
    public abstract class BaseRepository<EntityType>  where EntityType:class,new()
    {
        public BaseRepository()
        {

        }
        public BaseRepository(DbContext context)
        {
            this.context = context;
        }
        public DbContext context { get; set; }
        public abstract DbSet<EntityType> getEntitySet();

        public List<EntityType> GetALL()
        {
            return getEntitySet().ToList<EntityType>();
        }

        public EntityType Get(object id)
        {
            return getEntitySet().Find(new object[]{id});
        }

        public IQueryable<EntityType> Query()
        {
            return getEntitySet().AsQueryable<EntityType>();
        }

        public void Delete(object EntityId)
        {
            try
            {
                getEntitySet().Remove(Get(EntityId));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Delete(IEnumerable<EntityType> EntityForDelete)
        {
            getEntitySet().RemoveRange(EntityForDelete);
        }

        public virtual void Update(EntityType EntityForUpdate)
        {
            try
            {
                context.Entry<EntityType>(EntityForUpdate).State = EntityState.Modified;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Add(EntityType EntityForAdd)
        {
            getEntitySet().Add(EntityForAdd);
        }


    }
}
