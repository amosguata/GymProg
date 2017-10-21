using GymProgFramework.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GymProgWebApiBL.Controllers
{
    public class IpController : BaseControler
    { 
        [HttpGet]
        [Route("Ip")]
        public HttpResponseMessage GetClintIp()
        {
            String ip = TokenManager.GetAccesseingClientIp(Request);
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(ip, System.Text.Encoding.UTF8, "text/plain") }; ;
        }
    }
}
