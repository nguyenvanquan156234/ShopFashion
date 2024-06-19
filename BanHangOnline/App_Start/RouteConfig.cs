using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BanHangOnline
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Product",
                url: "san-pham",
                defaults: new { controller = "Product", action = "Index", alias = UrlParameter.Optional },
                namespaces: new[] { "BanHangOnline.Controllers"}
            );
            routes.MapRoute(
               name: "Cart",
               url: "gio-hang",
               defaults: new { controller = "ShoppingCart", action = "Index", alias = UrlParameter.Optional },
               namespaces: new[] { "BanHangOnline.Controllers" }
           );
            routes.MapRoute(
              name: "Checkout",
              url: "thanh-toan",
              defaults: new { controller = "ShoppingCart", action = "checkOut", alias = UrlParameter.Optional },
              namespaces: new[] { "BanHangOnline.Controllers" }
          );
            routes.MapRoute(
              name: "DetailProduct",
              url: "chi-tiet/{alias}-p{id}",
              defaults: new { controller = "Product", action = "Detail", alias = UrlParameter.Optional },
              namespaces: new[] { "BanHangOnline.Controllers" }
          );
            routes.MapRoute(
     name: "CategoryProduct",
     url: "danh-muc-san-pham/{alias}-{id}",
     defaults: new { controller = "Product", action = "ProductCategory", id = UrlParameter.Optional },
     namespaces: new[] { "BanHangOnline.Controllers" }
 );


            routes.MapRoute(
                   name: "Default",
                   url: "{controller}/{action}/{id}",
                   defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                   namespaces: new[] { "BanHangOnline.Controllers" }
               );

        }
    }
}
