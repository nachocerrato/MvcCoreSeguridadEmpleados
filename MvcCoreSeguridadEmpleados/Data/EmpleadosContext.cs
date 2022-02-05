using Microsoft.EntityFrameworkCore;
using MvcCoreSeguridadEmpleados.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreSeguridadEmpleados.Data
{
    public class EmpleadosContext : DbContext
    {
        public EmpleadosContext
            (DbContextOptions<EmpleadosContext> options)
            : base(options) { }
        public DbSet<Empleado> Empleados { get; set; }
    }
}
