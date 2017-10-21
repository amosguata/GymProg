using GymProgFramework.Authentication;
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
    public class BaseService
    {
        public String RestUrl { get { return @"http://169.254.80.80/"; } }

        private static HttpClient client { get; set; }

        private static UserDTO User { get; set; }

        public static bool IsLogedIn { get {return User != null;} }

        public async Task<UserDTO> GetUser(UserDTO user, bool UseCache = true)
        {
            try
            {
                if (user == null || (UseCache && UsersService.User != null))
                {
                    return UsersService.User;
                }
                else
                {
                    UserDTO foundUser = null;
                    BaseService.User = user;
                    HttpRequestMessage request = await getRequest(HttpMethod.Get, "users/" + user.Name);

                
                    HttpResponseMessage response = await getClient().SendAsync(request);
                    if (!response.IsSuccessStatusCode)
                    {
                        foundUser = null;
                    }
                    else
                    {
                        foundUser = JsonConvert.DeserializeObject<UserDTO>(await response.Content.ReadAsStringAsync());
                        if (foundUser == null)
                        {
                            foundUser = null;
                        }
                    }
                    foundUser.Password = user.Password;
                    // Get the user 
                    return UsersService.User = foundUser;
                }
            }
            catch (Exception e)
            {
                return UsersService.User = null;
            }
        }

        public HttpClient getClient()
        {
            if (client == null)
            {
                client = new HttpClient();
            }

            return client;
        }


        public async Task<HttpRequestMessage> getRequest(HttpMethod method, String urlSuffix)
        {
            HttpRequestMessage request = new HttpRequestMessage(method, RestUrl + urlSuffix);
            if (UsersService.User != null)
            {
                String ip = await new IpService().GetCurrentIp();
                request.Headers.Add(TokenManager.TOKEN_HEADER_NAME, TokenManager.GenerateToken(UsersService.User.Name, UsersService.User.Password, ip, DateTime.UtcNow.Ticks));

            }
            return request;
        }
    }
}
