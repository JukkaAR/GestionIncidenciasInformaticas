using GestionIncidenciasInformaticas.Backend.Modelo;
using GestionIncidenciasInformaticas.Frontend;
using GestionIncidenciasInformaticas.Frontend.ControlesUsuario;
using GestionIncidenciasInformaticas.Frontend.Dialogos;
using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace GestionIncidenciasInformaticas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private gestion_incidenciasEntities gesEnt;
        private profesor profesor;
        private roles rol;
        private HashSet<permisos> listaPermisos;
        private UCListaIncidencias ucListaIncidencias;
        private UCListaTipoHW ucListaTipoHW;
        private UCListaProfesores ucListaProf;
        private bool verTodasIncidencias;

        public MainWindow(gestion_incidenciasEntities gesEnt, profesor profesor)
        {
            InitializeComponent();
            this.gesEnt = gesEnt;
            this.profesor = profesor;

            // Obtenemos el rol del profesor
            rol = profesor.roles;

            // Posteriormente obtenemos los permisos que tiene dicho rol
            listaPermisos = (HashSet<permisos>)rol.permisos;
            //Ejecutamos el método que se encarga de gestionar los permisos para variar
            //las funcionalidades de la aplicación
            gestionDePermisos();

            //Abrimos el listado de incidencias de manera predeterminada para que no se va tan vacía la aplicación
            ucListaIncidencias = new UCListaIncidencias(gesEnt, profesor, verTodasIncidencias);
            gridCentral.Children.Add(ucListaIncidencias);
        }

        private void BtnAddProfesor_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DialogoMVCAddProfesor diag = new DialogoMVCAddProfesor(gesEnt);
            diag.ShowDialog();
        }


        private void Desconectar_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void btnListaIncidencia_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucListaIncidencias = new UCListaIncidencias(gesEnt, profesor, verTodasIncidencias);
            gridCentral.Children.Clear();
            gridCentral.Children.Add(ucListaIncidencias);
        }
        //Este método gestiona los permisos y alterna el interfaz dependiendo de los respectivos permisos
        private void gestionDePermisos()
        {
            // Ajustando las funcionalidades de la aplicación dependiendo de los permisos del rol del usuario
            foreach (permisos p in listaPermisos)
            {
                switch (p.cod_permiso)
                {
                    // El permiso 1 es para añadir incidencias
                    case 1:
                        BtnAñadirIncidencia.IsEnabled = true;
                        break;
                    // El permiso 2 es para modificar o borrar cualquier incidencia (no solo en las que participa el usuario)
                    case 2:
                        verTodasIncidencias = true;
                        break;
                    // El permiso 3 es para crear borrar y modificar tipos de Hardware
                    case 3:
                        btnAnyadirTipoHW.IsEnabled = true;
                        btnListaTipoHW.IsEnabled = true;
                        break;
                    // 4 es para alta baja modificacion de roles y permisos
                    case 4:
                        btnEditarRolesPermisos.IsEnabled = true;
                        break;
                    // 5 es para operaciones de importacion
                    case 5:
                        //BtnImportacion.IsEnabled = true;
                        break;
                    // 6 es para informes de incidencias
                    case 6:
                        BtnVerInformeIncidencia.IsEnabled = true;
                        break;
                }

            }
        }


        private void BtnAñadirIncidencia_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DialogoMVCAddIncidencia diag = new DialogoMVCAddIncidencia(gesEnt, profesor, null);
            diag.Closing += DialogoMVCAddIncidenciaClosing;
            diag.ShowDialog();

        }

        private void DialogoMVCAddIncidenciaClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ucListaIncidencias != null) ucListaIncidencias.cargarListaIncidencias();
        }

        private void BtnCambiarPassword_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UCCambioContrasenya uc = new UCCambioContrasenya(gesEnt, profesor);
            gridCentral.Children.Clear();
            gridCentral.Children.Add(uc);
        }

        private void BtnAddProfesor_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DialogoMVCAddProfesor diag = new DialogoMVCAddProfesor(gesEnt);
            diag.Closing += DialogoMVCAddProfesorClosing;
            diag.ShowDialog();
        }
        private void DialogoMVCAddProfesorClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ucListaProf != null) ucListaProf.cargarListaProfesores();
        }

        private void btnEnviarEmail_Click(object sender, RoutedEventArgs e)
        {
            UCEmail uc = new UCEmail();
            gridCentral.Children.Clear();
            gridCentral.Children.Add(uc);
        }

        private void btnAnyadirTipoHW_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DialogoMVCAddTipoHW diag = new DialogoMVCAddTipoHW(gesEnt, null);
            diag.Closing += DialogoMVCAddTipoHWClosing;
            diag.ShowDialog();
        }

        private void DialogoMVCAddTipoHWClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ucListaTipoHW != null) ucListaTipoHW.cargarListaTiposHW();
        }

        private void btnListaTipoHW_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucListaTipoHW = new UCListaTipoHW(gesEnt);
            gridCentral.Children.Clear();
            gridCentral.Children.Add(ucListaTipoHW);
        }

        private void btnEditarRolesPermisos_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UCListaRolesPermisos uc = new UCListaRolesPermisos(gesEnt);
            gridCentral.Children.Clear();
            gridCentral.Children.Add(uc);
        }

        private void BtnVerInformeIncidencia_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UCInformeIncidenciaGeneral uc = new UCInformeIncidenciaGeneral();
            gridCentral.Children.Clear();
            gridCentral.Children.Add(uc);
        }

        private void BtnVerInformePorDpto_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UCChartInciDep uc = new UCChartInciDep(gesEnt);
            gridCentral.Children.Clear();
            gridCentral.Children.Add(uc);
        }

        private void BtnListaProf_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucListaProf = new UCListaProfesores(gesEnt);
            gridCentral.Children.Clear();
            gridCentral.Children.Add(ucListaProf);
        }


    }
}