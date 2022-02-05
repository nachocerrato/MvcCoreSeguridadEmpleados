using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MvcCoreSeguridadEmpleados.Models;
using MvcCoreSeguridadEmpleados.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcCoreSeguridadEmpleados.Controllers
{
    
    public class ManageController : Controller
    {
        private RepositoryEmpleados repo;

        public ManageController(RepositoryEmpleados repo)
        {
            this.repo = repo;
        }

        
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync
                (CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>
            Login(string apellido, string password)
        {
            //Buscamos al empleado
            Empleado empleado = this.repo.ExisteEmpleado(apellido, int.Parse(password));
            //Si nos devuelve un objeto Empleado, las credenciales son correctas
            if (empleado != null)
            {
                //Usuario existe
                //Cualquier usuario Claims está compuesto por una identidad (name y rol)
                //y un principal
                ClaimsIdentity identity =
                    new ClaimsIdentity
                    (
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        ClaimTypes.Name, ClaimTypes.Role);
                //Vamos a almacenar un par de datos
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier,
                    empleado.IdEmpleado.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Name,
                    empleado.Apellido));
                //Vamos a almacenar también el oficio del empleado para luego filtrar por roles
                identity.AddClaim(new Claim(ClaimTypes.Role, empleado.Oficio));
                //Creamos el usuario
                ClaimsPrincipal user = new ClaimsPrincipal(identity);
                //Introducimos al usuario dentro del sistema
                await HttpContext.SignInAsync
                    (CookieAuthenticationDefaults.AuthenticationScheme,
                    user, new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(15)
                    });
                //Dejamos entrar al usuario en el perfil
                return RedirectToAction("PerfilEmpleado", "Empleados");
            }
            else
            {
                //Mostramos el típico mensaje
                ViewData["MENSAJE"] = "Usuario/Password incorrectos";
                return View();
            }
        }

        public IActionResult ErrorAcceso()
        {
            return View();
        }
    }
}
