﻿using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Auth

            // Split the string using a comma as the delimiter
            string[] array = str.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //var user = SessionUtils.GetLoggedInUser(context.HttpContext.Session);

            //if (user == null)
            //{
            //    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            //}
            //if (!string.IsNullOrEmpty(_role))
            //{
            //    if (!(user.role == _role))
            //    {
            //        context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            //    }
            //}
        }