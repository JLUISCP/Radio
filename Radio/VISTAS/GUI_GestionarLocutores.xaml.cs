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
    /// Lógica de interacción para GUI_GestionarLocutores.xaml
    /// </summary>
    public partial class GUI_GestionarLocutores : Window
    {
        List<Locutor> locutores;
        Locutor locutorSeleccionado = null;

        public GUI_GestionarLocutores()
        {
            InitializeComponent();
            locutores = new List<Locutor>();
            cargarLocutores();
        }

        private void cargarLocutores()
        {
            locutores = DAOLocutor.getLocutor();
            dg_Locutores.ItemsSource = locutores;
            dg_Locutores.AutoGenerateColumns = false;
        }

        private void btnCerrarVentana(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void dg_locutorSeleccionado(object sender, SelectionChangedEventArgs e)
        {
            locutorSeleccionado = (Locutor)dg_Locutores.SelectedItem;

            if (locutorSeleccionado != null)
            {
                tbModificarLocutor.Text = locutorSeleccionado.NombreLocutor;

            }

        }

        private void btnModificarLocutor(object sender, RoutedEventArgs e)
        {
            if (locutorSeleccionado != null)
            {
                if (!tbModificarLocutor.Text.Equals(locutorSeleccionado.NombreLocutor))
                {
                    MessageBoxResult opcionSeleccionada = MessageBox.Show("Esta seguro de actualiza el nombre '" + locutorSeleccionado.NombreLocutor + "' a '" + tbModificarLocutor.Text +
                                                                          "'", "Confirmacion Modificacion", MessageBoxButton.OKCancel);
                    if (opcionSeleccionada == MessageBoxResult.OK)
                    {
                        int resultado = DAOLocutor.modificarNombrelocutor(locutorSeleccionado.IdLocutor, tbModificarLocutor.Text);

                        if (resultado == 1)
                        {
                            cargarLocutores();
                            locutorSeleccionado = null;
                            tbModificarLocutor.Text = "";
                            tbBuscador.Text = "";
                            MessageBox.Show("Se modificio el locutor", "Locutor modificado");
                        }
                        else
                        {
                            MessageBox.Show("No se puedo modificar el locutor", "Locutor no modificado");
                        }

                    }
                }
            }
            else
            {
                MessageBox.Show("Se debe seleccionar un locutor y cambiar sus datos", "Locutor no seleccionado");
            }
        }

        private void btnEliminarLocutor(object sender, RoutedEventArgs e)
        {
            if (locutorSeleccionado != null)
            {
                MessageBoxResult opcionSeleccionada;

                if (locutorSeleccionado.NombrePrograma.Equals("Sin programa"))
                {
                    opcionSeleccionada = MessageBox.Show("Esta seguro de eliminar el locutor '" + locutorSeleccionado.NombreLocutor + "'", "Confirmacion Eliminacion", MessageBoxButton.OKCancel);
                }
                else
                {
                    opcionSeleccionada = MessageBox.Show("Esta seguro de eliminar el locutor '" + locutorSeleccionado.NombreLocutor + "' del programa '" + locutorSeleccionado.NombrePrograma + "'",
                                                         "Confirmacion Eliminacion", MessageBoxButton.OKCancel);
                }
                if (opcionSeleccionada == MessageBoxResult.OK)
                {
                    int resultado = DAOLocutor.eliminarNombrelocutor(locutorSeleccionado.IdLocutor);

                    if (resultado == 1)
                    {
                        cargarLocutores();
                        locutorSeleccionado = null;
                        tbModificarLocutor.Text = "";
                        tbBuscador.Text = "";
                        MessageBox.Show("Se elimino el locutor", "Locutor eliminado");
                    }
                    else
                    {
                        MessageBox.Show("No se puedo eliminar el locutor", "Locutor no eliminado");
                    }

                }
            }
            else
            {
                MessageBox.Show("Se debe seleccionar un locutor a eliminar", "Locutor no seleccionado");
            }
        }

        private void tbBuscarLocutor(object sender, TextChangedEventArgs e)
        {

            if (locutores.Count > 0)
            {
                var locutoresFiltrados = locutores.Where(Locutor => Locutor.NombreLocutor.ToUpper().Contains(tbBuscador.Text.ToUpper()));
                dg_Locutores.AutoGenerateColumns = false;
                dg_Locutores.ItemsSource = locutoresFiltrados;
                locutorSeleccionado = null;
                tbModificarLocutor.Text = "";
            }

        }
    }
}
