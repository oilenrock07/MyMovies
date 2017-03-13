using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyMovies.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //Category
            routes.MapRoute(
                name: "MovieCategory",
                url: "Movie/Index/Category/{Category}",
                defaults: new { controller = "Movie", action = "Index", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "MovieCategoryPage",
                url: "Movie/Index/Category/{Category}/{Page}",
                defaults: new {controller = "Movie", action = "Index", id = UrlParameter.Optional}
                );

            //Star
            routes.MapRoute(
                name: "MovieStar",
                url: "Movie/Index/Star/{Star}",
                defaults: new {controller = "Movie", action = "Index", id = UrlParameter.Optional}
                );

            routes.MapRoute(
                name: "MovieStarPage",
                url: "Movie/Index/Star/{Star}/{Page}",
                defaults: new {controller = "Movie", action = "Index", id = UrlParameter.Optional}
                );

            //Search
            routes.MapRoute(
                name: "MovieSearch",
                url: "Movie/Index/Search/{Search}",
                defaults: new {controller = "Movie", action = "Index", id = UrlParameter.Optional}
                );

            routes.MapRoute(
                name: "MovieSearchPage",
                url: "Movie/Index/Search/{Search}/{Page}",
                defaults: new { controller = "Movie", action = "Index", id = UrlParameter.Optional }
                );

            //Director
            routes.MapRoute(
                name: "MovieDirector",
                url: "Movie/Index/Director/{Director}",
                defaults: new { controller = "Movie", action = "Index", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "MovieDirectorPage",
                url: "Movie/Index/Director/{Director}/{Page}",
                defaults: new { controller = "Movie", action = "Index", id = UrlParameter.Optional }
                );

            //Writer
            routes.MapRoute(
                name: "MovieWriter",
                url: "Movie/Index/Writer/{Writer}",
                defaults: new { controller = "Movie", action = "Index", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "MovieWriterPage",
                url: "Movie/Index/Writer/{Writer}/{Page}",
                defaults: new { controller = "Movie", action = "Index", id = UrlParameter.Optional }
                );

            //Defaults
            routes.MapRoute(
                name: "IndexPagingDefault",
                url: "Movie/Index/{Page}",
                defaults: new {controller = "Movie", action = "Index", id = UrlParameter.Optional}
                );

            routes.MapRoute(
                name: "AdminSearch",
                url: "Admin/Search/{key}",
                defaults: new {controller = "Admin", action = "Search", id = UrlParameter.Optional}
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Movie", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}