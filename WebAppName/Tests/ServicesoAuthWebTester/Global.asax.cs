using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ServicesoAuthWebTester
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            oAuthSecurityCooridnator.ConfigureCoordinator(false, "test", "password", "http://localhost:22935/Token");

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
