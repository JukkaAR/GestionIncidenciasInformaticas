using GestionIncidenciasInformaticas.Backend.Modelo;
using GestionIncidenciasInformaticas.Backend.Servicios;
using GestionIncidenciasInformaticas.Frontend.ControlesUsuario;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace GestionIncidenciasInformaticas.Frontend.Dialogos
{
    /// <summary>
    /// Interaction logic for DialogoMVCAddIncidencia.xaml
    /// </summary>
    public partial class DialogoMVCAddIncidencia : MetroWindow
    {
        private gestion_incidenciasEntities gesEnt;
        private IncidenciaServicio inciServ;
        private DepartamentoServicio depServ;
        private incidencia incidenciaNueva;
        private incidencia incidenciaVieja;
        private HardwareServicio hardServ;
        private hardware hardwareNuevo;
        private profesor profesor;
        private profesor profesorResponsable;
        private Boolean tipoHardwareSeleccionado = false;
        private Boolean hayIncidenciaVieja = false;
        private int idInci = 0;
        private int idInciHW = 0;


        private static Logger log = LogManager.GetCurrentClassLogger();

        private System.Windows.Media.Brush borderOriginal;
        private System.Windows.Media.Brush colorOriginal;
        private BitmapImage imageBoxInfoAcomp;

        public DialogoMVCAddIncidencia(gestion_incidenciasEntities gesEnt, profesor profesor, incidencia incidenciaVieja)
        {
            InitializeComponent();

            borderOriginal = txtBoxDescripcion.BorderBrush;
            colorOriginal = txtBoxDescripcion.Foreground;

            this.gesEnt = gesEnt;
            this.profesor = profesor;

            inciServ = new IncidenciaServicio(gesEnt);
            hardServ = new HardwareServicio(gesEnt);
            depServ = new DepartamentoServicio(gesEnt);
            incidenciaNueva = new incidencia();
            hardwareNuevo = new hardware();


            comboDepartamento.ItemsSource = depServ.getAll().ToList();
            comboEstadoIncidencia.ItemsSource = new EstadoServicio(gesEnt).getAll().ToList();
            comboTipoIncidencia.ItemsSource = new List<string>(new string[] { "Hardware", "Software" });
            comboResponsable.ItemsSource = new ProfesorServicio(gesEnt).getAll().ToList();
            comboTipoHW.ItemsSource = new TipoHardwareServicio(gesEnt).getAll().ToList();


            // Dependiendo de si estamos recibiendo los datos para modificar la incidencia o crear una nueva,
            // rellenaremos los datos de una manera u otra

            if (incidenciaVieja == null)
            {
                dateFechaIncidencia.SelectedDate = DateTime.Now;
                dateFechaIntroduccion.SelectedDate = DateTime.Now;
                dateFechaResolucion.SelectedDate = DateTime.Now;

                foreach (estados estado in comboEstadoIncidencia.Items)
                {
                    string comunicada = estado.desc_estado.ToString();
                    if (comunicada.Equals("Comunicada"))
                    {
                        comboEstadoIncidencia.SelectedItem = estado;
                    }
                }
            }
            else
            {

                hayIncidenciaVieja = true;
                idInci = incidenciaVieja.num_incidencia;
                if (incidenciaVieja.hardware != null)
                {
                    idInciHW = (int)incidenciaVieja.hardware_id_incidencia_hw;
                }
                //Si editamos datos recibidos,cambiamos el título de la ventana y el contenido del botón para clarificar que editamos datos
                //También rellenamos datos

                txtBlockTitulo.Text = "Editar incidencia";
                BtnAñadir.Content = "Aplicar";
                TaskTitutlo.Title = "MODIFICAR INCIDENCIA";

                this.incidenciaVieja = incidenciaVieja;

                txtBoxDescripcion.Text = incidenciaVieja.descripcion_incidencia.ToString();

                //Precargamos imagen si es que la incidencia contenía una para mostrarla en miniatura
                byte[] byteArray = incidenciaVieja.info_acompanyada;
                BitmapImage bitmapImage = LoadBitmapImage(byteArray);
                thumbnailImagen.Source = bitmapImage;
                if (thumbnailImagen.Source != null) BtnBorrarInfoExtra.Visibility = Visibility.Visible;

                txtBoxObservaciones.Text = incidenciaVieja.observaciones.Trim().ToString();
                txtBoxUbicacion.Text = incidenciaVieja.aula_ubicacion;
                txtBoxTiempoInvertido.Text = incidenciaVieja.tiempo_invertido.ToString();
                dateFechaIncidencia.SelectedDate = incidenciaVieja.fecha_incidencia;
                dateFechaIntroduccion.SelectedDate = incidenciaVieja.fecha_introduccion;
                if (incidenciaVieja.fecha_resolución == null) dateFechaResolucion.SelectedDate = DateTime.Now; else dateFechaResolucion.SelectedDate = incidenciaVieja.fecha_resolución;
                comboDepartamento.SelectedItem = incidenciaVieja.departamento;
                comboResponsable.SelectedItem = incidenciaVieja.profesor1;
                comboTipoIncidencia.SelectedItem = incidenciaVieja.tipo_incidencia;
                comboEstadoIncidencia.SelectedItem = incidenciaVieja.estados;

                if (comboTipoIncidencia.SelectedItem.ToString().Equals("Hardware"))
                {
                    txtBoxNumSerie.Text = incidenciaVieja.hardware.num_serie.ToString();
                    txtBoxModelo.Text = incidenciaVieja.hardware.modelo.ToString();
                    comboTipoHW.SelectedItem = incidenciaVieja.hardware.tipo_hardware;
                }
            }
        }
        private void comboTipoIncidencia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string seleccion = comboTipoIncidencia.SelectedItem.ToString();

            if (seleccion == "Hardware")
            {
                comboTipoHW.Visibility = Visibility.Visible;
                txtBoxNumSerie.Visibility = Visibility.Visible;
                txtBoxModelo.Visibility = Visibility.Visible;
                tipoHardwareSeleccionado = true;
            }
            else
            {
                comboTipoHW.Visibility = Visibility.Hidden;
                txtBoxNumSerie.Visibility = Visibility.Hidden;
                txtBoxModelo.Visibility = Visibility.Hidden;
                tipoHardwareSeleccionado = false;

            }
        }

        private async void BtnAñadir_Click(object sender, RoutedEventArgs e)
        {
            if (Valida())
            {
                recogerDatos();

                //Si existe una incidencia vieja,significa que modificaremos los datos de la siguiente manera,
                //si no es asi,procedemos a anyadir la nueva incidencia directamente
                if (hayIncidenciaVieja)
                {
                    if (tipoHardwareSeleccionado & idInciHW != 0)
                    {
                        hardServ.Update(idInciHW, hardwareNuevo); hardServ.save();
                    }
                    else if (tipoHardwareSeleccionado)
                    {
                        hardServ.add(hardwareNuevo); hardServ.save();
                    }
                    incidenciaVieja = incidenciaNueva;
                    inciServ.Update(idInci, incidenciaVieja);

                }
                else
                {
                    if (tipoHardwareSeleccionado) hardServ.add(hardwareNuevo); hardServ.save();
                    inciServ.add(incidenciaNueva);
                }

                try
                {
                    inciServ.save();
                    await espera(1);
                    //Aquí tendremos en cuenta si el usuario quiere imprimir un justificante o no
                    //por lo que guardamos el resultado de la seleccion en la messageBox
                    //Si selecciona SI en vez de NO,mostraremos el justificante
                    var resultadoSeleccion = new MessageBoxResult();
                    if (!hayIncidenciaVieja)
                    {
                        idInci = incidenciaNueva.num_incidencia;
                        resultadoSeleccion = MessageBox.Show("Incidencia añadida correctamente.\r\n¿Desea ver el justificante?",
"Conexión a base de datos", MessageBoxButton.YesNo, MessageBoxImage.None);
                    }
                    else
                    {
                        resultadoSeleccion = MessageBox.Show("Incidencia modificada correctamente.\r\n¿Desea ver el justificante?",
"Conexión a base de datos", MessageBoxButton.YesNo, MessageBoxImage.None);
                    }
                    //Abrimos justificante
                    if (resultadoSeleccion == MessageBoxResult.Yes) mostrarJustificante(); else this.Close();
                }
                catch (Exception ex)
                {
                    log.Info("INSERTANDO/MODIFICANDO UN OBJETO INCIDENCIA ...\n");
                    log.Error(ex.InnerException);
                    log.Error(ex.StackTrace);
                    await this.ShowMessageAsync("GESTION INCIDENCIA", ex.InnerException.ToString()/*"ERROR!! No se puede insertar en la base de datos"*/);
                }
            }
        }
        private async Task espera(int secs)
        {
            await Task.Delay(secs * 1000);
        }

        private void recogerDatos()
        {
            //Info de incidencia
            //Num Incidencia
            if (!hayIncidenciaVieja)
            {
                incidenciaNueva.num_incidencia = inciServ.getLastId() + 1;
            }
            else
            {
                incidenciaNueva.num_incidencia = idInci;
            }
            //Tipo incidencia (hardware o software)
            incidenciaNueva.tipo_incidencia = comboTipoIncidencia.SelectedItem.ToString();
            //Fecha de incidencia
            incidenciaNueva.fecha_incidencia = (DateTime)dateFechaIncidencia.SelectedDate;
            //Fecha de introducción
            incidenciaNueva.fecha_introduccion = (DateTime)dateFechaIntroduccion.SelectedDate;
            //Aula o ubicación
            incidenciaNueva.aula_ubicacion = txtBoxUbicacion.Text;
            //Descripción de incidencia
            incidenciaNueva.descripcion_incidencia = txtBoxDescripcion.Text;
            //Fecha de resolución
            if (dateFechaResolucion.IsEnabled) incidenciaNueva.fecha_resolución = dateFechaResolucion.SelectedDate;
            //Observaciones
            incidenciaNueva.observaciones = txtBoxObservaciones.Text;
            //Estado de incidencia
            incidenciaNueva.estados = (estados)comboEstadoIncidencia.SelectedItem;
            incidenciaNueva.estados_codigo_estado = incidenciaNueva.estados.codigo_estado;
            //Departamento de la incidencia
            incidenciaNueva.departamento = (departamento)comboDepartamento.SelectedItem;
            incidenciaNueva.departamento_codigo_dep = incidenciaNueva.departamento.codigo_dep;
            //Tiempo invertido
            if (txtBoxTiempoInvertido.Text.Equals(""))
            {
                incidenciaNueva.tiempo_invertido = null;
            }
            else
            {
                incidenciaNueva.tiempo_invertido = double.Parse(txtBoxTiempoInvertido.Text);
            }
            //Profesor responsable de resolver la incidencia
            profesorResponsable = (profesor)comboResponsable.SelectedItem;
            incidenciaNueva.dni_responsable = profesorResponsable.dni.ToString();
            // Info que acompaña la incidencia (imageBox)
            if (imageBoxInfoAcomp is BitmapImage bitmapImage && imageBoxInfoAcomp != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                    encoder.Save(memoryStream);

                    incidenciaNueva.info_acompanyada = memoryStream.ToArray();
                }
            }
            //Profesor que introduce la incidencia
            incidenciaNueva.profesor_dni = profesor.dni.ToString();
            //Info de hardware (si este ha sido introducido)
            if (tipoHardwareSeleccionado == true)
            {
                int hardwareid = 0;
                incidenciaNueva.hardware = new hardware();
                //Verificamos si la incidencia contiene un objeto hardware
                if (idInciHW == 0)
                {
                    //Si es el caso le asignamos el id
                    hardwareid = hardServ.getLastId() + 1;
                }
                else
                {
                    //Sino asignamos el valor de id que ya tenemos
                    hardwareid = idInciHW;
                }

                hardwareNuevo.id_incidencia_hw = hardwareid;
                hardwareNuevo.modelo = txtBoxModelo.Text;
                tipo_hardware tphw = (tipo_hardware)comboTipoHW.SelectedItem;
                hardwareNuevo.tipo_hardware_id = tphw.id;
                hardwareNuevo.num_serie = int.Parse(txtBoxNumSerie.Text);
                incidenciaNueva.hardware = hardwareNuevo;
                incidenciaNueva.hardware_id_incidencia_hw = hardwareid;
            }

        }

        private Boolean Valida()
        {

            Boolean correcto = true;

            if (comboTipoIncidencia.SelectedItem == null)
            {
                correcto = false;
                resaltarError(comboTipoIncidencia, "El campo no puede estar vacío");
            }
            else
            {
                quitarError(comboTipoIncidencia);
                if (comboTipoIncidencia.SelectedItem.Equals("Hardware")){
                    if(comboTipoHW.SelectedItem == null)
                    {
                        correcto = false;
                        resaltarError(comboTipoHW, "El campo no puede estar vacío");
                    }
                    else quitarError(comboTipoHW);

                    if (String.IsNullOrEmpty(txtBoxModelo.Text))
                    {
                        correcto = false;
                        resaltarError(txtBoxModelo, "El campo no puede estar vacío");
                    }
                    else quitarError(txtBoxModelo);

                    if (String.IsNullOrEmpty(txtBoxNumSerie.Text))
                    {
                        correcto = false;
                        resaltarError(txtBoxNumSerie, "El campo no puede estar vacío");
                    }
                    else quitarError(txtBoxNumSerie);

                }
            }

            if (dateFechaIncidencia.SelectedDate == null)
            {
                correcto = false;
                resaltarError(dateFechaIncidencia, "El campo no puede estar vacío");
            }
            else quitarError(dateFechaIncidencia);


            if (dateFechaIntroduccion.SelectedDate == null)
            {
                correcto = false;
                resaltarError(dateFechaIntroduccion, "El campo no puede estar vacío");
            }
            else quitarError(dateFechaIntroduccion);


            if (String.IsNullOrEmpty(txtBoxUbicacion.Text))
            {
                correcto = false;
                resaltarError(txtBoxUbicacion, "El campo no puede estar vacío");
            }
            else quitarError(txtBoxUbicacion);


            if (String.IsNullOrEmpty(txtBoxDescripcion.Text))
            {
                correcto = false;
                resaltarError(txtBoxDescripcion, "El campo no puede estar vacío");
            }
            else quitarError(txtBoxDescripcion);


            if (comboResponsable.SelectedItem == null)
            {
                correcto = false;
                resaltarError(comboResponsable, "El campo no puede estar vacío");
            }
            else quitarError(comboResponsable);

            if (comboEstadoIncidencia.SelectedItem == null)
            {
                correcto = false;
                resaltarError(comboEstadoIncidencia, "El campo no puede estar vacío");
            }
            else quitarError(comboEstadoIncidencia);

            if (comboDepartamento.SelectedItem == null)
            {
                correcto = false;
                resaltarError(comboDepartamento, "El campo no puede estar vacío");
            }
            else quitarError(comboDepartamento);
            return correcto;
        }
        //Este metodo aplica el UC de Crystal Report con el justificante en el mismo Dialogo de la incidencia
        private void mostrarJustificante()
        {
            TaskTitutlo.Width = 1200;
            UCJustificanteIncidencia uc = new UCJustificanteIncidencia(idInci);
            gridPrincipal.Children.Clear();
            gridPrincipal.RowDefinitions.Clear();
            gridPrincipal.ColumnDefinitions.Clear();
            gridPrincipal.Children.Add(uc);

        }
        private void resaltarError(Control c, String error)
        {
            c.BorderBrush = System.Windows.Media.Brushes.Red;
            c.ToolTip = error;
        }
        private void quitarError(Control c)
        {
            c.BorderBrush = borderOriginal;
            c.ToolTip = null;
        }
        //El siguiente metodo es para abrir un dialogo al presionar el botón de Buscar
        //Podremos seleccionar una imagen para así poderla subir a la bbdd

        private void BtnBuscarImagen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";

            bool respuesta = (bool)openFileDialog.ShowDialog();
            if (respuesta)
            {
                string filepath = openFileDialog.FileName;

                // Seleccionamos la imagen y hacemos set al objeto privado imageBoxInfoAcomp
                BitmapImage image = new BitmapImage(new Uri(filepath));
                imageBoxInfoAcomp = image;
                //Asignamos la imagen al thumbnail
                thumbnailImagen.Source = image;
                BtnBorrarInfoExtra.Visibility = Visibility.Visible;
            }
        }

        //Este método sirve a la hora de cargar la imagen desde la bbdd
        //Recibe los bytes[] para pasarlo a BitmapImage
        private static BitmapImage LoadBitmapImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        //Con este metodo del boton de borrar imagen
        //Borramos el thumbnail y en la incidencia ponemos el valor a nulo
        private void BtnBorrarInfoExtra_Click(object sender, RoutedEventArgs e)
        {
            thumbnailImagen.Source = null;
            imageBoxInfoAcomp = null;
            BtnBorrarInfoExtra.Visibility = Visibility.Hidden;
        }
        //Con el método del comboEstadoIncidencia,habilitamos los campos de resolución en caso de que
        //el usuario ponga que el estado de la incidencia es resuelta
        private void comboEstadoIncidencia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            estados estado = (estados)comboEstadoIncidencia.SelectedItem;
            if (estado.desc_estado.Equals("Solucionada"))
            {
                txtBoxObservaciones.IsEnabled = true;
                dateFechaResolucion.IsEnabled = true;
            }
            else
            {
                txtBoxObservaciones.IsEnabled = false;
                dateFechaResolucion.IsEnabled = false;
            }

        }

        private void TaskTitutlo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtBoxNumSerie_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Verifica que el texto escrito es un número
            if (!esNumerico(e.Text))
            {
                e.Handled = true; // Cancelamos input si no es número
            }
        }

        // Método que verifica si el valor es un número
        private static bool esNumerico(string text)
        {
            return int.TryParse(text, out _);
        }

        private void txtBoxTiempoInvertido_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Verifica que el texto escrito es un número a excepción de una coma
            if (!esNumerico(e.Text) && e.Text != ",")
            {
                e.Handled = true;
            }
        }
    }
}