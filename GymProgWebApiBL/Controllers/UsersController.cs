using GymProgFramework.Authentication;
using GymProgFramework.Enums;
using GymProgFramework.Models;
using GymProgWebApiBL.App_Start;
using GymProgWebApiBL.Models;
using GymProgWebApiDAL;
using GymProgWebApiDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using static GymProgFramework.Enums.Enums;

namespace GymProgWebApiBL.Controllers
{
    public class UsersController : BaseControler
    {  
        private ActionResponse ValidateUser(User user)
        {
            UsersRepository repository = RepositoriesFactory.CreateRepository<UsersRepository, User>();

            if (repository.Query().Any(currUser => currUser.UserName == user.UserName && currUser.UserId != user.UserId))
            {
                return new ActionResponse() { CompletedSuccessfully = false, ErrorMessage = "a user with the given name already exists" };
            }
            else if (!Enum.IsDefined(typeof(PermissionsType), user.Permission))
            {
                return new ActionResponse() { CompletedSuccessfully = false, ErrorMessage = "invalid permission" };
            }

            return null;
        }

        [Route("users/{userName}")]
        [HttpGet]
        [RestTokenAuthorization(0,1,2)]
        public UserDTO GetUser(String userName)
        {
            UserDTO users = RepositoriesFactory.CreateRepository<UsersRepository, User>().Query()
                .Where(currUser => currUser.UserName == userName)
                .Select<User, UserDTO>(currUser => new UserDTO() { Name = currUser.UserName, Permission = currUser.Permission, UserId= currUser.UserId})
                .FirstOrDefault();

            return users;
        }

        [Route("users")]
        [HttpGet]
        [RestTokenAuthorization(2)]
        public ICollection<UserDTO> GetAllUsers()
        {
            return RepositoriesFactory.CreateRepository<UsersRepository, User>().GetALL()
                .Select<User, UserDTO>(currUser => new UserDTO() { Name = currUser.UserName, Permission = currUser.Permission, UserId = currUser.UserId }).ToList();
        }

        [Route("users/{userName}")]
        [HttpDelete]
        [RestTokenAuthorization(2)]
        public ActionResponse DeleteUser(String userName)
        {
            UsersRepository repository = RepositoriesFactory.CreateRepository<UsersRepository, User>();
            User user = repository.Query()
                .Where(currUser => currUser.UserName == userName)
                .FirstOrDefault();

            if (user == null)
            {
                return new ActionResponse() { CompletedSuccessfully = false, ErrorMessage = "User does not exist" };
            }

            repository.Delete(user.UserId);

            return new ActionResponse() { CompletedSuccessfully=true } ;
        }

        [Route("users")]
        [HttpPut]
        [RestTokenAuthorization(2)]
        public ActionResponse UpdateUser(User user)
        {
            ActionResponse response = ValidateUser(user);

            if (response != null)
            {
                return response;
            }
            UsersRepository repository = new UsersRepository();

            User existingUser = repository.Get(user.UserId);

            existingUser.UserName = user.UserName;
            existingUser.Password = TokenManager.HashUsingHmac(user.Password, TokenManager.key);
            existingUser.Permission = user.Permission;

            RepositoriesFactory.CreateRepository<UsersRepository, User>().Update(existingUser);

            return new ActionResponse() { CompletedSuccessfully = true, ErrorMessage = null };

        }

        [Route("users")]
        [HttpPost]
        [RestTokenAuthorization(2)]
        public ActionResponse CreateNewUser (User user)
        {
            ActionResponse response = ValidateUser(user);

            if (response != null)
            {
                return response;
            }
            
            RepositoriesFactory.CreateRepository<UsersRepository,User>().Add(new User()
            {
                UserName = user.UserName,
                Password = TokenManager.HashUsingHmac(user.Password, TokenManager.key),
                Permission = user.Permission
            });

            return new ActionResponse() { CompletedSuccessfully = true, ErrorMessage = null };
            
        }

    }   
}
