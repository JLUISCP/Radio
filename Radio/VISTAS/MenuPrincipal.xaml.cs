using System.Windows;

namespace Radio.VISTAS
{
    /// <summary>
    /// Lógica de interacción para MenuPrincipal.xaml
    /// </summary>
    public partial class MenuPrincipal : Window
    {
        public MenuPrincipal()
        {
            InitializeComponent();
        }

        private void btn_Programas_Click(object sender, RoutedEventArgs e)
        {
            GUI_VisualizarProgramas visualizarPrograma = new GUI_VisualizarProgramas();
            visualizarPrograma.ShowDialog();
        }
        private void btn_patron_Click(object sender, RoutedEventArgs e)
        {
            GUI_VisualizarPatrones visualizarPatrones = new GUI_VisualizarPatrones();
            visualizarPatrones.ShowDialog();
        }

            private void btn_Programacion_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_Canciones_Click(object sender, RoutedEventArgs e)
        {
            GUI_Canciones canciones = new GUI_Canciones();
            canciones.ShowDialog();
        }

        private void btn_Cantantes_Click(object sender, RoutedEventArgs e)
        {
            GUI_Cantantes cantantes = new GUI_Cantantes();
            cantantes.ShowDialog();
        }

        private void btn_Reportes_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
