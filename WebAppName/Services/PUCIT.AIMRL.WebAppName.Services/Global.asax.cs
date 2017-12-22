using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace PUCIT.AIMRL.WebAppName.Services
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);





            GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings
.Add(new System.Net.Http.Formatting.RequestHeaderMapping("Accept",
                              "text/html",
                              StringComparison.InvariantCultureIgnoreCase,
                              true,
                              "application/json"));

            //var xmlFormatters = GlobalConfiguration.Configuration.Formatters.XmlFormatter;
            //var appXmlType = xmlFormatters.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            //GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
        }
    }
}
