using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace ng.Net1
{
    public static class WebApiConfig
    {
        //http://www.asp.net/web-api/overview/web-api-routing-and-actions/routing-in-aspnet-web-api
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            // Use camelCase for JSON data. 
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Filters.Add(new ApiResultAttribute());

            config.Filters.Add(new ApiErrorHandleAttribute());

            // 全域層級使用 [Authorize]
            config.Filters.Add(new AuthorizeAttribute());
            config.Filters.Add(new RoleMenuAuthorizeAttribute());

            //I chaned the routeTemplate so that methods/services would be identified by their action, and not by their parameters.
            //I was getting conflicts if I had more than one GET services, that had identical parameter options, but totally different return data.
            //Adding the action to the routeTemplte correct this issue.
            /*
            config.Routes.MapHttpRoute(
              name: "PagingApi",
              routeTemplate: "api/{controller}/{pageSize}/{pageNumber}/{orderBy}",
              defaults: new
              {
                  pageSize = 10,
                  pageNumber = 0,
                  orderBy = RouteParameter.Optional
              }
            );
            */
            config.Routes.MapHttpRoute(
               name: "ActioinApi",
               routeTemplate: "api/ud/{controller}/{action}",
               defaults: new
               {
                   action = "GetCurrentUserName"
               }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}", //routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
