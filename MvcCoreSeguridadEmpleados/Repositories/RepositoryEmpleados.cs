using MvcCoreSeguridadEmpleados.Data;
using MvcCoreSeguridadEmpleados.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreSeguridadEmpleados.Repositories
{
    public class RepositoryEmpleados
    {
        private EmpleadosContext context;

        public RepositoryEmpleados(EmpleadosContext context)
        {
            this.context = context;
        }

        public Empleado ExisteEmpleado(string apellido, int empno)
        {
            var consulta = from datos in this.context.Empleados
                           where datos.Apellido == apellido
                           && datos.IdEmpleado == empno
                           select datos;
            return consulta.FirstOrDefault();
        }

        public Empleado FindEmpleado(int idempleado)
        {
            return this.context.Empleados.SingleOrDefault(x => x.IdEmpleado == idempleado);
        }
    }
}
