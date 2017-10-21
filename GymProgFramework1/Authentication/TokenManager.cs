using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using ECL.Auth.HMAC.Services;
using PCLCrypto;
namespace GymProgFramework.Authentication
{
    public class TokenManager
    {
        public const String key = "f4619bbb-51b2-4426-ba60-efae2218ee93";
        private const int EXPERATION_IN_MINUTES = 30;
        public const String TOKEN_HEADER_NAME = "GYMPROG_TOKEN";

        private const string HttpContext = "MS_HttpContext";
        private const string RemoteEndpointMessage = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";

        public static String HashUsingHmac(String value,String Key)
        {
            HmacSignatureCalculator calc = new ECL.Auth.HMAC.Services.HmacSignatureCalculator();

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(calc.CalculateSignature(Key, value)));
        }

        public static String GenerateToken(String username, String PassWord, String ip, long timeStamp)
        {
            String HashedPassword = HashUsingHmac(PassWord, key);

            
            String hashedContent = String.Join(":", new string[] { username, ip, timeStamp.ToString() });
            hashedContent = HashUsingHmac(hashedContent, HashedPassword);

            String token = String.Join(":", new string[] { hashedContent, username, timeStamp.ToString() });

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
        }

        public static Boolean IsTokenValid(String Token, String userName, String HashedPassword, String ip, String timeStamp)
        {
            String hashedContent = String.Join(":", new string[] { userName, ip, timeStamp.ToString() });
            hashedContent = HashUsingHmac(hashedContent, HashedPassword); 

            String token = String.Join(":", new string[] { hashedContent, userName, timeStamp.ToString() });

            DateTime tokenTime = new DateTime(long.Parse(timeStamp));
            if (Token != Convert.ToBase64String(Encoding.UTF8.GetBytes(token)))
            {
                return false;
            }
            if (Math.Abs((DateTime.UtcNow- tokenTime).TotalMinutes) > EXPERATION_IN_MINUTES)
            {
                return false;
            }

            return true;
        }

        public static String ExtractUserNameFromToken(String token)
        {
            return (Encoding.UTF8.GetString(Convert.FromBase64String(token),0, Convert.FromBase64String(token).Length).Split(':')[1]);
        }

        public static String ExtractUserTimesatmpFromToken(String token)
        {
            return (Encoding.UTF8.GetString(Convert.FromBase64String(token), 0, Convert.FromBase64String(token).Length).Split(':')[2]);
        }

        public static String ExtractHashedContentFromToken(String token)
        {
            return (Encoding.UTF8.GetString(Convert.FromBase64String(token), 0, Convert.FromBase64String(token).Length).Split(':')[0]);
        }

        public static String GetAccesseingClientIp(HttpRequestMessage request)
        {
            try
            {
                //Web-hosting
                if (request.Properties.ContainsKey(HttpContext))
                {
                    dynamic ctx = request.Properties[HttpContext];
                    if (ctx != null)
                    {
                        return ctx.Request.UserHostAddress;
                    }
                }
                //Self-hosting
                if (request.Properties.ContainsKey(RemoteEndpointMessage))
                {
                    dynamic remoteEndpoint = request.Properties[RemoteEndpointMessage];
                    if (remoteEndpoint != null)
                    {
                        return remoteEndpoint.Address;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return null;
        }
    }
}
