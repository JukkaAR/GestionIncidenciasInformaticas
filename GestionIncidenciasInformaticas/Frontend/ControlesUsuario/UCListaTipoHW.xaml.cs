using GestionIncidenciasInformaticas.Backend.Modelo;
using GestionIncidenciasInformaticas.Backend.Servicios;
using GestionIncidenciasInformaticas.Frontend.Dialogos;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace GestionIncidenciasInformaticas.Frontend.ControlesUsuario
{
    /// <summary>
    /// Interaction logic for UCListaTipoHW.xaml
    /// </summary>
    public partial class UCListaTipoHW : UserControl
    {
        private gestion_incidenciasEntities gesEnt;
        private TipoHardwareServicio tpHWServ;
        private static Logger log = LogManager.GetCurrentClassLogger();

        public UCListaTipoHW(gestion_incidenciasEntities gesEnt)
        {
            InitializeComponent();
            this.gesEnt = gesEnt;
            tpHWServ = new TipoHardwareServicio(gesEnt);

            cargarListaTiposHW();

        }
        //Este método sirve para inicializar la lista en el user control
        public void cargarListaTiposHW()
        {

            //Cargamos todos los tipos de hardware
            List<tipo_hardware> listadoTipoHW = new List<tipo_hardware>();
            listadoTipoHW = tpHWServ.getAll().ToList();

            dgTipoHW.ItemsSource = listadoTipoHW;

        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            //En este caso, pasamos al constructor el objeto tipo_hardware para que esta se autorrellene
            DialogoMVCAddTipoHW diag = new DialogoMVCAddTipoHW(gesEnt, (tipo_hardware)dgTipoHW.CurrentItem);
            diag.ShowDialog();
            cargarListaTiposHW();
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            //Borramos el tipo de hardware de la base de datos
            tpHWServ.delete((tipo_hardware)dgTipoHW.SelectedItem);
            try
            {
                tpHWServ.save();
                MessageBox.Show("Tipo de hardware borrado correctamente",
            "Conexión a base de datos", MessageBoxButton.OK, MessageBoxImage.None);

                // Ejecutamos este método para recargar la lista de tipos de hardware
                cargarListaTiposHW();

            }
            catch (Exception ex)
            {
                log.Info("BORRANDO UN OBJETO TIPO HARDWARE ...\n");
                log.Error(ex.InnerException);
                log.Error(ex.StackTrace);
            }

        }

        private void BtnEliminarTipoHW_Click(object sender, RoutedEventArgs e)
        {
            //Verificamos si el tipo de hardware que decidimos borrar ha sido utilizado ya en alguna incidencia
            //Si es así,avisaremos de que no se puede borrar,por ser lo contrario procedemos a borrarla
            tipo_hardware tpHW = (tipo_hardware)dgTipoHW.SelectedItem;

            if (!tpHWServ.esUsadoEnIncidencia(tpHW.id))
            {
                //Borramos el tipo de hardware de la base de datos

                tpHWServ.delete(tpHW);
                try
                {
                    tpHWServ.save();
                    MessageBox.Show("Tipo de hardware borrado correctamente",
                "Conexión a base de datos", MessageBoxButton.OK, MessageBoxImage.None);

                    // Ejecutamos este método para recargar la lista de tipos de hardware
                    cargarListaTiposHW();

                }
                catch (Exception ex)
                {
                    log.Info("BORRANDO UN OBJETO TIPO HARDWARE ...\n");
                    log.Error(ex.InnerException);
                    log.Error(ex.StackTrace);
                }
            }
            else MessageBox.Show("Este tipo de hardware no se puede eliminar debido a su uso en una o varias incidencias.",
             "Conexión a base de datos", MessageBoxButton.OK, MessageBoxImage.None);

        }
    }
}
