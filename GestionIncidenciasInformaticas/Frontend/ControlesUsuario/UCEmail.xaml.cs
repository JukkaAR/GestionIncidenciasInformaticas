using System.Diagnostics;
using System.Windows;

namespace GestionIncidenciasInformaticas.Frontend.ControlesUsuario
{
    /// <summary>
    /// Interaction logic for UCEmail.xaml
    /// </summary>
    public partial class UCEmail : System.Windows.Controls.UserControl
    {
        public UCEmail()
        {
            InitializeComponent();
        }

        private void btnEnviarCorreo_Click(object sender, RoutedEventArgs e)
        {

            string destinatario = txtPara.Text;
            string asunto = txtAsunto.Text;
            string mensaje = txtMensaje.Text;

            string mailtoLink = string.Format("mailto:{0}?subject={1}&body={2}", destinatario, asunto, mensaje);

            Process.Start(mailtoLink);

        }
    }
}
