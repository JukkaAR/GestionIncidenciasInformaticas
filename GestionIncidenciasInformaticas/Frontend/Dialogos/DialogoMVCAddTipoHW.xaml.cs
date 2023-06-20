using GestionIncidenciasInformaticas.Backend.Modelo;
using GestionIncidenciasInformaticas.Backend.Servicios;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using NLog;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GestionIncidenciasInformaticas.Frontend.Dialogos
{
    /// <summary>
    /// Interaction logic for DialogoMVCAddTipoHW.xaml
    /// </summary>
    public partial class DialogoMVCAddTipoHW : MetroWindow
    {
        private gestion_incidenciasEntities gesEnt;
        private TipoHardwareServicio tipoHwServ;
        private tipo_hardware tipoHwNuevo;
        private static Logger log = LogManager.GetCurrentClassLogger();
        private tipo_hardware tipo_HardwareViejo;
        private bool hay_tpHWViejo = false;

        private Brush borderOriginal;
        private Brush colorOriginal;
        public DialogoMVCAddTipoHW(gestion_incidenciasEntities gesEnt, tipo_hardware tipo_HardwareViejo)
        {
            InitializeComponent();
            this.gesEnt = gesEnt;
            this.tipo_HardwareViejo = tipo_HardwareViejo;

            borderOriginal = txtBoxTipoHW.BorderBrush;
            colorOriginal = txtBoxTipoHW.Foreground;

            tipoHwServ = new TipoHardwareServicio(gesEnt);
            tipoHwNuevo = new tipo_hardware();

            //Introducimos el dato para editar el tipo de hardware si es que se entregó dicho objeto

            if (tipo_HardwareViejo != null)
            {
                hay_tpHWViejo = true;
                txtBoxTipoHW.Text = tipo_HardwareViejo.nombre.ToString();

                //Tambien cambiamos el titulo y botones para dar a enteder que estamos editando
                BtnAñadir.Content = "Editar";
                txtBlockAnyadirTipoHW.Text = "Modificar tipo de Hardware";


            }

        }

        private async void BtnAñadir_Click(object sender, RoutedEventArgs e)
        {
            if (valida())
            {
                recogerDatos();
                if (!hay_tpHWViejo)
                {
                    tipoHwServ.add(tipoHwNuevo);
                }
                else
                {
                    int id = tipo_HardwareViejo.id;
                    tipoHwNuevo.id = id;
                    tipoHwServ.Update(id, tipoHwNuevo);
                }
                try
                {
                    tipoHwServ.save();
                    await espera(2);
                    if (!hay_tpHWViejo)
                    {
                        MessageBox.Show("Tipo de hardware añadido correctamente",
"Conexión a base de datos", MessageBoxButton.OK, MessageBoxImage.None);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Tipo de hardware editado correctamente",
"Conexión a base de datos", MessageBoxButton.OK, MessageBoxImage.None);
                        this.Close();
                    }

                }
                catch (Exception ex)
                {
                    log.Info("INSERTANDO UN OBJETO TIPO HW ...\n");
                    log.Error(ex.InnerException);
                    log.Error(ex.StackTrace);
                    await this.ShowMessageAsync("GESTION TIPO HW", "ERROR!! No se puede insertar en la base de datos");
                }
            }
        }

        private async Task espera(int secs)
        {
            await Task.Delay(secs * 1000);
        }

        private void recogerDatos()
        {
            tipoHwNuevo.nombre = txtBoxTipoHW.Text;

        }

        private Boolean valida()
        {
            Boolean correcto = true;

            if (String.IsNullOrEmpty(txtBoxTipoHW.Text))
            {
                correcto = false;
                resaltarError(txtBoxTipoHW, "El campo no puede estar vacío");
            }

            return correcto;
        }
        private void resaltarError(Control c, String error)
        {
            c.BorderBrush = Brushes.Red;
            c.ToolTip = error;
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void MetroWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
