using GestionIncidenciasInformaticas.Backend.Modelo;
using System.Data.Entity;
using System.Linq;

namespace GestionIncidenciasInformaticas.Backend.Servicios
{
    class HardwareServicio : ServicioGenerico<hardware>
    {
        private DbContext contexto;

        /*
         * Constructor
         */
        public HardwareServicio(DbContext context) : base(context)
        {
            contexto = context;
        }
        public int getLastId()
        {
            hardware hw = contexto.Set<hardware>().OrderByDescending(h => h.id_incidencia_hw).FirstOrDefault();

            if (hw == null)
            {
                return 1;
            }
            else
            {
                return hw.id_incidencia_hw;
            }
        }
    }
}