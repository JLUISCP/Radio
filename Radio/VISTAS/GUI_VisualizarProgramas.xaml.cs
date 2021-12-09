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
    /// Lógica de interacción para GUI_VisualizarProgramas.xaml
    /// </summary>
    public partial class GUI_VisualizarProgramas : Window, InterfaceActualizarProgramas
    {
        List<Programa> programas;
        private Boolean seActualiza = false;

        public GUI_VisualizarProgramas()
        {
            InitializeComponent();
            programas = new List<Programa>();
            cargarProgramas();
        }

        private void btnVisualizarPrograma(object sender, RoutedEventArgs e)
        {
            int programaSeleccionada = dg_Programas.SelectedIndex;
            if (programaSeleccionada >= 0)
            {
                Programa programaVisualizar = new Programa();
                programaVisualizar = programas[programaSeleccionada];

                GUI_RegistrarModificarPrograma registrarModificarPrograma = new GUI_RegistrarModificarPrograma(programaVisualizar);
                registrarModificarPrograma.ShowDialog();
            }
            else
            {
                MessageBox.Show("Para Visualizar un programa tiene que seleccionarlo", "Error al Visualizar");
            }


        }

        private void btnRegistrarPrograma(object sender, RoutedEventArgs e)
        {
            GUI_RegistrarModificarPrograma registrarModificarPrograma = new GUI_RegistrarModificarPrograma(true, null);
            registrarModificarPrograma.Owner = this;
            registrarModificarPrograma.ShowDialog();

        }

        private void btnModificar(object sender, RoutedEventArgs e)
        {
            int programaSeleccionada = dg_Programas.SelectedIndex;
            if (programaSeleccionada >= 0)
            {
                Programa programaEditar = programas[programaSeleccionada];

                GUI_RegistrarModificarPrograma registrarModificarPrograma = new GUI_RegistrarModificarPrograma(false, programaEditar);
                registrarModificarPrograma.Owner = this;
                registrarModificarPrograma.ShowDialog();
            }
            else
            {
                MessageBox.Show("Para modificar un programa tiene que seleccionarlo", "Error al modificar");
            }
        }

        private void btnEliminarPrograma(object sender, RoutedEventArgs e)
        {
            int programaSeleccionada = dg_Programas.SelectedIndex;
            if (programaSeleccionada >= 0)
            {
                Programa programaEliminar = programas[programaSeleccionada];
                MessageBoxResult resultado = MessageBox.Show("Seguro de eliminar el programa: " + programaEliminar.NombrePrograma, "Confirmar accion", MessageBoxButton.OKCancel);
                if (resultado == MessageBoxResult.OK)
                {
                    int respuestaConsulta = DAOPrograma.eliminarPrograma(programaEliminar.IdPrograma);

                    if (respuestaConsulta > 0)
                    {
                        MessageBox.Show("El Programa se elimino correctamente", "Programa Eliminado");
                        cargarProgramas();
                    }
                    else
                    {
                        MessageBox.Show("El programa no se puedo eliminar. intente más tarde", "Error al Eliminar");
                    }
                }
            }
            else
            {
                MessageBox.Show("Para eliminar un programa tiene que seleccionarlo", "Error al Eliminar");
            }
        }

        private void btnBuscarPrograma(object sender, TextChangedEventArgs e)
        {

            if (programas.Count > 0)
            {
                var conductoresFiltrados = programas.Where(programa => programa.NombrePrograma.ToUpper().Contains(tbBuscador.Text.ToUpper()));
                dg_Programas.AutoGenerateColumns = false;
                dg_Programas.ItemsSource = conductoresFiltrados;
            }

        }

        public void cargarProgramas()
        {
            programas = DAOPrograma.getProgramas();
            dg_Programas.ItemsSource = programas;
            dg_Programas.AutoGenerateColumns = false;
        }

        public void actualizar(bool seActualiza)
        {
            if (seActualiza)
            {
                cargarProgramas();
            }
        }
    }
}
