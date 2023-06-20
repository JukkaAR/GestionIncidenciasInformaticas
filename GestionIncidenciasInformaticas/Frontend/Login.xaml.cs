using GestionIncidenciasInformaticas.Backend.Modelo;
using GestionIncidenciasInformaticas.Backend.Servicios;
using MahApps.Metro.Controls;
using NLog;
using System;
using System.Windows;

namespace GestionIncidenciasInformaticas.Frontend
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : MetroWindow
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private gestion_incidenciasEntities gesEnt;
        private ProfesorServicio profServ;
        public Login()
        {
            InitializeComponent();
            if (!conectar())
            {
                MessageBox.Show("ERROR!!! NO SE PUEDE CONECTAR CON LA BASE DE DATOS",
                "CONEXION BASE DE DATOS", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
            profServ = new ProfesorServicio(gesEnt);
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtBoxUsuario.Text) || string.IsNullOrEmpty(PassWordBox.Password))
            {
                generalError.Visibility = Visibility.Visible;
                datosIncorrectosError.Visibility = Visibility.Hidden;
                ErrorUsuario.Visibility = Visibility.Hidden;
                ErrorPassword.Visibility = Visibility.Hidden;

            }
            else if (profServ.login(txtBoxUsuario.Text, PassWordBox.Password))
            {

                MainWindow ventanaPrincipal = new MainWindow(gesEnt, profServ.profLogin);
                ventanaPrincipal.Show();
                this.Close();
            }
            else
            {
                ErrorUsuario.Visibility = Visibility.Visible;
                ErrorPassword.Visibility = Visibility.Visible;
                generalError.Visibility = Visibility.Hidden;
                datosIncorrectosError.Visibility = Visibility.Visible;
            }
        }

        private Boolean conectar()
        {
            Boolean conectar = true;
            gesEnt = new gestion_incidenciasEntities();
            try
            {
                gesEnt.Database.Connection.Open();
            }
            catch (Exception ex)
            {
                conectar = false;
                log.Info("Conexión con base de datos fallida");
                log.Error(ex.InnerException);
                log.Error(ex.StackTrace);
            }
            return conectar;
        }
    }
}
