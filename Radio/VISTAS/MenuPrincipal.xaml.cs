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
            this.Close();
        }

        private void btn_Programacion_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_Canciones_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_Cantantes_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_Reportes_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
