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
    /// Lógica de interacción para GUI_Generos.xaml
    /// </summary>
    public partial class GUI_Generos : Window
    {
        List<Generos> generos;
        Generos generoSeleccionado = null;

        public GUI_Generos()
        {
            InitializeComponent();
            generos = new List<Generos>();
            cargarGeneros();
        }

        public void cargarGeneros()
        {
            generos = DAOGenero.getGenero();
            dg_Generos.ItemsSource = generos;
            dg_Generos.AutoGenerateColumns = false;
            bt_modificarGenero.IsEnabled = false;
            bt_registrarGenero.IsEnabled = true;
            bt_Deshacer.IsEnabled = false;
        }

        private bool validarCampo()
        {
            Boolean validar = false;
            int campo = DAOGenero.campoDuplicado(tb_nombreGenero.Text);
            if (tb_nombreGenero.Text == "")
            {
                MessageBox.Show("Favor de llenar el campo requerido", "Campo vacio");
                validar = false;
            }
            else if (campo == 1)
            {
                MessageBox.Show("Género ya registrado, favor de verificar", "Campo duplicado");
                tb_nombreGenero.Text = "";
                dg_Generos.SelectedItem = null;
                generoSeleccionado = null;
                limpiarCampos();
                validar = false;
            }
            else
            {
                validar = true;
            }

            return validar;
        }

        private void bt_registrarGenero_Click(object sender, RoutedEventArgs e)
        {
            if (validarCampo())
            {
                Generos genero = new Generos();
                int campo = DAOGenero.getId();
                campo = campo + 1;
                genero.GNR_ID1 = campo;
                genero.GNR_NOMBRE1 = tb_nombreGenero.Text.ToUpper();
                MessageBoxResult opcionSeleccionada = MessageBox.Show("Estas seguro de registrar la el género: " + tb_nombreGenero.Text.ToUpper(), "Confirmación", MessageBoxButton.OKCancel);
                if (opcionSeleccionada == MessageBoxResult.OK)
                {
                    String resultado = DAOGenero.guardarGenero(genero);
                    MessageBox.Show(resultado, "Operación.");
                    tb_nombreGenero.Text = "";
                    bt_Deshacer.IsEnabled = false;
                    limpiarCampos();
                }
                else
                {
                    limpiarCampos();
                }

            }
        }

        private void dg_generoSeleccion(object sender, SelectionChangedEventArgs e)
        {
            bt_registrarGenero.IsEnabled = false;
            bt_modificarGenero.IsEnabled = true;
            generoSeleccionado = (Generos)dg_Generos.SelectedItem;
            cargarInformacionGenero();
        }

        private void bt_modificarGenero_Click(object sender, RoutedEventArgs e)
        {
            if (validarCampo())
            {
                generoSeleccionado.GNR_NOMBRE1 = tb_nombreGenero.Text.ToUpper();
                MessageBoxResult opcionSeleccionada = MessageBox.Show("Estas seguro de actualizar el género selecionado? ", "Confirmación", MessageBoxButton.OKCancel);
                if (opcionSeleccionada == MessageBoxResult.OK)
                {
                    String resultado = DAOGenero.modificarGenero(generoSeleccionado);
                    MessageBox.Show(resultado, "Operación.");
                    bt_modificarGenero.IsEnabled = false;
                    bt_Deshacer.IsEnabled = false;
                    limpiarCampos();
                }
                else
                {
                    limpiarCampos();
                }
            }
        }

        private void cargarInformacionGenero()
        {
            if (generoSeleccionado != null)
            {
                tb_nombreGenero.Text = generoSeleccionado.GNR_NOMBRE1;
            }
        }

        private void tb_cambio(object sender, RoutedEventArgs e)
        {
            bt_Deshacer.IsEnabled = true;
        }

        private void limpiarCampos()
        {
            tb_nombreGenero.Text = "";
            tb_buscar.Text = "";
            bt_Deshacer.IsEnabled = false;
            dg_Generos.SelectedItem = null;
            generoSeleccionado = null;
            cargarGeneros();
        }

        private void bt_Deshacer_Click(object sender, RoutedEventArgs e)
        {
            limpiarCampos();
            bt_registrarGenero.IsEnabled = true;
            bt_modificarGenero.IsEnabled = false;
        }

        private void tb_Buscar(object sender, TextChangedEventArgs e)
        {
            if (generos.Count > 0)
            {
                var generosFiltrados = generos.Where(Generos => Generos.GNR_NOMBRE1.ToUpper().Contains(tb_buscar.Text.ToUpper()));
                dg_Generos.AutoGenerateColumns = false;
                dg_Generos.ItemsSource = generosFiltrados;
            }
        }

    }
}
