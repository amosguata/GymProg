using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace GymProgWebApiDAL.Repositories
{
    public class WebApiDbContextCommit : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
            try
            {
                object requestContext = null;
                if (actionExecutedContext.Request.Properties.TryGetValue(RepositoriesFactory.DB_CONTEXT_KEY, out requestContext))
                {
                    DbContext context = ((DbContext)requestContext);
                    context.SaveChanges();
                    context.Dispose();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
