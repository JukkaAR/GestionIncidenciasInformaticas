using GestionIncidenciasInformaticas.Backend.Modelo;
using GestionIncidenciasInformaticas.Backend.Servicios;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GestionIncidenciasInformaticas.Frontend.Dialogos
{
    /// <summary>
    /// Interaction logic for DialogoMVCAddProfesor.xaml
    /// </summary>
    public partial class DialogoMVCAddProfesor : MetroWindow
    {
        private gestion_incidenciasEntities gesEnt;
        private ProfesorServicio profServ;
        private profesor profesorNuevo;
        private static Logger log = LogManager.GetCurrentClassLogger();

        private Brush borderOriginal;
        private Brush colorOriginal;

        public DialogoMVCAddProfesor(gestion_incidenciasEntities gesEnt)
        {
            InitializeComponent();
            this.gesEnt = gesEnt;

            borderOriginal = txtBoxDNI.BorderBrush;
            colorOriginal = txtBoxDNI.Foreground;

            profServ = new ProfesorServicio(gesEnt);
            profesorNuevo = new profesor();

            comboDepartamento.ItemsSource = new DepartamentoServicio(gesEnt).getAll().ToList();
            comboRol.ItemsSource = new RolesServicio(gesEnt).getAll().ToList();

        }

        private void comboDepartamento_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            quitarError(comboDepartamento);
        }

        private void comboRol_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            quitarError(comboRol);
        }

        private async void BtnAñadir_Click(object sender, RoutedEventArgs e)
        {
            if (valida())
            {
                recogerDatos();
                profServ.add(profesorNuevo);
                try
                {
                    profServ.save();
                    await espera(2);
                    MessageBox.Show("Profesor añadido correctamente",
                "Conexión a base de datos", MessageBoxButton.OK, MessageBoxImage.None);
                    this.Close();
                }
                catch (Exception ex)
                {
                    log.Info("INSERTANDO UN OBJETO PROFESOR ...\n");
                    log.Error(ex.InnerException);
                    log.Error(ex.StackTrace);
                    await this.ShowMessageAsync("GESTION PROFESOR", "ERROR!! No se puede insertar en la base de datos");
                }
            }
        }
        private async Task espera(int secs)
        {
            await Task.Delay(secs * 1000);
        }

        private void recogerDatos()
        {
            profesorNuevo.dni = txtBoxDNI.Text;
            profesorNuevo.nombre = txtBoxNombre.Text;
            profesorNuevo.apellidos = txtBoxApellidos.Text;
            profesorNuevo.roles = (roles)comboRol.SelectedItem;
            profesorNuevo.departamento = (departamento)comboDepartamento.SelectedItem;
            profesorNuevo.email = txtBoxEmail.Text;
            profesorNuevo.contraseña = passBoxPassword.Password;

        }

        private Boolean valida()
        {
            Boolean correcto = true;
            if (comboDepartamento.SelectedItem == null)
            {
                correcto = false;
                resaltarError(comboDepartamento, "El campo no puede estar vacío");
            }
            if (comboRol.SelectedItem == null)
            {
                correcto = false;
                resaltarError(comboRol, "El campo no puede estar vacío");
            }
            if (String.IsNullOrEmpty(txtBoxDNI.Text))
            {
                correcto = false;
                resaltarError(txtBoxDNI, "El campo no puede estar vacío");
            }
            if (String.IsNullOrEmpty(txtBoxApellidos.Text))
            {
                correcto = false;
                resaltarError(txtBoxApellidos, "El campo no puede estar vacío");
            }
            if (String.IsNullOrEmpty(txtBoxEmail.Text))
            {
                correcto = false;
                resaltarError(txtBoxEmail, "El campo no puede estar vacío");
            }
            if (String.IsNullOrEmpty(txtBoxNombre.Text))
            {
                correcto = false;
                resaltarError(txtBoxNombre, "El campo no puede estar vacío");
            }
            if (String.IsNullOrEmpty(passBoxPassword.Password))
            {
                correcto = false;
                resaltarError(passBoxPassword, "El campo no puede estar vacío");
            }
            List<profesor> profesorList = profServ.getAll().ToList();
            foreach (profesor profesor in profesorList)
            {
                if (profesor.dni == txtBoxDNI.Text)
                {
                    correcto = false;
                    resaltarError(txtBoxDNI, "Este DNI ya existe");
                }
            }
            return correcto;
        }

        private void resaltarError(Control c, String error)
        {
            c.BorderBrush = Brushes.Red;
            c.ToolTip = error;
        }
        private void quitarError(Control c)
        {
            c.BorderBrush = borderOriginal;
            c.ToolTip = null;
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
