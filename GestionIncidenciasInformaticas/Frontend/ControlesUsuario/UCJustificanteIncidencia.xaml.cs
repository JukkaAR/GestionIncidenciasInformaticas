using CrystalDecisions.CrystalReports.Engine;
using GestionIncidenciasInformaticas.Backend.Servicios;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GestionIncidenciasInformaticas.Frontend.ControlesUsuario
{
    /// <summary>
    /// Interaction logic for UCJustificanteIncidencia.xaml
    /// </summary>
    public partial class UCJustificanteIncidencia : UserControl
    {
        private int idInci;

        public UCJustificanteIncidencia(int idInci)
        {
            InitializeComponent();
            this.idInci = idInci;
            cargaInforme();
        }

        private void cargaInforme()
        {
            try
            {
                // Creamos un nuevo objeto Documento
                ReportDocument rd = new ReportDocument();
                // Indicamos la ruta del informe
                rd.Load("../../Informes/CrystalReports/CRJustificanteIncidencia.rpt");
                // Obtenemos el servicio asociado
                ServicioSQL sqlServ = new ServicioSQL();
                // Rellenamos la fuente de datos del informe con los datos
                // que obtenemos del servicio SQL mediante el método getDatos
                // al cual le pasamos la sentencia SQL
                rd.SetDataSource(sqlServ.getDatos("SELECT * FROM incidencia i " +
                    "JOIN estados e ON i.estados_codigo_estado = e.codigo_estado " +
                    "JOIN departamento d ON i.departamento_codigo_dep = codigo_dep " +
                    "LEFT JOIN profesor p1 on i.profesor_dni = p1.dni " +
                    "LEFT JOIN profesor p2 on i.dni_responsable = p2.dni " +
                    "LEFT JOIN hardware h ON i.hardware_id_incidencia_hw = h.id_incidencia_hw " +
                    "LEFT JOIN tipo_hardware t ON h.tipo_hardware_id = t.id WHERE i.num_incidencia =" + idInci));
                // Rellenamos los datos del informe
                SapViewerJustificanteIncidencia.ViewerCore.ReportSource = rd;

            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.StackTrace);
                System.Console.WriteLine(ex.InnerException);
            }
        }

        private void btnCerrarJustificante_Click(object sender, RoutedEventArgs e)
        {
            // Find the parent dialog and close it
            Window parentDialog = Window.GetWindow(this);
            parentDialog.Close();
        }
    }
}
