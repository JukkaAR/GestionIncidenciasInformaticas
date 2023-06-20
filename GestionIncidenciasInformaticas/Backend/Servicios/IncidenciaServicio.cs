using GestionIncidenciasInformaticas.Backend.Modelo;
using System.Data.Entity;
using System.Linq;

namespace GestionIncidenciasInformaticas.Backend.Servicios
{
    class IncidenciaServicio : ServicioGenerico<incidencia>
    {
        private DbContext contexto;

        /*
         * Constructor
         */
        public IncidenciaServicio(DbContext context) : base(context)
        {
            contexto = context;
        }
        /*
         * Obtiene el último id de la tabla incidencia
         */
        public int getLastId()
        {
            incidencia inci = contexto.Set<incidencia>().OrderByDescending(i => i.num_incidencia).FirstOrDefault();

            if (inci == null)
            {
                return 0;
            }
            else
            {
                return inci.num_incidencia;
            }
        }

    }
}