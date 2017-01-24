using System.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;
using Classifieds.IOC;

namespace Classifieds.UserServiceAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //CORS enabled globaly need to configure server url environment
            var cors = new EnableCorsAttribute(ConfigurationManager.AppSettings["CorsUrl"], "*", "*");
            cors.SupportsCredentials = true;
            config.EnableCors(cors);
            // Web API configuration and services
            //config.Filters.Add(new BasicAuthorization());
            UnityConfig.RegisterComponents(config);
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "UserServiceAPI",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
