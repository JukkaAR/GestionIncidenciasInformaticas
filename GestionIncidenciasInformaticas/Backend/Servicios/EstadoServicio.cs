using GestionIncidenciasInformaticas.Backend.Modelo;
using System.Data.Entity;

namespace GestionIncidenciasInformaticas.Backend.Servicios
{
    class EstadoServicio : ServicioGenerico<estados>
    {
        private DbContext contexto;

        /*
         * Constructor
         */
        public EstadoServicio(DbContext context) : base(context)
        {
            contexto = context;
        }

    }
}