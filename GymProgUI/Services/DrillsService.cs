using GymProgFramework.Models;
using GymProgWebApiBL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GymProgUI.Services
{
    public class DrillsService : BaseService
    {
        public async Task<ActionResponse> AddnewDrill(DrillDTO newDrill)
        {
            HttpRequestMessage request = await getRequest(HttpMethod.Post, "drills");
            request.Content = new StringContent(JsonConvert.SerializeObject(newDrill), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await getClient().SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return new ActionResponse() { ErrorMessage = "Operation failed drill did not save properly", CompletedSuccessfully = false };

            }

            return JsonConvert.DeserializeObject<ActionResponse>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ICollection<DrillDTO>> GetAllDrills()
        {
            HttpRequestMessage request = await getRequest(HttpMethod.Get, "drills");

            HttpResponseMessage response = await getClient().SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<ICollection<DrillDTO>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ICollection<DrillDTO>> GetUserDrills()
        {
            HttpRequestMessage request = await getRequest(HttpMethod.Get, "drills/" + (await new UsersService().GetUser(null)).Name);

            HttpResponseMessage response = await getClient().SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<ICollection<DrillDTO>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ActionResponse> UpdateDrill(DrillDTO drillForUpdate)
        {
            HttpRequestMessage request = await getRequest(HttpMethod.Put, "drills");
            request.Content = new StringContent(JsonConvert.SerializeObject(drillForUpdate), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await getClient().SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return new ActionResponse() { ErrorMessage = "Operation failed drill did not save properly", CompletedSuccessfully = false };

            }

            return JsonConvert.DeserializeObject<ActionResponse>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ActionResponse> DeleteDrill(DrillDTO drillForDelete)
        {
            HttpRequestMessage request = await getRequest(HttpMethod.Delete, "drills/" + drillForDelete.Id);

            HttpResponseMessage response = await getClient().SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return new ActionResponse() { ErrorMessage = "Operation failed dril did not delete properly", CompletedSuccessfully = false };

            }

            return JsonConvert.DeserializeObject<ActionResponse>(await response.Content.ReadAsStringAsync());

        }
    }
}
