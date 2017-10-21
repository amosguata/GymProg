using GymProgFramework.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using GymProgWebApiDAL;
using GymProgWebApiDAL.Repositories;
using GymProgFramework.Enums;

namespace GymProgWebApiBL.App_Start
{
    public class RestTokenAuthorizationAttribute : AuthorizeAttribute
    {
        public RestTokenAuthorizationAttribute(params int[] allowedPermissionTypes)
        {
            _allowedPermissionTypes = allowedPermissionTypes;
        }
        public int[] _allowedPermissionTypes  { get; set; }
        public override void OnAuthorization(HttpActionContext filterContext)
        {
            if (Authorize(filterContext))
            {
                return;
            }
            HandleUnauthorizedRequest(filterContext);
        }
        protected override void HandleUnauthorizedRequest(HttpActionContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
        }
        private bool Authorize(HttpActionContext actionContext)
        {
            try
            { 
                IEnumerable<String> AuthenticationHeaders;
                actionContext.Request.Headers.TryGetValues(TokenManager.TOKEN_HEADER_NAME,out AuthenticationHeaders);
                String token = AuthenticationHeaders.First();

                String userName = TokenManager.ExtractUserNameFromToken(token);
                String timeStamp = TokenManager.ExtractUserTimesatmpFromToken(token);


                User WantedUser = RepositoriesFactory.CreateRepository<UsersRepository, User>()
                    .Query().FirstOrDefault<User>(currUser => currUser.UserName == userName);

                if (WantedUser == null)
                {
                    return false;
                }
                else if (!_allowedPermissionTypes.Contains(WantedUser.Permission))
                {
                    return false;
                }
               

                String hashedPassword = WantedUser.Password;

                return TokenManager.IsTokenValid(token, userName, hashedPassword, TokenManager.GetAccesseingClientIp(actionContext.Request), timeStamp);
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}