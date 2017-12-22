using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace ServicesoAuthWebTester
{
    public static class oAuthSecurityCooridnator
    {
        private static Boolean _isCertificate = false;
        private static String _appUserName = "";
        private static String _appPassword = "";
        private static String _apiTokenUrl = "";

        public static void ConfigureCoordinator(Boolean isCertificate, String appUserName, String appPassword, String apiTokenUrl)
        {
            _isCertificate = isCertificate;
            _appUserName = appUserName;
            _appPassword = appPassword;
            _apiTokenUrl = apiTokenUrl;

        }
        public static Dictionary<string, string> GetTokenDetails()
        {
            Dictionary<string, string> tokenDetails = null;
            try
            {
                //WebRequestHandler handler = new WebRequestHandler();
                //X509Certificate2 certificate = GetMyX509Certificate();
                //handler.ClientCertificates.Add(certificate);


                using (var client = new HttpClient())
                {
                    if (_isCertificate)
                    {
                        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    }
                    var login = new Dictionary<string, string>
                   {
                       {"grant_type", "password"},
                       {"username", _appUserName},
                       {"password",  _appPassword},
                   };
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                    var resp = client.PostAsync(_apiTokenUrl, new FormUrlEncodedContent(login));
                    Task.WaitAll(resp);

                    if (resp.IsCompleted)
                    {
                        if (resp.Result.Content.ReadAsStringAsync().Result.Contains("access_token"))
                        {
                            tokenDetails = JsonConvert.DeserializeObject<Dictionary<string, string>>(resp.Result.Content.ReadAsStringAsync().Result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return tokenDetails;
        }

        public static Dictionary<string, string> RefreshTokenDetails(string refresh_token)
        {
            Dictionary<string, string> tokenDetails = null;
            try
            {
                using (var client = new HttpClient())
                {
                    if (_isCertificate)
                    {
                        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    }
                    var login = new Dictionary<string, string>
                   {
                       {"grant_type", "refresh_token"},
                       {"refresh_token", refresh_token}
                   };
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                    var resp = client.PostAsync(_apiTokenUrl, new FormUrlEncodedContent(login));
                    Task.WaitAll(resp);

                    if (resp.IsCompleted)
                    {
                        if (resp.Result.Content.ReadAsStringAsync().Result.Contains("access_token"))
                        {
                            tokenDetails = JsonConvert.DeserializeObject<Dictionary<string, string>>(resp.Result.Content.ReadAsStringAsync().Result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return tokenDetails;
        }
    }
}