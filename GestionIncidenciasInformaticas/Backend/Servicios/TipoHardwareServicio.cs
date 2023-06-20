using GestionIncidenciasInformaticas.Backend.Modelo;
using System.Data.Entity;
using System.Linq;

namespace GestionIncidenciasInformaticas.Backend.Servicios
{
    class TipoHardwareServicio : ServicioGenerico<tipo_hardware>
    {
        private DbContext contexto;

        /*
         * Constructor
         */
        public TipoHardwareServicio(DbContext context) : base(context)
        {
            contexto = context;
        }

        //Este metodo sirve para saber si dado el id del tipohardware,se ha usado en alguna incidencia
        public bool esUsadoEnIncidencia(int tipoHardwareId)
        {
            bool tipoHardwareExists = contexto.Set<incidencia>().Any(incidencia => incidencia.hardware.tipo_hardware_id == tipoHardwareId);
            return tipoHardwareExists;
        }
    }
}
