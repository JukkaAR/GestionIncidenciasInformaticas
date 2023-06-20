using GestionIncidenciasInformaticas.Backend.Modelo;
using System;
using System.Data.Entity;
using System.Linq;

namespace GestionIncidenciasInformaticas.Backend.Servicios
{
    /*
     * Clase que contiene las reglas de negocio de la clase Articulo
     */
    class ProfesorServicio : ServicioGenerico<profesor>
    {
        private DbContext contexto;

        /*
         * Constructor
         */
        public ProfesorServicio(DbContext context) : base(context)
        {
            contexto = context;
        }

        /*
         * Se almacena el usuario que ha iniciado sesión
         */
        public profesor profLogin { get; set; }

        /*
         * Método que comprueba las credenciales del usuario en la base de datos
         */
        public Boolean login(String user, String pass)
        {
            Boolean correcto = true;
            try
            {
                profLogin = contexto.Set<profesor>().Where(p => p.dni == user).FirstOrDefault();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.StackTrace);
            }
            if (profLogin == null)
            {
                correcto = false;
            }
            else if (!profLogin.dni.Equals(user) || !profLogin.contraseña.Equals(pass))
            {
                correcto = false;
            }

            return correcto;
        }
        /*
         * Comprueba si en la base de datos existe un usuario con ese login
         * El login de un usuario debe de ser único
         */
        public Boolean usuarioUnico(string usu)
        {
            bool unico = true;
            if (contexto.Set<profesor>().Where(p => p.dni == usu).Count() > 0)
            {
                unico = false;
            }
            return unico;
        }

    }
}