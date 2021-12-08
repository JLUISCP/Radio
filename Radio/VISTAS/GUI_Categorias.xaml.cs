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
    /// Lógica de interacción para GUI_Categorias.xaml
    /// </summary>
    public partial class GUI_Categorias : Window
    {
        List<Categorias> categorias;
        Categorias categoriaSeleccionada = null;

        public GUI_Categorias()
        {
            InitializeComponent();
            categorias = new List<Categorias>();
            cargarCategorias();
        }

        public void cargarCategorias()
        {
            categorias = DAOCategoria.getCategoria();
            dg_Categorias.ItemsSource = categorias;
            dg_Categorias.AutoGenerateColumns = false;
            bt_modificarCategoria.IsEnabled = false;
            bt_registrarCategoria.IsEnabled = true;
            bt_Deshacer.IsEnabled = false;
        }

        private bool validarCampo()
        {
            Boolean validar = false;
            int Campo = DAOCategoria.campoDuplicado(tb_nombreCategoria.Text);
            if (tb_nombreCategoria.Text == "")
            {
                MessageBox.Show("Favor de llenar el campo requerido", "Campo Vacio");
                validar = false;
            }
            else if (Campo == 1)
            {
                MessageBox.Show("Categoría ya registrada, favor de verificar", "Campo Duplicado");
                tb_nombreCategoria.Text = "";
                dg_Categorias.SelectedItem = null;
                categoriaSeleccionada = null;
                validar = false;
                limpiarCampos();
            }
            else
            {
                validar = true;
            }

            return validar;
        }


        private void bt_registrarCategoria_Click(object sender, RoutedEventArgs e)
        {
            if (validarCampo() == true)
            {
                Categorias categoria = new Categorias();
                int campo = DAOCategoria.getID();
                campo = campo + 1;
                categoria.CAT_ID1 = campo;
                categoria.CAT_NOMBRE1 = tb_nombreCategoria.Text.ToUpper();
                MessageBoxResult opcionSeleccionada = MessageBox.Show("Estas seguro de registrar la categoría: " + tb_nombreCategoria.Text.ToUpper(), "Confirmación", MessageBoxButton.OKCancel);
                if (opcionSeleccionada == MessageBoxResult.OK)
                {
                    String resultado = DAOCategoria.guardarCategoria(categoria);
                    MessageBox.Show(resultado, "Operación.");
                    tb_nombreCategoria.Text = "";
                    bt_Deshacer.IsEnabled = false;
                    limpiarCampos();
                }
                else
                {
                    limpiarCampos();
                }

            }
        }

        private void dg_categoriaSeleccion(object sender, SelectionChangedEventArgs e)
        {
            bt_registrarCategoria.IsEnabled = false;
            bt_modificarCategoria.IsEnabled = true;
            categoriaSeleccionada = (Categorias)dg_Categorias.SelectedItem;
            cargarInformacionCategoria();
        }

        private void bt_modificarCategoria_Click(object sender, RoutedEventArgs e)
        {
            if (validarCampo())
            {
                categoriaSeleccionada.CAT_NOMBRE1 = tb_nombreCategoria.Text.ToUpper();
                MessageBoxResult opcionSeleccionada = MessageBox.Show("Estas seguro de actualizar la categoría seleccionada? ", "Confirmación", MessageBoxButton.OKCancel);
                if (opcionSeleccionada == MessageBoxResult.OK)
                {
                    String resultado = DAOCategoria.modificarCategoria(categoriaSeleccionada);
                    MessageBox.Show(resultado, "Operación");
                    bt_modificarCategoria.IsEnabled = false;
                    bt_Deshacer.IsEnabled = false;
                    limpiarCampos();
                }
                else
                {
                    limpiarCampos();
                }
            }
        }

        private void cargarInformacionCategoria()
        {
            if (categoriaSeleccionada != null)
            {
                tb_nombreCategoria.Text = categoriaSeleccionada.CAT_NOMBRE1;
            }
        }

        private void tb_cambio(object sender, RoutedEventArgs e)
        {
            bt_Deshacer.IsEnabled = true;
        }

        private void limpiarCampos()
        {

            tb_nombreCategoria.Text = "";
            tb_Buscar.Text = "";
            bt_Deshacer.IsEnabled = false;
            dg_Categorias.SelectedItem = null;
            categoriaSeleccionada = null;
            cargarCategorias();
        }

        private void bt_Deshacer_Click(object sender, RoutedEventArgs e)
        {
            limpiarCampos();
            bt_registrarCategoria.IsEnabled = true;
            bt_modificarCategoria.IsEnabled = false;
        }

        private void bt_Buscar(object sender, TextChangedEventArgs e)
        {
            if (categorias.Count > 0)
            {
                var categoriasFiltradas = categorias.Where(Categorias => Categorias.CAT_NOMBRE1.ToUpper().Contains(tb_Buscar.Text.ToUpper()));
                dg_Categorias.AutoGenerateColumns = false;
                dg_Categorias.ItemsSource = categoriasFiltradas;
            }
        }
    }
}
