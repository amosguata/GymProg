using GymProgFramework.Authentication;
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
    public class UsersService : BaseService
    {

        public UsersService() : base()
        {
                
        }

        public async Task<ActionResponse> DeleteUser(UserDTO userForDelete)
        {
            HttpRequestMessage request = await getRequest(HttpMethod.Delete, "users/" + userForDelete.Name);

            HttpResponseMessage response = await getClient().SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return new ActionResponse() { ErrorMessage = "Operation failed User did not delete properly", CompletedSuccessfully = false };

            }

            return JsonConvert.DeserializeObject<ActionResponse>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ICollection<UserDTO>> GetAllUsers()
        {
            HttpRequestMessage request = await getRequest(HttpMethod.Get, "users");

            HttpResponseMessage response = await getClient().SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<ICollection<UserDTO>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ActionResponse> UpdateUser(UserDTO user)
        {
            try
            {
                HttpRequestMessage request = await getRequest(HttpMethod.Put, "users");
                request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await getClient().SendAsync(request);
                return JsonConvert.DeserializeObject<ActionResponse>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                return new ActionResponse() { ErrorMessage = e.Message, CompletedSuccessfully = false };
            }

        }

        public async Task<ActionResponse> CreateNewUser(UserDTO Newuser)
        {
            try
            {
                HttpRequestMessage request = await getRequest(HttpMethod.Post, "users");
                request.Content = new StringContent(JsonConvert.SerializeObject(Newuser),Encoding.UTF8, "application/json");
                
                HttpResponseMessage response = await getClient().SendAsync(request);
                return JsonConvert.DeserializeObject<ActionResponse>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                return new ActionResponse() {  ErrorMessage = e.Message, CompletedSuccessfully = false };
            }
           
        }
    }
}
