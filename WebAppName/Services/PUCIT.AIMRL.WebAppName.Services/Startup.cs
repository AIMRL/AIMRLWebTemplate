using Microsoft.Owin;
using Owin;
//using IBEX.Tech.Force.WebAPI.ForceWebAPI;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Web.Http;
using PUCIT.AIMRL.WebAppName.Services.Providers;

//using IBEX.Tech.Force.WebAPI.ForceWebAPI.Providers;

[assembly: OwinStartup(typeof(PUCIT.AIMRL.WebAppName.Services.Startup))]
namespace PUCIT.AIMRL.WebAppName.Services
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            

            HttpConfiguration config = new HttpConfiguration();
            

            ConfigureOAuth(app);

            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings
.Add(new System.Net.Http.Formatting.RequestHeaderMapping("Accept",
                              "text/html",
                              StringComparison.InvariantCultureIgnoreCase,
                              true,
                              "application/json"));
        }
        public void ConfigureOAuth(IAppBuilder app)
        {
            //double tokenExpiryTime = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["AccessTokenExpiryDurationInMinutes"]);

            double tokenExpiryTime = 1;

            //OAuth Server Options
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"), //
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(tokenExpiryTime), //Access Token expiry time is picked up from web config

                Provider = new ApplicationOAuthProvider(), //pointing to OAuth Access Token provider class
                RefreshTokenProvider = new RefreshTokenProvider() //pointing to Refresh Token Provider class

            };

            /// configuring request and response object settings 
            app.Use(async (context, next) =>
            {
                IOwinRequest req = context.Request;
                IOwinResponse res = context.Response;
                if (req.Path.StartsWithSegments(new PathString("/Token")))
                {
                    var origin = req.Headers.Get("Origin");
                    if (!string.IsNullOrEmpty(origin))
                    {
                        res.Headers.Set("Access-Control-Allow-Origin", origin);
                    }
                    if (req.Method == "OPTIONS")
                    {
                        res.StatusCode = 200;
                        res.Headers.AppendCommaSeparatedValues("Access-Control-Allow-Methods", "GET", "POST");
                        res.Headers.AppendCommaSeparatedValues("Access-Control-Allow-Headers", "authorization", "content-type");
                        return;
                    }
                }
                await next();
            });

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions); //OAuthServerOptions object defined above for Access Token provider configurations.
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}