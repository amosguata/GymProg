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

namespace GymProgWebApiBL.Controllers
{
    public class DrillsController : BaseControler
    {
        private User _currUser = null;

        private User GetCurrentUser()
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

        private Drill ConvertDTOToEntity(DrillDTO drill)
        {
            return new Drill()
            {
                DrillId = drill.Id, 
                Description = drill.Description,
                Name = drill.Name,
                DrillMuscleGroups = drill.MuscleGroups.Select<MuscleGroupDTO, DrillMuscleGroup>(currDrillDTO =>
                {
                    return new DrillMuscleGroup() { MuscleGroupId = currDrillDTO.Id };
                }).ToList()
            };
        }

        private DrillDTO ConvertEntityTODTO(Drill drill)
        {
            IList<MuscleGroupDTO> muscleGroup = drill.DrillMuscleGroups
                          .Select<DrillMuscleGroup, MuscleGroupDTO>(currDrilMuscleGroup =>
                          {
                              return new MuscleGroupDTO()
                              {
                                  Name = currDrilMuscleGroup.MuscleGroup.Name,
                                  Id = currDrilMuscleGroup.MuscleGroup.MuscleGroupId
                              };
                          }).ToList();

            return new DrillDTO()
            {
                Description = drill.Description,
                Name = drill.Name,
                MuscleGroups = muscleGroup,
                Id = drill.DrillId
            };
        }

        private ActionResponse ValidateDrill(DrillDTO drill, bool isUpdate = false )
        {
            DrillsRepository repository = RepositoriesFactory.CreateRepository<DrillsRepository, Drill>();
            MuscleGroupsRepository muscleGroupRep = RepositoriesFactory.CreateRepository<MuscleGroupsRepository, MuscleGroup>();

            if (repository.Query().Any(currDrill => currDrill.DrillId != drill.Id && currDrill.Name == drill.Name))
            {
                return new ActionResponse()
                {
                    CompletedSuccessfully = false,
                    ErrorMessage = "A drill with the given name already exists"
                };
            }

            bool doAllMuscleGroupsExist = drill.MuscleGroups.All(currNewMuscleGroup => {
                return muscleGroupRep.Query().Any(CurrMuscleGroup => CurrMuscleGroup.MuscleGroupId == currNewMuscleGroup.Id);
            });

            if (!doAllMuscleGroupsExist)
            {
                return new ActionResponse()
                {
                    CompletedSuccessfully = false,
                    ErrorMessage = "None existing muscle group"
                };
            }

            return null;
        }

        private ActionResponse CanUserEditDrill (int drillId)
        {
            Drill wantedDrill = RepositoriesFactory.CreateRepository<DrillsRepository, Drill>()
                .Query().FirstOrDefault(currDrill => currDrill.DrillId == drillId);

            if (wantedDrill == null)
            {
                return new ActionResponse() { ErrorMessage = "Wanted drill does not exists", CompletedSuccessfully = false };
            }

            if (!(GetCurrentUser().Permission == (int)Enums.PermissionsType.Admin))
            {
                if (wantedDrill.CreatorUserId != GetCurrentUser().UserId)
                {
                    return new ActionResponse() { ErrorMessage = "The given user does not have permissions to update the given drill", CompletedSuccessfully = false };
                }
            }

            return null;
        }

        [Route("drills")]
        [HttpGet]
        [RestTokenAuthorization(0,1,2)]
        public ICollection<DrillDTO> GetAllDrills()
        {
            return RepositoriesFactory.CreateRepository<DrillsRepository, Drill>().GetALL()
                .Select<Drill, DrillDTO>(currDrill => ConvertEntityTODTO(currDrill)).ToList();
        }

        [Route("drills/{UserName}")]
        [HttpGet]
        [RestTokenAuthorization(0, 2)]
        public ICollection<DrillDTO> GetUserDrills(String UserName)
        {
            User user = GetCurrentUser();
            if (user.Permission == (int)Enums.PermissionsType.Admin)
            {
                return GetAllDrills();
            }
            else
            {
                return RepositoriesFactory.CreateRepository<DrillsRepository, Drill>().GetALL().Where(currDrill => currDrill.CreatorUserId == user.UserId)
                .Select<Drill, DrillDTO>(currDrill => ConvertEntityTODTO(currDrill)).ToList();
            }
        }

        [Route("drills/{drillId}")]
        [HttpDelete]
        [RestTokenAuthorization(0, 2)]
        public ActionResponse DeleteDrill(int drillId)
        {
            ActionResponse response;
            response = CanUserEditDrill(drillId);

            if (response != null)
            {
                return response;
            }

            DrillsRepository drillRepository = RepositoriesFactory.CreateRepository<DrillsRepository, Drill>();

            drillRepository.Delete(drillId);

            return new ActionResponse() { CompletedSuccessfully = true };
        }

        [Route("drills")]
        [HttpPut]
        [RestTokenAuthorization(0, 2)]
        public ActionResponse UpdateDrill(DrillDTO newDrill)
        {
            ActionResponse response;
            response = ValidateDrill(newDrill);

            if (response != null)
            {
                return response;
            }

            response = CanUserEditDrill(newDrill.Id);

            if (response != null)
            {
                return response;
            }


            Drill drillForUpdate = ConvertDTOToEntity(newDrill);

            drillForUpdate.CreatorUserId = GetCurrentUser().UserId;
            DrillsRepository drillRepository = RepositoriesFactory.CreateRepository<DrillsRepository, Drill>();

            Drill existingDrill = drillRepository.Get(drillForUpdate.DrillId);

            existingDrill.Name = drillForUpdate.Name;
            existingDrill.Description = drillForUpdate.Description;
            existingDrill.DrillMuscleGroups = drillForUpdate.DrillMuscleGroups;

            drillRepository.Update(existingDrill);      

            return new ActionResponse() { CompletedSuccessfully = true };
        }


        [Route("drills")]
        [HttpPost]
        [RestTokenAuthorization(0,2)]
        public ActionResponse AddNewDrill(DrillDTO newDrill)
        {
            ActionResponse response;

            response = ValidateDrill(newDrill);

            if (response != null )
            {
                return response;
            }

            Drill drillToSave = ConvertDTOToEntity(newDrill);
            drillToSave.CreatorUserId = GetCurrentUser().UserId;
            RepositoriesFactory.CreateRepository<DrillsRepository, Drill>().Add(drillToSave);

            response = new ActionResponse();
            response.CompletedSuccessfully = true;
            return response;
        }
    }
}
