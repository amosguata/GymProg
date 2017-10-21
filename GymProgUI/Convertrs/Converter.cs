using GymProgFramework.Models;
using GymProgUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProgUI.Convertrs
{
    public static class Converter
    {
        public static EntityType Convert<EntityType>(ProgramDrillDTO from, bool areSetsEditable = false) where EntityType : class
        {
            EntityType entityForReturn = null;

            if (typeof(EntityType) == typeof(ProgramDrillViewModel))
            {
                List<SetViewModel> drillSets = from.Sets.Select<SetDTO, SetViewModel>(currSet =>
                {
                    return new SetViewModel(currSet) { IsEditable = areSetsEditable, Id = currSet.Id };
                }).ToList();

                if (areSetsEditable)
                {
                    int SetsNum = drillSets.Count();
                    int setsToCreate = 5 - SetsNum;
                    List<SetViewModel> emptySets = new List<SetViewModel>();

                    if (setsToCreate > 0)
                    {
                        while (setsToCreate > 0)
                        {
                            emptySets.Add(new SetViewModel() { IsEditable = true, IsVisable = true });
                            setsToCreate--;
                        }

                        drillSets.AddRange(emptySets);
                    }
                }

                ProgramDrillViewModel convertedEntity = new ProgramDrillViewModel()
                {
                    Id = from.Id,
                    Name = from.Drill.Name,
                    Drill = from.Drill,
                    Sets = drillSets
                };

                entityForReturn = (EntityType)System.Convert.ChangeType(convertedEntity, typeof(EntityType));
            }

            return entityForReturn;
        }


        public static EntityType Convert<EntityType>(ProgramDrillViewModel from) where EntityType : class
        {
            EntityType entityForReturn = null;

            if (typeof(EntityType) == typeof(ProgramDrillDTO))
            {

                ProgramDrillDTO convertedProgram = new ProgramDrillDTO()
                {
                    Drill = from.Drill,
                    Id = from.Id,
                    Sets = from.Sets
                            .Where(CurrSet => CurrSet.Repetitions != 0 || CurrSet.Weight != 0)
                            .Select<SetViewModel, SetDTO>(currSet =>
                                    {
                                        return new SetDTO()
                                        {
                                            Repetitions = currSet.Repetitions,
                                            Weight = currSet.Weight,
                                            Id = currSet.Id
                                        };
                                    }).ToList()
                };
                entityForReturn = (EntityType)System.Convert.ChangeType(convertedProgram, typeof(EntityType));
            }

            return entityForReturn;
        }

        public static EntityType Convert<EntityType>(BaseProgramViewModel from) where EntityType : class
        {
            EntityType entityForReturn = null;

            if (typeof(EntityType) == typeof(ProgramDTO))
            {
                ProgramDTO convertedProgram = new ProgramDTO()
                {
                    Name = from.ProgramName,
                    Drills = from.Drills.Select<ProgramDrillViewModel, ProgramDrillDTO>(currDrill => Converter.Convert<ProgramDrillDTO>(currDrill)).ToList()
                };

                entityForReturn = (EntityType)System.Convert.ChangeType(convertedProgram, typeof(EntityType));
            }

            return entityForReturn;
        }
    }
}
