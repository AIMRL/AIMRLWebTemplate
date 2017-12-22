using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ServicesoAuthTester
{
    class Program
    {
        static void Main(string[] args)
        {



            using (var client = new HttpClient())
            {
                //bool isCertificate = Boolean.Parse(ConfigurationManager.AppSettings["isCertificateInstalled"].ToString());
                //if (isCertificate)
                //{
                //    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                //}

                var login = new Dictionary<string, string>
                   {
                       {"grant_type", "password"},
                       {"username", "test1"},
                       {"password",  "password"},
                   };
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                var resp = client.PostAsync("http://localhost:22935/Token", new FormUrlEncodedContent(login));
                Task.WaitAll(resp);

                //resp.
                //resp.Wait(TimeSpan.FromSeconds(10));


                if (resp.IsCompleted)
                {
                    if (resp.Result.Content.ReadAsStringAsync().Result.Contains("access_token"))
                    {
                        var tokenDetails = resp.Result.Content.ReadAsStringAsync().Result;

                    }
                }
            }


        }
    }
}
