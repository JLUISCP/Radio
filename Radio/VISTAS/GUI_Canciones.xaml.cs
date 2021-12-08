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
    /// Lógica de interacción para GUI_Canciones.xaml
    /// </summary>
    public partial class GUI_Canciones : Window
    {
        List<Canciones> canciones;
        List<Cantantes> cantantes;
        List<Categorias> categorias;
        List<Generos> generos;
        Canciones cancionSeleccionada = null;
        Int64 idCantante = 0;
        Int64 idCategoria = 0;
        Int64 idGenero = 0;
        Boolean isNuevo = true;
        String dias = "";

        public GUI_Canciones()
        {
            InitializeComponent();
            canciones = new List<Canciones>();
            cantantes = new List<Cantantes>();
            categorias = new List<Categorias>();
            generos = new List<Generos>();
            cargarCanciones();
            cargarListaElementos();
            cargarCombo();
        }

        private void cargarListaElementos()
        {
            cantantes = DAOCantantes.getCantante();
            categorias = DAOCategoria.getCategoria();
            generos = DAOGenero.getGenero();
        }

        private void cargarCanciones()
        {
            canciones = DAOCanciones.getCanciones();
            dg_canciones.ItemsSource = canciones;
            dg_canciones.AutoGenerateColumns = false;
            bt_eliminar.IsEnabled = false;
            bt_modificar.IsEnabled = false;
            bt_registrar.IsEnabled = true;
            bt_limpiarCampos.IsEnabled = false;
            tb_estatus.Text = "POR DEFINIR...";
        }

        private void cargarCombo()
        {
            cb_prioridad.Items.Add("1");
            cb_prioridad.Items.Add("2");
            cb_prioridad.Items.Add("3");
            cb_prioridad.Items.Add("4");
            cb_prioridad.Items.Add("5");
        }

        private void bt_registrar_Click(object sender, RoutedEventArgs e)
        {
            dias = "";
            if ((bool)(chb_lunes.IsChecked))
            {
                dias = dias + "1";
            }
            if ((bool)(chb_martes.IsChecked))
            {
                dias = dias + "2";
            }
            if ((bool)(chb_miercoles.IsChecked))
            {
                dias = dias + "3";
            }
            if ((bool)(chb_jueves.IsChecked))
            {
                dias = dias + "4";
            }
            if ((bool)(chb_viernes.IsChecked))
            {
                dias = dias + "5";
            }
            if ((bool)(chb_sabado.IsChecked))
            {
                dias = dias + "6";
            }
            if ((bool)(chb_domingo.IsChecked))
            {
                dias = dias + "7";
            }
            if (verificarCampos())
            {
                int campo = DAOCanciones.getID();
                campo = campo + 1;
                String titulo = tb_titulo.Text.ToUpper();
                String clave = tb_clave.Text.ToUpper();
                Int64 prioridad = Convert.ToInt64(cb_prioridad.SelectedItem.ToString());
                int estatus;
                String comentario;
                String observacion;
                if (tb_comentarios.Text == "")
                {
                    comentario = "Agrega un comentario aquí...";
                }
                else
                {
                    comentario = tb_comentarios.Text.ToUpper();
                }
                if (tb_observacion.Text == "")
                {
                    observacion = "Agrega una observación aquí...";
                }
                else
                {
                    observacion = tb_observacion.Text.ToUpper();
                }
                String ets = DAOCanciones.getEstatus((int)(idCantante));
                if (ets == "ACTIVO")
                {
                    estatus = 1;
                }
                else
                {
                    estatus = 0;
                }

                MessageBoxResult opcionSeleccionada = MessageBox.Show("Estas seguro de registrar la canción: " + titulo + "  en tu catálogo?", "Confirmación registro", MessageBoxButton.OKCancel);
                if (opcionSeleccionada == MessageBoxResult.OK)
                {
                    String resultado = DAOCanciones.guardarCancion(campo, titulo, clave, dias, prioridad, estatus, comentario,
                    observacion, idCantante, idCategoria, idGenero);
                    MessageBox.Show(resultado, "Operación");
                    limpiarCampos();
                    cargarCanciones();
                }
                else
                {
                    limpiarCampos();
                }
            }
        }

        private void dg_cancionesSeleccion(object sender, SelectionChangedEventArgs e)
        {
            bt_registrar.IsEnabled = false;
            bt_modificar.IsEnabled = true;
            bt_eliminar.IsEnabled = true;
            bt_limpiarCampos.IsEnabled = true;
            cancionSeleccionada = (Canciones)dg_canciones.SelectedItem;
            cargarInformacionCancion();
        }

        private void bt_modificar_Click(object sender, RoutedEventArgs e)
        {
            isNuevo = false;
            if (verificarCampos())
            {
                dias = "";
                if ((bool)(chb_lunes.IsChecked))
                {
                    dias = dias + "1";
                }
                if ((bool)(chb_martes.IsChecked))
                {
                    dias = dias + "2";
                }
                if ((bool)(chb_miercoles.IsChecked))
                {
                    dias = dias + "3";
                }
                if ((bool)(chb_jueves.IsChecked))
                {
                    dias = dias + "4";
                }
                if ((bool)(chb_viernes.IsChecked))
                {
                    dias = dias + "5";
                }
                if ((bool)(chb_sabado.IsChecked))
                {
                    dias = dias + "6";
                }
                if ((bool)(chb_domingo.IsChecked))
                {
                    dias = dias + "7";
                }

                String claveAct = tb_clave.Text.ToUpper();
                String tituloAct = tb_titulo.Text.ToUpper();
                Int64 prioridadAct = Convert.ToInt64(cb_prioridad.SelectedItem.ToString());
                String comentarioAct = tb_comentarios.Text.ToUpper();
                String observacionAct = tb_observacion.Text.ToUpper();
                MessageBoxResult opcionSeleccionada = MessageBox.Show("Estas seguro de modificar la canción: selecionada? ", "Confirmacion actualización", MessageBoxButton.OKCancel);
                if (opcionSeleccionada == MessageBoxResult.OK)
                {
                    String resultado = DAOCanciones.modificarCancion(cancionSeleccionada.CAN_ID1, tituloAct, claveAct, dias, prioridadAct,
                        comentarioAct, observacionAct, idCantante, idCategoria, idGenero);
                    MessageBox.Show(resultado, "Operación");
                    limpiarCampos();
                    cargarCanciones();
                }
                else
                {
                    limpiarCampos();
                }
            }
        }

        private void bt_Categorias_Click(object sender, RoutedEventArgs e)
        {
            GUI_Categorias categoria = new GUI_Categorias();
            categoria.ShowDialog();
        }

        private void bt_Generos_Click(object sender, RoutedEventArgs e)
        {
            GUI_Generos genero = new GUI_Generos();
            genero.ShowDialog();
        }

        private void bt_eliminar_Click(object sender, RoutedEventArgs e)
        {
            if (cancionSeleccionada == null)
            {
                MessageBox.Show("Para eliminar una canción, debe de seleccionarla", "Sin selección");
            }
            else
            {
                MessageBoxResult opcionSeleccionada = MessageBox.Show("Estas seguro de eliminar la canción: " + cancionSeleccionada.CAN_TITULO1 + " ?", "Confirmacion eliminacion", MessageBoxButton.OKCancel);
                if (opcionSeleccionada == MessageBoxResult.OK)
                {
                    String resultado = DAOCanciones.eliminarCancion(cancionSeleccionada.CAN_ID1);
                    MessageBox.Show(resultado, "Operación");
                    limpiarCampos();
                }
                else
                {
                    limpiarCampos();
                }
            }
        }

        private void cargarInformacionCancion()
        {
            if (cancionSeleccionada != null)
            {
                chb_domingo.IsChecked = false;
                chb_lunes.IsChecked = false;
                chb_martes.IsChecked = false;
                chb_miercoles.IsChecked = false;
                chb_jueves.IsChecked = false;
                chb_viernes.IsChecked = false;
                chb_sabado.IsChecked = false;

                dias = "";

                tb_clave.Text = cancionSeleccionada.CAN_CLAVE1;
                tb_titulo.Text = cancionSeleccionada.CAN_TITULO1;
                dp_fechaAlta.SelectedDate = cancionSeleccionada.CAN_FECHAALTA1;
                dp_fechaModificacion.SelectedDate = cancionSeleccionada.CAN_FECHAMODIFICACION1;
                tb_comentarios.Text = cancionSeleccionada.CAN_COMENTARIOS1;
                tb_observacion.Text = cancionSeleccionada.CAN_OBSERVACIONES1;
                tb_cantante.Text = cancionSeleccionada.CNT_NOMBRE1;
                idCantante = cancionSeleccionada.CNT_ID1;
                tb_categoria.Text = cancionSeleccionada.CAT_NOMBRE1;
                tb_genero.Text = cancionSeleccionada.GNR_NOMBRE1;
                idCantante = cancionSeleccionada.CNT_ID1;
                idCategoria = cancionSeleccionada.CAT_ID1;
                idGenero = cancionSeleccionada.GNR_ID1;
                String prioridad = Convert.ToString(cancionSeleccionada.CAN_PRIORIDAD1);
                cb_prioridad.SelectedItem = prioridad;
                isNuevo = false;
                if (cancionSeleccionada.CAN_ESTATUS1 == 1)
                {
                    tb_estatus.Text = "ACTIVA";
                }
                else
                {
                    tb_estatus.Text = "BLOQUEADA";
                }

                for (int i = 0; i < cancionSeleccionada.CAN_DIAS1.Length; i++)
                {
                    dias = cancionSeleccionada.CAN_DIAS1.Substring(i, 1);
                    switch (dias)
                    {
                        case "1":
                            chb_lunes.IsChecked = true;
                            break;
                        case "2":
                            chb_martes.IsChecked = true;
                            break;
                        case "3":
                            chb_miercoles.IsChecked = true;
                            break;
                        case "4":
                            chb_jueves.IsChecked = true;
                            break;
                        case "5":
                            chb_viernes.IsChecked = true;
                            break;
                        case "6":
                            chb_sabado.IsChecked = true;
                            break;
                        case "7":
                            chb_domingo.IsChecked = true;
                            break;
                        default:
                            break;

                    }
                }
            }
        }

        private bool verificarCampos()
        {
            Boolean validar = false;
            if (cb_prioridad.SelectedIndex == -1)
            {
                MessageBox.Show("Favor de seleccionar el tipo de prioridad de la canción.", "Campo Vacío");
                validar = false;
            }
            else
            {
                int campoClave = DAOCanciones.claveDuplicada(tb_clave.Text);
                int titulo = DAOCanciones.tituloDuplicado(tb_titulo.Text, idCantante);
                int cancionDuplicada = DAOCanciones.cancionDuplicada(tb_clave.Text, tb_titulo.Text, idCantante, idCategoria, idGenero);
                if (dias == "")
                {
                    MessageBox.Show("Favor de seleccionar al menos un día que se permita la canción", "Campo vacío");
                    validar = false;
                }
                else if (cancionDuplicada == 1)
                {
                    MessageBox.Show("Canción ya registrada. Favor de verificar...", "Canción Duplicada");
                    validar = false;
                }
                else if (titulo == 1 && isNuevo)
                {
                    MessageBox.Show("Título de canción repetida, favor de verificar el título de la cancion o " +
                        "el cantante seleccionado...", "Campos Duplicados");
                    validar = false;
                }
                else if (campoClave == 1 && isNuevo)
                {
                    MessageBox.Show("Clave de canción repetida, favor de verificar...", "Campo Duplicado");
                    validar = false;
                }
                else if (tb_clave.Text == "" || tb_titulo.Text == "" || tb_cantante.Text == "" || tb_categoria.Text == ""
                    || tb_genero.Text == "")
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

        private void limpiarCampos()
        {
            tb_clave.Text = "";
            tb_titulo.Text = "";
            tb_cantante.Text = "";
            tb_categoria.Text = "";
            tb_genero.Text = "";
            cb_prioridad.SelectedIndex = -1;
            chb_domingo.IsChecked = false;
            chb_lunes.IsChecked = false;
            chb_martes.IsChecked = false;
            chb_miercoles.IsChecked = false;
            chb_jueves.IsChecked = false;
            chb_viernes.IsChecked = false;
            chb_sabado.IsChecked = false;
            dp_fechaAlta.SelectedDate = null;
            dp_fechaModificacion.SelectedDate = null;
            tb_estatus.Text = "POR DEFINIR...";
            tb_comentarios.Text = "";
            tb_observacion.Text = "";
            dg_canciones.SelectedItem = null;
            cancionSeleccionada = null;
            tb_buscar.Text = "";
            idCantante = 0;
            idCategoria = 0;
            idGenero = 0;
            bt_limpiarCampos.IsEnabled = false;
            cancionSeleccionada = null;
            isNuevo = true;
            cargarCanciones(); ;
            cargarListaElementos();

        }

        private void bt_limpiarCampos_Click(object sender, RoutedEventArgs e)
        {
            limpiarCampos();
        }

        private void bt_buscarCambio(object sender, TextChangedEventArgs e)
        {
            if (canciones.Count > 0)
            {
                var cancionesFiltradas = canciones.Where(Canciones => Canciones.CAN_TITULO1.ToUpper().Contains(tb_buscar.Text.ToUpper()));
                dg_canciones.AutoGenerateColumns = false;
                dg_canciones.ItemsSource = cancionesFiltradas;
            }
        }

        private void tb_tituloCambio(object sender, RoutedEventArgs e)
        {
            bt_limpiarCampos.IsEnabled = true;
        }

        private void cb_prioridadCambio(object sender, SelectionChangedEventArgs e)
        {
            bt_limpiarCampos.IsEnabled = true;
        }

        private void tb_comentariosCambio(object sender, RoutedEventArgs e)
        {
            bt_limpiarCampos.IsEnabled = true;
        }

        private void tb_observacionCambio(object sender, RoutedEventArgs e)
        {
            bt_limpiarCampos.IsEnabled = true;
        }

        private void tb_claveCambio(object sender, RoutedEventArgs e)
        {
            bt_limpiarCampos.IsEnabled = true;
        }

        private void tb_cantante_tch(object sender, TextChangedEventArgs e)
        {
            String buscar = tb_buscar.Text.ToUpper();
            List<Cantantes> autolist = new List<Cantantes>();

            foreach (Cantantes item in cantantes)
            {
                if (item.ToString().ToUpper().StartsWith(buscar.ToUpper()) && item.ToString() != null && !item.ToString().Equals(""))
                {
                    autolist.Add(item);
                }
            }

            if (autolist.Count > 0)
            {
                lb_cantante.ItemsSource = autolist;
                lb_cantante.Visibility = Visibility.Visible;
            }

            if (autolist.Count == 0)
            {
                lb_cantante.Visibility = Visibility.Collapsed;
                lb_cantante.ItemsSource = null;
            }

            if (tb_cantante.Text == "")
            {
                lb_cantante.Visibility = Visibility.Collapsed;
                lb_cantante.ItemsSource = null;
            }
        }

        private void lb_cantante_seleccion(object sender, SelectionChangedEventArgs e)
        {
            if (lb_cantante.ItemsSource != null)
            {
                if (lb_cantante.SelectedIndex != -1)
                {
                    tb_cantante.Text = lb_cantante.SelectedIndex.ToString();
                    lb_cantante.Visibility = Visibility.Collapsed;
                    lb_cantante.ItemsSource = null;
                }
            }
        }

        private void tb_cantante_LF(object sender, RoutedEventArgs e)
        {
            String buscar = tb_cantante.Text.ToUpper();
            List<Cantantes> autolist = new List<Cantantes>();
            if (!tb_cantante.Equals(""))
            {
                foreach (Cantantes item in cantantes)
                {
                    if (item.ToString().ToUpper().StartsWith(buscar.ToUpper()) && item.ToString() != null && !item.ToString().Equals(""))
                    {
                        autolist.Add(item);
                    }
                }

                if (autolist.Count == 0)
                {
                    MessageBox.Show("Cantante no existente, favor de verificar", "Operación");
                    tb_cantante.Text = "";
                    MessageBox.Show(autolist[0].CNT_NOMBRE1);
                }
                else
                {
                    tb_cantante.Text = autolist[0].CNT_NOMBRE1;
                    idCantante = autolist[0].CNT_ID1;

                    cantantes = DAOCantantes.getCantante();
                }
            }
        }

        private void tb_canntante_SCH(object sender, RoutedEventArgs e)
        {
            bt_limpiarCampos.IsEnabled = true;
        }

        private void bt_categoria_tch(object sender, TextChangedEventArgs e)
        {
            bt_limpiarCampos.IsEnabled = true;
            String buscar = tb_categoria.Text.ToUpper();
            List<Categorias> autolist = new List<Categorias>();

            foreach (Categorias item in categorias)
            {
                if (item.ToString().ToUpper().StartsWith(buscar.ToUpper()) && item.ToString() != null && !item.ToString().Equals(""))
                {
                    autolist.Add(item);
                }
            }

            if (autolist.Count > 0)
            {
                lb_categorias.ItemsSource = autolist;
                lb_categorias.Visibility = Visibility.Visible;
            }
            if (autolist.Count == 0)
            {
                lb_categorias.Visibility = Visibility.Collapsed;
                lb_categorias.ItemsSource = null;
            }
            if (tb_categoria.Text == "")
            {
                lb_categorias.Visibility = Visibility.Collapsed;
                lb_categorias.ItemsSource = null;
            }
        }

        private void lb_categorias_SCH(object sender, SelectionChangedEventArgs e)
        {
            if (lb_categorias.ItemsSource != null)
            {
                if (lb_categorias.SelectedIndex != -1)
                {
                    tb_categoria.Text = lb_categorias.SelectedItem.ToString();
                    lb_categorias.Visibility = Visibility.Collapsed;
                    lb_categorias.ItemsSource = null;
                }
            }
        }

        private void bt_categoria_LF(object sender, RoutedEventArgs e)
        {
            String buscar = tb_categoria.Text.ToUpper();
            List<Categorias> autolist = new List<Categorias>();

            if (!buscar.Equals(""))
            {

                foreach (Categorias item in categorias)
                {
                    if (item.ToString().ToUpper().StartsWith(buscar.ToUpper()) && item.ToString() != null && !item.ToString().Equals(""))
                    {
                        autolist.Add(item);
                    }
                }
                if (autolist.Count == 0)
                {
                    MessageBoxResult opcionSeleccionada = MessageBox.Show("¿Desea registrar: '" + buscar + "' como categoría?", "Operación", MessageBoxButton.OKCancel);
                    if (opcionSeleccionada == MessageBoxResult.OK)
                    {
                        Categorias cat = new Categorias();
                        int campo = DAOCategoria.getID();
                        campo = campo + 1;
                        cat.CAT_ID1 = campo;
                        cat.CAT_NOMBRE1 = tb_categoria.Text.ToUpper();
                        String resultado = DAOCategoria.guardarCategoria(cat);
                        MessageBox.Show(resultado, "Operación");
                        idCategoria = campo;

                    }
                    else
                    {
                        tb_categoria.Text = "";
                    }

                    categorias = DAOCategoria.getCategoria();
                }
                else
                {
                    tb_categoria.Text = autolist[0].CAT_NOMBRE1;
                    idCategoria = autolist[0].CAT_ID1;
                    lb_categorias.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void tb_genero_tch(object sender, TextChangedEventArgs e)
        {
            bt_limpiarCampos.IsEnabled = true;
            String buscar = tb_genero.Text.ToUpper();
            List<Generos> autolist = new List<Generos>();

            foreach (Generos item in generos)
            {
                if (item.ToString().ToUpper().StartsWith(buscar.ToUpper()) && item.ToString() != null && !item.ToString().Equals(""))
                {
                    autolist.Add(item);
                }
            }

            if (autolist.Count > 0)
            {
                lb_genero.ItemsSource = autolist;
                lb_genero.Visibility = Visibility.Visible;
            }
            if (autolist.Count == 0)
            {
                lb_genero.Visibility = Visibility.Collapsed;
                lb_genero.ItemsSource = null;
            }
            if (tb_genero.Text == "")
            {
                lb_genero.Visibility = Visibility.Collapsed;
                lb_genero.ItemsSource = null;
            }
        }

        private void lb_genero_SCH(object sender, SelectionChangedEventArgs e)
        {
            if (lb_genero.ItemsSource != null)
            {
                if (lb_genero.SelectedIndex != -1)
                {
                    tb_genero.Text = lb_genero.SelectedItem.ToString();
                    lb_genero.Visibility = Visibility.Collapsed;
                    lb_genero.ItemsSource = null;
                }
            }
        }

        private void tb_genero_LF(object sender, RoutedEventArgs e)
        {
            String buscar = tb_genero.Text.ToUpper();
            List<Generos> autolist = new List<Generos>();

            if (!buscar.Equals(""))
            {

                foreach (Generos item in generos)
                {
                    if (item.ToString().ToUpper().StartsWith(buscar.ToUpper()) && item.ToString() != null && !item.ToString().Equals(""))
                    {
                        autolist.Add(item);
                    }
                }
                if (autolist.Count == 0)
                {
                    MessageBoxResult opcionSeleccionada = MessageBox.Show("¿Desea registrar: '" + buscar + "' como género?", "Operación", MessageBoxButton.OKCancel);
                    if (opcionSeleccionada == MessageBoxResult.OK)
                    {
                        Generos gen = new Generos();
                        int campo = DAOGenero.getId();
                        campo = campo + 1;
                        gen.GNR_ID1 = campo;
                        gen.GNR_NOMBRE1 = tb_genero.Text.ToUpper();
                        String resultado = DAOGenero.guardarGenero(gen);
                        MessageBox.Show(resultado, "Operación");
                        idGenero = campo;
                    }
                    else
                    {
                        tb_genero.Text = "";
                    }

                    categorias = DAOCategoria.getCategoria();
                }
                else
                {
                    tb_genero.Text = autolist[0].GNR_NOMBRE1;
                    idGenero = autolist[0].GNR_ID1;
                    lb_genero.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
