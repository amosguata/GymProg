using GymProgFramework.Models;
using GymProgWebApiBL.App_Start;
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
    public class MuscleGroupsController : BaseControler
    {
        [Route("muscle-groups")]
        [HttpGet]
        [RestTokenAuthorization(0,1,2)]
        public ICollection<MuscleGroupDTO> GetUser()
        {
            return RepositoriesFactory.CreateRepository<MuscleGroupsRepository,MuscleGroup>()
                    .GetALL().Select<MuscleGroup,MuscleGroupDTO>(CurrMuscleGroup => new MuscleGroupDTO()
                    {
                        Id = CurrMuscleGroup.MuscleGroupId,
                        Name = CurrMuscleGroup.Name
                    }).ToList();
        }

    }
}
