using GymProgFramework.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GymProgUI.Services
{
    public class MuscleGroupsService : BaseService
    {
        public async Task<ICollection<MuscleGroupDTO>> GetAllMuscleGroups()
        {
            HttpRequestMessage request = await getRequest(HttpMethod.Get, "muscle-groups");
          
            HttpResponseMessage response = await getClient().SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return null; 
            }

            return JsonConvert.DeserializeObject<ICollection<MuscleGroupDTO>>(await response.Content.ReadAsStringAsync());
        }
    }
}
