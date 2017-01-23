using System.Web.Http;
using Classifieds.IOC;
using System.Web.Http.Cors;
using System.Configuration;

namespace Classifieds.SearchAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //CORS enabled globaly need to configure server url environment
            var cors = new EnableCorsAttribute(ConfigurationManager.AppSettings["CORSUrl"], "*", "*");
            cors.SupportsCredentials = true;
            config.EnableCors(cors);
            // Web API configuration and services
            UnityConfig.RegisterComponents(config);
           
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "Search",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
