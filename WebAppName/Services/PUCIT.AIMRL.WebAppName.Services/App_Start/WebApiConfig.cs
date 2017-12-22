using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace PUCIT.AIMRL.WebAppName.Services
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            //var cors = new EnableCorsAttribute("*", "*", "*");
            //config.EnableCors(cors);


            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));



            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}",
                defaults: new { id = RouteParameter.Optional }
            );

            //var xmlFormatters = GlobalConfiguration.Configuration.Formatters.XmlFormatter;
            //var appXmlType = xmlFormatters.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            //GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

            //var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            //jsonFormatter.SerializerSettings.ContractResolver = new DefaultContractResolver();


//            GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings
//.Add(new System.Net.Http.Formatting.RequestHeaderMapping("Accept",
//                              "text/html",
//                              StringComparison.InvariantCultureIgnoreCase,
//                              true,
//                              "application/json"));
        }
    }
}
