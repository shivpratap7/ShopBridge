using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

using ShopBridgeAPI.Providers;
using System.Web.Http.Cors;
using System.Net.Http.Formatting;

namespace ShopBridgeAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());

            config.EnableCors(new EnableCorsAttribute("*", headers: "*", methods: "*"));

            // Web API routes
        
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, action = RouteParameter.Optional }
            );
            config.MapHttpAttributeRoutes();
            //       config.Filters.Add(new ApiAuthenticationFilter(true));
            //  config.Filters.Add(new MyExceptionFilterAttribute());

        }
    }
}
