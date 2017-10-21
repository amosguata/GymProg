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
    public class ProgramsController : BaseControler
    {
        private Program ConvertDTOToEntity(ProgramDTO program, bool shouldSetCurrentUser = false )
        {
           Program returnProgram = new Program()
            {
               ProgramId = program.Id,
                ProgramName = program.Name,
                ProgramDrills = program.Drills.Select<ProgramDrillDTO, ProgramDrill>(CurrDrill =>
                {
                    int currDrillOrder = 0;
                    return new ProgramDrill()
                    {
                        ProgramId = program.Id,
                        DrillId = CurrDrill.Drill.Id,
                        Id = CurrDrill.Id,
                        Sets = CurrDrill.Sets.Select<SetDTO, Set>(currSet =>
                        {
                            currDrillOrder++;
                            return new Set()
                            {
                                ProgramDrillId = CurrDrill.Drill.Id,
                                OrderId = currDrillOrder,
                                Repetitions = (short)currSet.Repetitions,
                                Weight = (short)currSet.Weight,
                                SetId = currSet.Id
                            };
                        }).ToList()
                    };
                }).ToList(),
                
            };

            if(shouldSetCurrentUser)
            {
                returnProgram.UserPrograms = new UserProgram[] { new UserProgram() { UserId = this.GetCurrentUser().UserId } };
            }

            return returnProgram;
        }

        private ProgramDTO ConvertEntityTODTO(Program program)
        {
            return new ProgramDTO()
            {
                Id = program.ProgramId,
                Name = program.ProgramName,
                Drills = program.ProgramDrills.Select<ProgramDrill, ProgramDrillDTO>(currProgramDrill =>
                {
                    return new ProgramDrillDTO()
                    {
                        Id = currProgramDrill.Id,
                        Drill = new DrillDTO()
                        {

                            Id = currProgramDrill.Drill.DrillId,
                            Description = currProgramDrill.Drill.Description,
                            Name = currProgramDrill.Drill.Name,
                            MuscleGroups = currProgramDrill.Drill.DrillMuscleGroups.Select<DrillMuscleGroup, MuscleGroupDTO>(currMuscleGroup =>
                            {
                                return new MuscleGroupDTO() { Id = currMuscleGroup.Id, Name = currMuscleGroup.MuscleGroup.Name };
                            }).ToList()
                        },
                        Sets = currProgramDrill.Sets.OrderBy(curr => curr.OrderId).Select<Set, SetDTO>(currSet =>
                        {
                            return new SetDTO()
                            {
                                Repetitions = currSet.Repetitions,
                                Weight = currSet.Weight,
                                Id = currSet.SetId
                            };
                        }).ToList()
                    };
                }).ToList()
            };
        }

        private ActionResponse ValidateProgram(ProgramDTO newProgram, bool isUpdate = false)
        {
            ActionResponse response = null;
            try
            {
                if (RepositoriesFactory.CreateRepository<ProgramRepository, Program>().Query()
                .Any(currProgram => currProgram.ProgramId != newProgram.Id  && currProgram.ProgramName == newProgram.Name))
                {
                    return new ActionResponse() { CompletedSuccessfully = false, ErrorMessage = "A Program with the given name already exists" };
                }

            }
            catch (Exception e)
            {

                throw e;
            }
            
            return response; 
        }

        private ActionResponse CanUserEditProgram(int ProgramId)
        {
            Program wantedProgram = RepositoriesFactory.CreateRepository<ProgramRepository, Program>()
                .Query().FirstOrDefault(currProgram => currProgram.ProgramId == ProgramId);

            if (wantedProgram  == null)
            {
                return new ActionResponse() { ErrorMessage = "Wanted Program does not exists", CompletedSuccessfully = false };
            }

            if (!(GetCurrentUser().Permission == (int)Enums.PermissionsType.Admin))
            {
                if (!wantedProgram.UserPrograms.Any(currUserProgram => currUserProgram.UserId == GetCurrentUser().UserId))
                {
                    return new ActionResponse() { ErrorMessage = "The given user does not have permissions to update the given program", CompletedSuccessfully = false };
                }
            }

            return null;
        }

        [Route("programs/{programId}")]
        [HttpDelete]
        [RestTokenAuthorization(0, 2)]
        public ActionResponse DeleteProgram(int programId)
        {
            ActionResponse response = CanUserEditProgram(programId);

            if (response != null)
            {
                return response;
            }

            ProgramRepository programRepository = RepositoriesFactory.CreateRepository<ProgramRepository, Program>();

            programRepository.Delete(programId);

            return new ActionResponse() { CompletedSuccessfully = true };
           
        }

        [Route("programs")]
        [HttpPut]
        [RestTokenAuthorization(0, 2)]
        public ActionResponse UpdateProgram(ProgramDTO program)
        {
            ActionResponse response = ValidateProgram(program, true);

            if (response != null)
            {
                return response;
            }

             response = CanUserEditProgram(program.Id);

            if (response != null)
            {
                return response;
            }

            Program programForUpdate = ConvertDTOToEntity(program);

            ProgramRepository programRepository = RepositoriesFactory.CreateRepository<ProgramRepository, Program>();

            Program existingProgram = programRepository.Get(programForUpdate.ProgramId);

            existingProgram.ProgramName = programForUpdate.ProgramName;

            foreach (ProgramDrill currDrill in programForUpdate.ProgramDrills)
            {
                ProgramDrill existingProgramDrill =
                    existingProgram.ProgramDrills.FirstOrDefault(currProgramDrill => currProgramDrill.Id == currDrill.Id);
                if (existingProgramDrill == null)
                {
                    existingProgram.ProgramDrills.Add(currDrill);
                }
                else
                {
                    foreach (Set currSet in currDrill.Sets)
                    {
                        Set existingSet = 
                            existingProgramDrill.Sets.FirstOrDefault(currExistingSet => currExistingSet.SetId == currSet.SetId);

                        if (existingSet == null)
                        {
                            existingProgramDrill.Sets.Add(currSet);
                        }
                        else
                        {
                            existingSet.Repetitions = currSet.Repetitions;
                            existingSet.Weight = currSet.Weight;
                            existingSet.Repetitions = currSet.Repetitions;
                        }
                    }
                }
            }

            programRepository.Update(existingProgram);
            return new ActionResponse() { CompletedSuccessfully = true };
        }

        [Route("programs")]
        [HttpPost]
        [RestTokenAuthorization(0,2)]
        public ActionResponse AddNewProgram(ProgramDTO newProgram)
        {
            ProgramRepository repository = RepositoriesFactory.CreateRepository<ProgramRepository, Program>();

            ActionResponse response = ValidateProgram(newProgram);
            if (response == null)
            {
                Program programToSave = ConvertDTOToEntity(newProgram, true);

                repository.Add(programToSave);

                return new ActionResponse() { CompletedSuccessfully = true };
            }
            else
            {
                return response;
            }
        }


        [Route("programs/{userName}")]
        [HttpGet]
        [RestTokenAuthorization(0,1,2)]
        public ICollection<ProgramDTO> GetUsersPrograms(ProgramDTO newProgram, String userName)
        {
            ActionResponse response = new ActionResponse();

            ProgramRepository repository = RepositoriesFactory.CreateRepository<ProgramRepository, Program>();

            int UserId = GetCurrentUser().UserId;

            ICollection<Program> wantedPrograms;

            if (GetCurrentUser().Permission == (int)Enums.PermissionsType.Admin)
            {
                wantedPrograms = repository.GetALL();
            }
            else
            {
                wantedPrograms =
                    repository.Query().Where(CurrProgram => CurrProgram.UserPrograms.Any(currUser => currUser.UserId == UserId)).ToList();
            }
          

            ICollection<ProgramDTO> ProgramDrills = 
                wantedPrograms.Select<Program, ProgramDTO>(currProgram => ConvertEntityTODTO(currProgram)).ToList();

            return ProgramDrills;
        }

        [Route("programs")]
        [HttpGet]
        [RestTokenAuthorization(0,1,2)]
        public ICollection<ProgramDTO> GetAllPrograms(ProgramDTO newProgram)
        {
            ActionResponse response = new ActionResponse();

            ProgramRepository repository = RepositoriesFactory.CreateRepository<ProgramRepository, Program>();

            ICollection<Program> wantedPrograms = repository.GetALL();

            ICollection<ProgramDTO> ProgramDrills = wantedPrograms
                .Select<Program, ProgramDTO>(currProgram => ConvertEntityTODTO(currProgram)).ToList();

            return ProgramDrills;
        }

    }
}
