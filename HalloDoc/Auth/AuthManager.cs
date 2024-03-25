using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HalloDoc.Services.Interfaces;

namespace HalloDoc.Auth
{
   

    public class CustomAuthorize : Attribute, IAuthorizationFilter
    {
        private readonly string[] _roles;

        public CustomAuthorize(string role)
        {
            string str = role;

            // Split the string using a comma as the delimiter
            string[] array = str.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            this._roles = array;
        }


        public void OnAuthorization(AuthorizationFilterContext context)
        {
                var jwtService = context.HttpContext.RequestServices.GetService<IJwtToken>();
            if (jwtService == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Patient", action = "Login", }));
                return;
            }

            var request = context.HttpContext.Request;
            var token = request.Cookies["jwt"];

            if (token == null || !jwtService.ValidateJwtToken(token, out JwtSecurityToken jwtToken))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "AccessDenied", }));
                return;
            }

            var roleClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role);
            if (roleClaim == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "AccessDenied", }));
                return;
            }

            if (_roles == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "AccessDenied", }));
                return;
            }

            if (!_roles.Any(role => role == roleClaim.Value))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "AccessDenied", }));
                return;
            }
           
        }
    }
}


