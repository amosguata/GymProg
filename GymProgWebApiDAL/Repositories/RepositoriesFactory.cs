using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
    
namespace GymProgWebApiDAL.Repositories
{
    public class RepositoriesFactory
    {

        public const String DB_CONTEXT_KEY = "DB_CONTEXT";

        public static DbContext getContext()
        {
            object foundContext = null;
            HttpRequestMessage request = (HttpRequestMessage)HttpContext.Current.Items["MS_HttpRequestMessage"];
            request.Properties.TryGetValue(RepositoriesFactory.DB_CONTEXT_KEY,out foundContext);
            if (foundContext == null)
            {
                GymProgDBEntities dbContext = new GymProgDBEntities();
                request.Properties.Add(RepositoriesFactory.DB_CONTEXT_KEY, dbContext);
                return dbContext;
            }
            else
            {
                return (DbContext)foundContext;
            }
        }

        public static RepositoryType CreateRepository<RepositoryType, EntityType>(DbContext context)
          where RepositoryType : BaseRepository<EntityType>, new() where EntityType : class, new()
        {
            RepositoryType result = null;

            RepositoryType wantedReposetory = new RepositoryType() { context = context };
            result = (RepositoryType)Convert.ChangeType(wantedReposetory, typeof(RepositoryType));

            return result;
        }

        public static RepositoryType CreateRepository<RepositoryType, EntityType>()
           where RepositoryType : BaseRepository<EntityType>,new() where EntityType : class, new()
        {
            return CreateRepository<RepositoryType, EntityType>(getContext());
        }
    }
}

