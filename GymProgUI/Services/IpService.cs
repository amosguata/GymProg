using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GymProgUI.Services
{
    public class IpService : BaseService
    {
        public async Task<String> GetCurrentIp()
        {
            try
            {
                HttpResponseMessage response = await  getClient().GetAsync(this.RestUrl + "ip");
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                return e.Message;
            } 
        }
    }
}
