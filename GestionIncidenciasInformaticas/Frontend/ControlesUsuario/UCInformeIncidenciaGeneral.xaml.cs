using CrystalDecisions.CrystalReports.Engine;
using GestionIncidenciasInformaticas.Backend.Servicios;
using System;
using System.Windows.Controls;

namespace GestionIncidenciasInformaticas.Frontend.ControlesUsuario
{
    /// <summary>
    /// Interaction logic for UCInformeIncidenciaaGeneral.xaml
    /// </summary>
    public partial class UCInformeIncidenciaGeneral : UserControl
    {
        public UCInformeIncidenciaGeneral()
        {
            InitializeComponent();
            cargaInforme();
        }

        private void cargaInforme()
        {
            try
            {
                // Creamos un nuevo objeto Documento
                ReportDocument rd = new ReportDocument();
                // Indicamos la ruta del informe
                rd.Load("../../Informes/CrystalReports/CRIncidenciasGenerales.rpt");
                // Obtenemos el servicio asociado
                ServicioSQL sqlServ = new ServicioSQL();
                // Rellenamos la fuente de datos del informe con los datos
                // que obtenemos del servicio SQL mediante el método getDatos
                // al cual le pasamos la sentencia SQL
                rd.SetDataSource(sqlServ.getDatos("SELECT * FROM incidencia"));
                // Rellenamos los datos del informe
                SapViewerIncidenciaGeneral.ViewerCore.ReportSource = rd;

            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.StackTrace);
                System.Console.WriteLine(ex.InnerException);
            }
        }
    }
}

