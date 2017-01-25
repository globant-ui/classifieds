﻿using System.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;
using Classifieds.IOC;


namespace Classifieds.MasterDataAPI
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
            UnityConfig.RegisterComponents(config);
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "Category",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
