using GestionIncidenciasInformaticas.Backend.Modelo;
using GestionIncidenciasInformaticas.Backend.Servicios;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Data;
using System.Windows.Controls;

namespace GestionIncidenciasInformaticas.Frontend.ControlesUsuario
{
    /// <summary>
    /// Interaction logic for UCChartInciDep.xaml
    /// </summary>
    public partial class UCChartInciDep : UserControl
    {
        private gestion_incidenciasEntities gesEnt;

        public UCChartInciDep(gestion_incidenciasEntities gesEnt)
        {
            InitializeComponent();
            this.gesEnt = gesEnt;
            loadChartSQL();

        }
        private void loadChartSQL()
        {
            ServicioSQL serChart = new ServicioSQL();
            DataTable dt = serChart.getDatos("SELECT d.nombre_dep, COUNT(*) AS num_incidencia FROM incidencia i JOIN departamento d ON i.departamento_codigo_dep = d.codigo_dep GROUP BY d.nombre_dep");

            Func<ChartPoint, string> labelPoint = chartPoint => string.Format("{0}({1:P})", chartPoint.Y, chartPoint.Participation);
            ChartValues<double> cht_y_values = new ChartValues<double>();
            SeriesCollection series = new LiveCharts.SeriesCollection();
            foreach (DataRow dr in dt.Rows)
            {
                PieSeries ps = new PieSeries
                {
                    Title = "Departamento: " + dr[0],
                    Values = new ChartValues<double> { double.Parse(dr[1].ToString()) },
                    DataLabels = true,
                    LabelPoint = labelPoint
                };
                series.Add(ps);
            }
            lvIncidencias.Series = series;
        }
    }
}
