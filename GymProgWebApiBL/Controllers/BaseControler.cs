using GymProgFramework.Authentication;
using GymProgWebApiDAL;
using GymProgWebApiDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace GymProgWebApiBL.Controllers
{
    public class BaseControler : ApiController
    {
        private User _currUser = null;

        protected User GetCurrentUser()
        {
            if (_currUser == null)
            {
                IEnumerable<String> AuthenticationHeaders;
                Request.Headers.TryGetValues(TokenManager.TOKEN_HEADER_NAME, out AuthenticationHeaders);
                String token = AuthenticationHeaders.First();
                String UserName = TokenManager.ExtractUserNameFromToken(token);
                _currUser = RepositoriesFactory.CreateRepository<UsersRepository, User>().Query().FirstOrDefault(currUser => currUser.UserName == UserName);
            }

            return _currUser;
        }
    }
}