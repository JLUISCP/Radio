using Radio.MODELO.DAO;
using Radio.MODELO.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Radio.VISTAS
{
    /// <summary>
    /// Lógica de interacción para GUI_VisualizarPatrones.xaml
    /// </summary>
    public partial class GUI_VisualizarPatrones : Window
    {
        List<Patron> patrones;

        public GUI_VisualizarPatrones()
        {
            patrones = new List<Patron>();

            InitializeComponent();
            cargarListaPatrones();
        }

        private void btnRegistarPatron(object sender, RoutedEventArgs e)
        {
            GUI_RegistrarModificarPatron registrarModificarPatron = new GUI_RegistrarModificarPatron("nuevo", null);
            registrarModificarPatron.ShowDialog();
        }


        public void cargarListaPatrones()
        {
            patrones = DAOPatron.getPatrones();
            dg_Patrones.ItemsSource = patrones;
        }

        private void btnVisualizarPatron(object sender, RoutedEventArgs e)
        {
            int patronSeleccionado = dg_Patrones.SelectedIndex;
            if (patronSeleccionado >= 0)
            {
                Patron programaVisualizar = patrones[patronSeleccionado];

                GUI_RegistrarModificarPatron registrarModificarPatron = new GUI_RegistrarModificarPatron("visualizar", programaVisualizar);
                registrarModificarPatron.ShowDialog();

            }
            else
            {
                MessageBox.Show("Para Visualizar un patron tiene que seleccionarlo", "Error al Visualizar");
            }
        }

        private void btnModificarPatron(object sender, RoutedEventArgs e)
        {
            int patronSeleccionado = dg_Patrones.SelectedIndex;
            if (patronSeleccionado >= 0)
            {
                Patron programaModificar = patrones[patronSeleccionado];

                GUI_RegistrarModificarPatron registrarModificarPatron = new GUI_RegistrarModificarPatron("modificar", programaModificar);
                registrarModificarPatron.ShowDialog();
            }
            else
            {
                MessageBox.Show("Para Modificar un patron tiene que seleccionarlo", "Error al modificar");
            }
        }

        private void btnEliminarPatron(object sender, RoutedEventArgs e)
        {
            int patronSeleccionada = dg_Patrones.SelectedIndex;
            if (patronSeleccionada >= 0)
            {
                Patron patronEliminar = patrones[patronSeleccionada];
                MessageBoxResult resultado = MessageBox.Show("Seguro de eliminar el patron: " + patronEliminar.NombrePatron, "Confirmar accion", MessageBoxButton.OKCancel);
                if (resultado == MessageBoxResult.OK)
                {
                    int respuestaConsulta = DAOPatron.eliminarPatron(patronEliminar.IdPatron);

                    if (respuestaConsulta > 0)
                    {
                        MessageBox.Show("El patrón se elimino correctamente", "Patron Eliminado");
                        cargarListaPatrones();
                    }
                    else
                    {
                        MessageBox.Show("El patrón no se puedo eliminar. intente más tarde", "Error al Eliminar");
                    }
                }
            }
            else
            {
                MessageBox.Show("Para eliminar un patron tiene que seleccionarlo", "Error al Eliminar");
            }
        }
    }
}
