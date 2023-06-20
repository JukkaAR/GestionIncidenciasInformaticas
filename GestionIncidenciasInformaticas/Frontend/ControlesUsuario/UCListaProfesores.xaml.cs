using GestionIncidenciasInformaticas.Backend.Modelo;
using GestionIncidenciasInformaticas.Backend.Servicios;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace GestionIncidenciasInformaticas.Frontend.ControlesUsuario
{
    /// <summary>
    /// Interaction logic for UCListaProfesores.xaml
    /// </summary>
    public partial class UCListaProfesores : UserControl
    {
        private gestion_incidenciasEntities gesEnt;
        private ProfesorServicio profServ;
        public UCListaProfesores(gestion_incidenciasEntities gesEnt)
        {
            InitializeComponent();
            this.gesEnt = gesEnt;
            profServ = new ProfesorServicio(gesEnt);

            cargarListaProfesores();

        }
        //Este método sirve para inicializar la lista en el user control
        public void cargarListaProfesores()
        {

            //Cargamos todos los tipos de hardware
            List<profesor> listadoProf = new List<profesor>();
            listadoProf = profServ.getAll().ToList();

            dgProf.ItemsSource = listadoProf;

        }
    }
}
