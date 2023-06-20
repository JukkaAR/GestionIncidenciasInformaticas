using GestionIncidenciasInformaticas.Backend.Modelo;
using GestionIncidenciasInformaticas.Backend.Servicios;
using GestionIncidenciasInformaticas.Frontend.Dialogos;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace GestionIncidenciasInformaticas.Frontend.ControlesUsuario
{
    /// <summary>
    /// Interaction logic for UCListaIncidencias.xaml
    /// </summary>
    public partial class UCListaIncidencias : System.Windows.Controls.UserControl
    {
        private gestion_incidenciasEntities gesEnt;
        private profesor profesor;
        private IncidenciaServicio inciServ;
        private HardwareServicio hardServ;
        private bool verTodasIncidencias;
        private static Logger log = LogManager.GetCurrentClassLogger();
        private List<incidencia> listadoIncidencias;
        public UCListaIncidencias(gestion_incidenciasEntities gesEnt, profesor profesor, bool verTodasIncidencias)
        {
            InitializeComponent();

            this.gesEnt = gesEnt;
            this.profesor = profesor;
            this.verTodasIncidencias = verTodasIncidencias;
            inciServ = new IncidenciaServicio(gesEnt);
            hardServ = new HardwareServicio(gesEnt);
            comboTipoHW.ItemsSource = new TipoHardwareServicio(gesEnt).getAll().ToList();


            cargarListaIncidencias();
        }

        //Este método sirve para inicializar la lista en el user control
        //Carga la lista y además comprueba si el usuario puede ver toda la lista o parte de ella
        public void cargarListaIncidencias()
        {
            //Cargamos todas las incidencias
            //inciServ = new IncidenciaServicio(gesEnt);
            listadoIncidencias = inciServ.getAll().ToList();
            List<incidencia> listaFiltrada = new List<incidencia>();

            // Verificamos si el usuario solo puede ver sus incidencias en las que participa
            // Si no puede ver todas, solo ve las que participa y esto se verificará siempre
            if (!verTodasIncidencias)
            {
                foreach (incidencia inci in listadoIncidencias)
                {
                    //Filtramos y mostramos las que coinciden con el DNI del responsable de solucionar el problema o del
                    //usuario que ha introducido la incidencia
                    if (profesor.dni.Equals(inci.profesor.dni) || profesor.dni.Equals(inci.profesor1.dni))
                    {
                        listaFiltrada.Add(inci);
                    }

                }

                // Construimos la tabla con la lista filtrada
                dgIncidencias.ItemsSource = listaFiltrada;
                listadoIncidencias = listaFiltrada;
            }
            else
            {
                //Si el usuario puede ver todas las incidencias,no aplicaremos el filtrado de dni
                dgIncidencias.ItemsSource = listadoIncidencias;
            }

        }

        //Este método sirve para filtrar la lista ya cargada
        //Filtra entre las dos fechas indicada y por tipo de hardware
        private void filtrarLista()
        {

            List<incidencia> listaFiltrada = new List<incidencia>();
            //Comprobaremos si los seleccionadores de fechas han sido usados para evitar errores al comparar o de repetición de incidencias dentro de un margen
            foreach (incidencia inci in listadoIncidencias)
            {

                if (DatePickerFiltroPrimeraFecha.SelectedDate != null & DatePickerFiltroSegundaFecha.SelectedDate != null)
                {
                    //Anyadiremos fechas posteriores a la primera fecha
                    //Y a la vez las anteriores a las segunda fecha
                    if (inci.fecha_incidencia.Date >= DatePickerFiltroPrimeraFecha.SelectedDate & inci.fecha_incidencia.Date <= DatePickerFiltroSegundaFecha.SelectedDate)
                    {
                        listaFiltrada.Add(inci);
                    }
                }
                else if (DatePickerFiltroPrimeraFecha.SelectedDate != null & DatePickerFiltroSegundaFecha.SelectedDate == null)
                {
                    //Anyadiremos fechas posteriores a la primera fecha
                    if (inci.fecha_incidencia.Date >= DatePickerFiltroPrimeraFecha.SelectedDate)
                    {
                        listaFiltrada.Add(inci);
                    }
                }
                else if (DatePickerFiltroPrimeraFecha.SelectedDate == null & DatePickerFiltroSegundaFecha.SelectedDate != null)
                {
                    //Anyadiremos fechas anteriores a la segunda fecha
                    if (inci.fecha_incidencia.Date <= DatePickerFiltroSegundaFecha.SelectedDate)
                    {
                        listaFiltrada.Add(inci);
                    }
                }
                else if (DatePickerFiltroPrimeraFecha.SelectedDate == null & DatePickerFiltroSegundaFecha.SelectedDate == null)
                {
                    //Si ninguna de las fechas está seleccionada,anyadiremos todas las incidencias estándar
                    listaFiltrada = listadoIncidencias;
                }

            }

            //Luego aparte podemos de esa lista filtrada,volverla a filtrar para obtener solo las incidencias del tipo hw seleccionado en el comboBox
            if (comboTipoHW.SelectedItem != null)
            {
                List<incidencia> listaFiltradaTipoHW = new List<incidencia>();

                foreach (incidencia inci in listaFiltrada)
                {
                    if (inci.hardware != null)
                    {
                        if (inci.hardware.tipo_hardware == comboTipoHW.SelectedItem)
                        {
                            listaFiltradaTipoHW.Add(inci);
                        }
                    }
                }
                listaFiltrada = listaFiltradaTipoHW;
            }

            dgIncidencias.ItemsSource = listaFiltrada;
        }
        //Este metodo es para editar el item seleccionado(incidencia)
        //Abrirá la ventana de anyadir incidencia pero modificada para dejar claro que la estamos modificando
        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            //En este caso, pasamos al constructor el objeto incidencia para que esta se autorrellene
            DialogoMVCAddIncidencia diag = new DialogoMVCAddIncidencia(gesEnt, profesor, (incidencia)dgIncidencias.SelectedItem);
            diag.Closing += DialogoMVCAddIncidenciaClosing;
            diag.ShowDialog();
        }

        private void DialogoMVCAddIncidenciaClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cargarListaIncidencias();
        }

        //Este metodo elimina la incidencia de la fila seleccionada
        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            //Borramos la incidencia de la base de datos
            //Sacamos la incidencia y su respectivo hardware (luego verificamos si tiene uno,si no tiene se copiará nulo)
            incidencia inciABorrar = (incidencia)dgIncidencias.SelectedItem;
            hardware hardwareABorrar = inciABorrar.hardware;
            //Borramos la incidencia y el hardware
            inciServ.delete(inciABorrar);
            if (hardwareABorrar != null) hardServ.delete(hardwareABorrar);

            try
            {
                inciServ.save();
                hardServ.save();
                System.Windows.MessageBox.Show("Incidencia borrada correctamente",
            "Conexión a base de datos", MessageBoxButton.OK, MessageBoxImage.None);

                // Ejecutamos este método para recargar la lista de incidencias
                cargarListaIncidencias();

            }
            catch (Exception ex)
            {
                log.Info("BORRANDO UN OBJETO INCIDENCIA ...\n");
                log.Error(ex.InnerException);
                log.Error(ex.StackTrace);
            }

        }
        //Este metodo resetea los parámetros de búsqueda y carga la lista de incidencias entera
        private void btnResetearLista_Click(object sender, RoutedEventArgs e)
        {
            //Volvemos a mostrar todas las incidencias
            cargarListaIncidencias();

            //Reseteamos los filtros a predeterminado, en este caso nulos
            comboTipoHW.SelectedItem = null;
            DatePickerFiltroPrimeraFecha.SelectedDate = null;
            DatePickerFiltroSegundaFecha.SelectedDate = null;
            //Deshabilitamos el botón de buscar
            btnBuscar.IsEnabled = false;
            //Deshabilitamos límites de fechas en los DatePicker
            DatePickerFiltroPrimeraFecha.BlackoutDates.Clear();
            DatePickerFiltroSegundaFecha.BlackoutDates.Clear();

        }
        //Este metodo es para aplicar los filtros de busqueda en la lista
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            filtrarLista();
        }
        //Este metodo es para ajustar los controles del filtro al seleccionar la fecha de inicio de búsqueda
        private void DatePickerFiltroPrimeraFecha_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DatePickerFiltroPrimeraFecha.SelectedDate != null)
            {
                // Quitamos límites anteriores del segundo DatePicker
                DatePickerFiltroSegundaFecha.BlackoutDates.Clear();

                // Sacamos el valor del primer DatePicker y en base al dato
                DateTime selectedDate = DatePickerFiltroPrimeraFecha.SelectedDate ?? DateTime.MinValue;
                if (selectedDate != DateTime.MinValue)
                {
                    DateTime startDate = DateTime.MinValue;
                    DateTime endDate = selectedDate.AddDays(-1);

                    DatePickerFiltroSegundaFecha.BlackoutDates.Add(new CalendarDateRange(startDate, endDate));
                }
                aparecerBtnBuscar();
            }
        }
        //Este metodo es para ajustar los controles del filtro al seleccionar la fecha de fin de búsqueda
        private void DatePickerFiltroSegundaFecha_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DatePickerFiltroSegundaFecha.SelectedDate != null)
            {
                // Quitamos límites anteriores del primer DatePicker
                DatePickerFiltroPrimeraFecha.BlackoutDates.Clear();
                //Hacemos el mismo proceso que en el método anterior pero a la inversa
                DateTime selectedDate = DatePickerFiltroSegundaFecha.SelectedDate ?? DateTime.MinValue;
                if (selectedDate != DateTime.MinValue)
                {
                    DateTime startDate = selectedDate.AddDays(1);
                    DateTime endDate = DateTime.MaxValue;

                    DatePickerFiltroPrimeraFecha.BlackoutDates.Add(new CalendarDateRange(startDate, endDate));
                }
                aparecerBtnBuscar();
            }
        }
        //Este metodo es para ajustar los controles del filtro al seleccionar el tipo de hardware
        private void comboTipoHW_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboTipoHW.SelectedItem != null)
            {
                aparecerBtnBuscar();
            }
        }
        //Este método es para hacer aparecer el botón de buscar cuando se cambie algún filtro de búsqueda
        private void aparecerBtnBuscar()
        {
            btnBuscar.IsEnabled = true;
        }
        private void BtnDownload_Click(object sender, RoutedEventArgs e)
        {
            //Solo ejecutaremos el método si la incidencia contiene una imagen
            System.Windows.Controls.Button button = (System.Windows.Controls.Button)sender;
            incidencia dataItem = (incidencia)button.DataContext;
            if (dataItem.info_acompanyada != null)
            {
                BitmapImage image = LoadBitmapImage(dataItem.info_acompanyada);

                // Usamos la siguiente clase para guardar la imagen con los formatos seleccionados en el filtro
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Image Files (*.png; *.jpg; *.jpeg) | *.png; *.jpg; *.jpeg";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;

                    // Guardamos la imagen en la ubicacion elegida
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        BitmapEncoder encoder = new PngBitmapEncoder(); // Change the encoder based on your image format
                        encoder.Frames.Add(BitmapFrame.Create(image));
                        encoder.Save(fileStream);
                    }

                    // Mensaje de guardado correcto
                    System.Windows.MessageBox.Show("Imagen guardada con éxito");
                }
            }
            else
            {
                // Mensaje de imagen no disponible,por lo que no permitimos la descarga
                System.Windows.MessageBox.Show("No hay imagen para descargar");
            }
        }
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
        //Cuando clicamos la imagen en la lista,abriremos una ventana para poder ver la imagen a un mayor tamanyo en vez de una miniatura
        private void Image_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            incidencia incidencia = (incidencia)dgIncidencias.SelectedItem;
            BitmapImage imagen = LoadBitmapImage(incidencia.info_acompanyada);
            PreviewImagen previewImagen = new PreviewImagen();
            previewImagen.imageControl.Source = imagen;
            previewImagen.Show();
        }
        private void ImageControl_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // Change the mouse cursor to a hand cursor
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Hand;
        }

        private void ImageControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // Restore the default mouse cursor
            Mouse.OverrideCursor = null;
        }
    }
}

