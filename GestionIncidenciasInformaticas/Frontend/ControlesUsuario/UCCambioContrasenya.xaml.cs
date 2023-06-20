using GestionIncidenciasInformaticas.Backend.Modelo;
using GestionIncidenciasInformaticas.Backend.Servicios;
using NLog;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GestionIncidenciasInformaticas.Frontend.ControlesUsuario
{
    /// <summary>
    /// Interaction logic for UCCambioContrasenya.xaml
    /// </summary>
    public partial class UCCambioContrasenya : UserControl
    {

        private gestion_incidenciasEntities gesEnt;
        private profesor profesor;
        private ProfesorServicio profServ;
        private static Logger log = LogManager.GetCurrentClassLogger();


        public UCCambioContrasenya(gestion_incidenciasEntities gesEnt, profesor profesor)
        {
            InitializeComponent();

            this.gesEnt = gesEnt;
            this.profesor = profesor;
            profServ = new ProfesorServicio(gesEnt);
        }

        private async void btnCambioContrasenya_Click(object sender, RoutedEventArgs e)
        {
            ErrorGeneral.Visibility = Visibility.Hidden;
            txtBlockErrorGeneral.Visibility = Visibility.Hidden;

            //Verificamos si ambas txtBoxs estan rellenadas y avisaremos si fuera el caso de que alguna esté vacía
            if (string.IsNullOrEmpty(txtBoxPrimeraContrasenya.Password) || string.IsNullOrEmpty(txtBoxSegundaContrasenya.Password))
            {
                ErrorGeneral.Visibility = Visibility.Visible;
                txtBlockErrorGeneral.Text = "Ambas contraseñas deben de estar rellenadas";
                txtBlockErrorGeneral.Visibility = Visibility.Visible;

            }
            else if (txtBoxPrimeraContrasenya.Password != txtBoxSegundaContrasenya.Password)
            {
                ErrorGeneral.Visibility = Visibility.Visible;
                txtBlockErrorGeneral.Text = "Ambas contraseñas deben de coincidir";
                txtBlockErrorGeneral.Visibility = Visibility.Visible;
            }
            else if (txtBoxPrimeraContrasenya.Password == txtBoxSegundaContrasenya.Password)
            {
                profesor.contraseña = txtBoxSegundaContrasenya.Password.ToString();
                //Borramos la incidencia de la base de datos
                profServ.edit(profesor);
                try
                {
                    profServ.save();
                    await espera(2);
                    MessageBox.Show("Contraseña cambiada correctamente",
                "Conexión a base de datos", MessageBoxButton.OK, MessageBoxImage.None);

                }
                catch (Exception ex)
                {
                    log.Info("MODIFICANDO UN OBJETO PROFESOR ...\n");
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
