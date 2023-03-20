using Microsoft.AspNetCore.Mvc.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Customer
{
    public class CustomRouteHandler : MvcRouteHandler
    {
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            string customerName = string.Empty;
            var controller = requestContext.RouteData.Values["controller"].ToString().Replace("-", "_");
            if (string.IsNullOrWhiteSpace(controller))
            {
                controller = "Home";
                // The initial call sometimes does not carry the controller name
            }
            if (!string.IsNullOrWhiteSpace(controller))
            {
                customerName = controller;
                var s = ""; //MyService.ResolveUrl(customerName);
                //YOUR BUSINESS LOGIC TO RESOLVE THE URL GOES HERE, IF YOU HAVE MULTIPLE VALUES IN THE URL USE STRING MANIPULATION AS YOU WANT.
                if (s == null) // CANNOT RESOLVE, LEAVE THE CONTROLLER NAME ALONE
                {
                    customerName = string.Empty;
                }
                else
                {
                    controller = "Home";
                    requestContext.RouteData.Values["name"] = customerName; //My Index method in HomeController is accepting a parameter called "name", that is the resolved customer name.
                }
            }
            requestContext.RouteData.Values["controller"] = controller; //Update the Controller Name
            var action = requestContext.RouteData.Values["action"].ToString().Replace("-", "_");
            if (string.IsNullOrWhiteSpace(action))
            {
                action = "Index"; //The initial call sometimes does not carry the action name
            }
            //Since I don't have much to resolve in action name, I just pass the value. But if you have multiple resolution to the action (methods to refer), I think you can figure out how to change this code.
            requestContext.RouteData.Values["action"] = action;
            //At this moment you should have a correct/resolved values to the route data values, otherwise you will get a error page.
            return base.GetHttpHandler(requestContext);
        }
    }
}