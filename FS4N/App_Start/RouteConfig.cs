using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FS4N
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "displayAndSave",
                url: "save/{ip}/{port}/{frequency}/{duration}/{file}",
                defaults: new { controller = "Map", action = "displayAndSave" }
            );

            routes.MapRoute(
                name: "display",
                url: "display/{ip}/{port}",
                defaults: new { controller = "Map", action = "display" }
            );

            routes.MapRoute(
                name: "displaySavedFlight",
                url: "display/{file}/{frequency}",
                defaults: new { controller = "Map", action = "displaySavedFlight" }
            );

            routes.MapRoute(
                name: "displayTime",
                url: "display/{ip}/{port}/{time}",
                defaults: new { controller = "Map", action = "displayTime" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Map", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
