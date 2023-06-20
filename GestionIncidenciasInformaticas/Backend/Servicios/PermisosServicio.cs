using GestionIncidenciasInformaticas.Backend.Modelo;
using System.Data.Entity;


namespace GestionIncidenciasInformaticas.Backend.Servicios
{
    class PermisosServicio : ServicioGenerico<permisos>
    {
        private DbContext contexto;

        /*
         * Constructor
         */
        public PermisosServicio(DbContext context) : base(context)
        {
            contexto = context;
        }

    }
}
