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
    /// Lógica de interacción para GUI_Cantantes.xaml
    /// </summary>
    public partial class GUI_Cantantes : Window
    {

        List<Cantantes> cantantes;
        Cantantes cantanteSeleccionado = null;


        public GUI_Cantantes()
        {
            InitializeComponent();
            cantantes = new List<Cantantes>();
            cargarCantantes();
            llenarCombo();
        }

        private void cargarCantantes()
        {
            cantantes = DAOCantantes.getCantante();
            dg_Cantantes.ItemsSource = cantantes;
            dg_Cantantes.AutoGenerateColumns = false;
            bt_eliminar.IsEnabled = false;
            bt_modificar.IsEnabled = false;
            bt_registrar.IsEnabled = true;
            bt_limpiarCampos.IsEnabled = false;
        }

        private void llenarCombo()
        {
            cb_tipo.Items.Add("GRUPO");
            cb_tipo.Items.Add("MASCULINO");
            cb_tipo.Items.Add("FEMENINO");
        }

        private void bt_registrar_Click(object sender, RoutedEventArgs e)
        {
            if (verificarCampos())
            {
                Cantantes cantante = new Cantantes();
                int campo = DAOCantantes.getID();
                campo = campo + 1;
                cantante.CNT_ID1 = campo;
                cantante.CNT_NOMBRE1 = tb_nombre.Text.ToUpper();
                cantante.CNT_TIPO1 = cb_tipo.SelectedItem.ToString();
                if ((bool)(cb_botonActivo.IsChecked))
                {
                    cantante.CNT_ESTATUS1 = "ACTIVO";
                }
                else
                {
                    cantante.CNT_ESTATUS1 = "BLOQUEADO";
                }
                MessageBoxResult opcionSeleccionada = MessageBox.Show("Estas seguro de registrar el cantante: " + tb_nombre.Text.ToUpper() + "  en tu catálogo?", " Confirmación", MessageBoxButton.OKCancel);
                if (opcionSeleccionada == MessageBoxResult.OK)
                {
                    String resultado = DAOCantantes.guardarCantante(cantante);
                    MessageBox.Show(resultado);
                    limpiarCampos();

                }
                else
                {
                    limpiarCampos();
                }
            }
        }

        private void dg_cantantesSeleccion(object sender, SelectionChangedEventArgs e)
        {
            bt_registrar.IsEnabled = false;
            bt_modificar.IsEnabled = true;
            bt_eliminar.IsEnabled = true;
            bt_limpiarCampos.IsEnabled = true;
            cantanteSeleccionado = (Cantantes)dg_Cantantes.SelectedItem;
            cargarInformacionCantante();
        }

        private void bt_modificar_Click(object sender, RoutedEventArgs e)
        {
            if (verificarCampos())
            {
                cantanteSeleccionado.CNT_NOMBRE1 = tb_nombre.Text.ToUpper();
                cantanteSeleccionado.CNT_TIPO1 = cb_tipo.SelectedItem.ToString();
                if ((bool)(cb_botonActivo.IsChecked))
                {
                    cantanteSeleccionado.CNT_ESTATUS1 = "ACTIVO";
                }
                else
                {
                    cantanteSeleccionado.CNT_ESTATUS1 = "BLOQUEADO";
                }

                MessageBoxResult opcionSeleccionada = MessageBox.Show("Estas seguro de actualizar el cantante: " + tb_nombre.Text.ToUpper() + "  ?", " Confirmación", MessageBoxButton.OKCancel);
                if (opcionSeleccionada == MessageBoxResult.OK)
                {
                    String resultado = DAOCantantes.modificarCantante(cantanteSeleccionado);
                    MessageBox.Show(resultado, "Operación ");
                    limpiarCampos();
                }
                else
                {
                    limpiarCampos();
                }
            }

        }

        private void bt_eliminar_Click(object sender, RoutedEventArgs e)
        {
            if (cantanteSeleccionado == null)
            {
                MessageBox.Show("Para eliminar un cantante, debe de seleccionarlo", "Sin seleccion");
            }
            else
            {
                MessageBoxResult opcionSeleccionada = MessageBox.Show("Estas seguro de eliminar al cantante: " + cantanteSeleccionado.CNT_NOMBRE1 + " ?", "Confirmacion eliminacion", MessageBoxButton.OKCancel);
                if (opcionSeleccionada == MessageBoxResult.OK)
                {
                    String resultado = DAOCantantes.eliminarCantante(cantanteSeleccionado.CNT_ID1);
                    MessageBox.Show(resultado, "Operación");
                    limpiarCampos();
                }
                else
                {
                    limpiarCampos();
                }
            }
        }

        private void cargarInformacionCantante()
        {
            if (cantanteSeleccionado != null)
            {
                tb_nombre.Text = cantanteSeleccionado.CNT_NOMBRE1;
                cb_tipo.SelectedItem = cantanteSeleccionado.CNT_TIPO1;
                dp_fechaAlta.SelectedDate = cantanteSeleccionado.CNT_FECHAALTA1;
                dp_fechaModificacion.SelectedDate = cantanteSeleccionado.CNT_FECHAMODIFICACION1;
                if (cantanteSeleccionado.CNT_ESTATUS1 == "ACTIVO")
                {
                    cb_botonActivo.IsChecked = true;
                }
                else
                {
                    cb_botonActivo.IsChecked = false;
                }
            }
        }

        private bool verificarCampos()
        {
            String estatus = "";
            if ((bool)(cb_botonActivo.IsChecked))
            {
                estatus = "ACTIVO";
            }
            else
            {
                estatus = "BLOQUEADO";
            }
            Boolean validar = false;
            if (cb_tipo.SelectedIndex == -1)
            {
                MessageBox.Show("Favor de seleccionar el tipo de cantante", "Campo Vacio");
                validar = false;
            }
            else
            {
                String tipoValor = cb_tipo.SelectedItem.ToString();
                int campo = DAOCantantes.camposDuplicados(tb_nombre.Text, tipoValor, estatus);
                if (campo == 1)
                {
                    MessageBox.Show("Cantante ya registrado, favor de verificar", "Cantante Duplicado");
                }
                else if (tb_nombre.Text == "")
                {
                    MessageBox.Show("Favor de llenar el campo requerido", "Campo Vacio");
                    validar = false;
                }
                else
                {
                    validar = true;
                }
            }
            return validar;
        }

        private void bt_nombreCambio(object sender, RoutedEventArgs e)
        {
            bt_limpiarCampos.IsEnabled = true;
        }

        private void cb_tipoCambio(object sender, SelectionChangedEventArgs e)
        {
            bt_limpiarCampos.IsEnabled = true;
        }

        private void limpiarCampos()
        {
            tb_nombre.Text = "";
            tb_buscar.Text = "";
            bt_limpiarCampos.IsEnabled = false;
            bt_modificar.IsEnabled = false;
            bt_eliminar.IsEnabled = false;
            cb_tipo.SelectedIndex = -1;
            dg_Cantantes.SelectedItem = null;
            cantanteSeleccionado = null;
            dp_fechaAlta.SelectedDate = null;
            dp_fechaModificacion.SelectedDate = null;
            cargarCantantes();
        }

        private void bt_limpiarCampos_Click(object sender, RoutedEventArgs e)
        {
            limpiarCampos();
            bt_eliminar.IsEnabled = false;
            bt_modificar.IsEnabled = false;
            bt_registrar.IsEnabled = true;
            dp_fechaAlta.SelectedDate = null;
            dp_fechaModificacion.SelectedDate = null;
        }

        private void bt_buscarCambio(object sender, TextChangedEventArgs e)
        {
            if (cantantes.Count > 0)
            {
                var cantantesFiltrados = cantantes.Where(Cantantes => Cantantes.CNT_NOMBRE1.ToUpper().Contains(tb_buscar.Text.ToUpper()));
                dg_Cantantes.AutoGenerateColumns = false;
                dg_Cantantes.ItemsSource = cantantesFiltrados;
            }
        }

    }
}
