using GestionIncidenciasInformaticas.Backend.Modelo;
using System.Data.Entity;

namespace GestionIncidenciasInformaticas.Backend.Servicios
{
    class DepartamentoServicio : ServicioGenerico<departamento>
    {
        private DbContext contexto;

        /*
         * Constructor
         */
        public DepartamentoServicio(DbContext context) : base(context)
        {
            contexto = context;
        }

    }
}
