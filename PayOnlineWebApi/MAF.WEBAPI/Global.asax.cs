using MAF.WEBAPI.App_Start;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace MAF.WEBAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //GlobalConfiguration.Configure(WebApiConfig.Register);
            //GlobalConfiguration.Configure(Bootstrapper.Register);

            //try
            //{
            //    WebClient client = new WebClient();
            //    string reply = client.DownloadString(ConfigurationManager.AppSettings["APILoadBaseURL"]);
            //}
            //catch (Exception)
            //{ }
        }
    }
}
