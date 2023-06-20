using GestionIncidenciasInformaticas.Backend.Modelo;
using System.Data.Entity;


namespace GestionIncidenciasInformaticas.Backend.Servicios
{
    class RolesServicio : ServicioGenerico<roles>
    {
        private DbContext contexto;

        /*
         * Constructor
         */
        public RolesServicio(DbContext context) : base(context)
        {
            contexto = context;
        }



    }
}
