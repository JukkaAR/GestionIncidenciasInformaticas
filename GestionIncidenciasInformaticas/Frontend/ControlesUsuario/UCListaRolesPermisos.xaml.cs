using GestionIncidenciasInformaticas.Backend;
using GestionIncidenciasInformaticas.Backend.Modelo;
using GestionIncidenciasInformaticas.Backend.Servicios;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GestionIncidenciasInformaticas.Frontend.ControlesUsuario
{
    /// <summary>
    /// Interaction logic for UCListaRolesPermisos.xaml
    /// </summary>
    public partial class UCListaRolesPermisos : UserControl
    {
        private gestion_incidenciasEntities gesEnt;
        private RolesServicio rolesServ;
        private PermisosServicio permServ;
        private List<permisos> listaPermisos;
        private List<roles> listaRoles;
        private static Logger log = LogManager.GetCurrentClassLogger();
        public UCListaRolesPermisos(gestion_incidenciasEntities gesEnt)
        {
            InitializeComponent();

            this.gesEnt = gesEnt;
            rolesServ = new RolesServicio(gesEnt);
            permServ = new PermisosServicio(gesEnt);
            listaPermisos = permServ.getAll().ToList();
            listaRoles = rolesServ.getAll().ToList();
            cargarDataGrid();

        }

        //Este método sirve para inicializar la lista en el user control
        private void cargarDataGrid()
        {
            //Generamos la lista con objeto RolesPermisos
            //Así en el mismo objeto tenemos el rol y sus permisos para facilitar los demás procesos
            List<RolesPermisos> listaRolesPermisos = new List<RolesPermisos>();

            //Ahora empezamos a procesar cada rol y sus permisos,formando una lista con en nuevo objeto RolesPermisos

            //dgRolesPermisos.ItemsSource = listaRoles;
            foreach (roles rol in listaRoles)
            {
                RolesPermisos nuevoRolPermiso = new RolesPermisos();
                //Rellenamos los datos correspondientes
                nuevoRolPermiso.rol = rol.rol.ToString();

                foreach (permisos p in rol.permisos.ToList())
                {

                    switch (p.cod_permiso)
                    {
                        case 1:
                            nuevoRolPermiso.puedeAnyadirIncidencias = true;
                            break;
                        case 2:
                            nuevoRolPermiso.puedeModificarIncidencias = true;
                            break;
                        case 3:
                            nuevoRolPermiso.puedeEditarTipoHW = true;
                            break;
                        case 4:
                            nuevoRolPermiso.puedeEditarRoles = true;
                            break;
                        case 5:
                            nuevoRolPermiso.puedeImportarExportar = true;
                            break;
                        case 6:
                            nuevoRolPermiso.puedeGenerarInformes = true;
                            break;
                    }

                }
                listaRolesPermisos.Add(nuevoRolPermiso);
                //preRellenarCheckboxes();
            }
            dgRolesPermisos.ItemsSource = listaRolesPermisos;

        }

        //Este método sirve para anyadir permisos al Rol de dicha fila
        //Al presionar en el checkbox,inmediatamente se hace el cambio en el rol
        private async void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (dgRolesPermisos.SelectedItem != null)
            {
                // Obtenemos el rolPermiso elegido
                RolesPermisos rolSeleccionado = (RolesPermisos)dgRolesPermisos.SelectedItem;

                //Buscamos el objeto Rol correspondiente
                roles rol = listaRoles[dgRolesPermisos.SelectedIndex];

                //Obtenemos los permisos para añadir el nuevo al objeto

                List<permisos> listaPermisosDelRol = (List<permisos>)rol.permisos.ToList();
                int posicionColumna = dgRolesPermisos.CurrentColumn.DisplayIndex - 1;

                if (!listaPermisosDelRol.Contains(listaPermisos[posicionColumna]))
                {
                    listaPermisosDelRol.Add(listaPermisos[posicionColumna]);
                    rol.permisos = listaPermisosDelRol;
                    rolesServ.edit(rol);
                }
                try
                {
                    rolesServ.save();
                    await espera(1);

                    MessageBox.Show("Permiso añadido correctamente",
                    "Conexión a base de datos", MessageBoxButton.OK, MessageBoxImage.None);
                }
                catch (Exception ex)
                {
                    log.Info("MODIFICANDO UN OBJETO ROL ...\n");
                    log.Error(ex.InnerException);
                    log.Error(ex.StackTrace);
                }
            }

        }

        //Este método sirve para retirar permisos al Rol de dicha fila
        //Al presionar en el checkbox,inmediatamente se hace el cambio en el rol
        private async void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (dgRolesPermisos.SelectedItem != null)
            {
                // Obtenemos el rolPermiso elegido
                RolesPermisos rolSeleccionado = (RolesPermisos)dgRolesPermisos.SelectedItem;

                //Buscamos el objeto Rol correspondiente
                roles rol = listaRoles[dgRolesPermisos.SelectedIndex];

                //Obtenemos los permisos para retirarselo al objeto
                List<permisos> listaPermisosDelRol = (List<permisos>)rol.permisos.ToList();
                int posicionColumna = dgRolesPermisos.CurrentColumn.DisplayIndex - 1;

                if (listaPermisosDelRol.Contains(listaPermisos[posicionColumna]))
                {
                    listaPermisosDelRol.Remove(listaPermisos[posicionColumna]);
                    rol.permisos = listaPermisosDelRol;
                    rolesServ.edit(rol);
                }
                try
                {
                    rolesServ.save();
                    await espera(1);

                    MessageBox.Show("Permiso retirado correctamente",
                    "Conexión a base de datos", MessageBoxButton.OK, MessageBoxImage.None);
                }
                catch (Exception ex)
                {
                    log.Info("MODIFICANDO UN OBJETO ROL ...\n");
                    log.Error(ex.InnerException);
                    log.Error(ex.StackTrace);
                }
            }

        }
        private async Task espera(int secs)
        {
            await Task.Delay(secs * 1000);
        }



    }
}
