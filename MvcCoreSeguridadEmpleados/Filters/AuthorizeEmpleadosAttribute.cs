using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreSeguridadEmpleados.Filters
{
    public class AuthorizeEmpleadosAttribute : AuthorizeAttribute
        , IAuthorizationFilter
    {
        //Este método incercepta las peticiones que tengamos cuando decoramos un controller
        //o un IActionResult con nuestro atributo [AuthorizeEmpleados]
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //Cuando validamos a un usuario (en login) debemos almacenarlo dentro de la
            //aplicación, dicho usuario se encuentra dentro de context.HttpContext.User
            var user = context.HttpContext.User;
            //Si el usuario no se ha validado, qué hacemos?
            if(user.Identity.IsAuthenticated == false)
            {
                context.Result = this.GetRedirectRoute("Manage", "Login");

                ////Donde enviamos al usuario no validado
                //RouteValueDictionary rutalogin =
                //    new RouteValueDictionary(new
                //    {
                //        controller = "Manage", action = "Login"
                //    });
                ////Creamos una redirección
                //RedirectToRouteResult result = new RedirectToRouteResult(rutalogin);
                ////Devolvemos una respuesta
                //context.Result = result;
            }
            else
            {
                if(user.IsInRole("PRESIDENTE") == false
                && user.IsInRole("DIRECTOR") == false
                && user.IsInRole("ANALISTA") == false)
                {
                    //Devolvemos una respuesta
                    context.Result = this.GetRedirectRoute("Manage", "ErrorAcceso");
                }
            }
        }

        public RedirectToRouteResult GetRedirectRoute(string controller, string action)
        {
            RouteValueDictionary ruta =
                new RouteValueDictionary(new
                {
                    controller = controller,
                    action = action
                });
            //Creamos una redirección
            RedirectToRouteResult result = new RedirectToRouteResult(ruta);
            return result;
        }
    }
}
