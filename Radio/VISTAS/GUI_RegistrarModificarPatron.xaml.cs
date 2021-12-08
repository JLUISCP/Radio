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
    /// Lógica de interacción para GUI_RegistrarModificarPatron.xaml
    /// </summary>
    public partial class GUI_RegistrarModificarPatron : Window
    {
        Patron patron;
        String esNuevo;
        List<Categoria> categorias;
        List<Genero> generos;
        List<LineaPatron> listaPatron;
        RadioPrograma unicoRadio;

        int contadorLineaPatron = 1;


        public GUI_RegistrarModificarPatron()
        {
            InitializeComponent();
        }

        public GUI_RegistrarModificarPatron(String esNuevo, Patron patron)
        {
            this.patron = patron;
            this.esNuevo = esNuevo;

            InitializeComponent();

            categorias = new List<Categoria>();
            generos = new List<Genero>();
            listaPatron = new List<LineaPatron>();
            unicoRadio = new RadioPrograma();

            cargarCategorias();
            cargarGeneros();

            if (esNuevo.Equals("nuevo"))
            {
                lbTituloVentana.Content = "Registro de Patrón";
                cargarElementosNuevoRegistro();
            }
            if (esNuevo.Equals("visualizar"))
            {
                lbTituloVentana.Content = "Visualización de Patrón";
                cargarElementosVisualizar();
            }
            if (esNuevo.Equals("modificar"))
            {
                lbTituloVentana.Content = "Modificacion de Patrón";
                cargarElementosModificar();
            }
        }

        private void cargarElementosModificar()
        {
            tbClave.IsEnabled = false;
            tbClave.Text = patron.IdPatron.ToString();

            tbNombre.Text = patron.NombrePatron;

            tbRadio.IsEnabled = false;
            tbRadio.Text = patron.NombreRadio;

            listaPatron = patron.LineaPatron;
            refrescarTabla(listaPatron);

            List<int> numeros = new List<int>();

            foreach (LineaPatron linea in listaPatron)
            {
                numeros.Add(linea.PrioridadPatron);
            }

            contadorLineaPatron = numeros.Max() + 1;
        }

        public void cargarElementosVisualizar()
        {
            dg_canciones.IsEnabled = false;

            tbClave.IsEnabled = false;
            tbClave.Text = patron.IdPatron.ToString();

            tbNombre.IsEnabled = false;
            tbNombre.Text = patron.NombrePatron;

            tbRadio.IsEnabled = false;
            tbRadio.Text = patron.NombreRadio;

            lbanadirPatron.Visibility = Visibility.Hidden;
            lbCategoria.Visibility = Visibility.Hidden;
            cbCategoria.Visibility = Visibility.Hidden;
            lbgenero.Visibility = Visibility.Hidden;
            cbGenero.Visibility = Visibility.Hidden;
            lbtotalCancionesText.Visibility = Visibility.Hidden;
            lbTotalCanciones.Visibility = Visibility.Hidden;
            btAgregar.Visibility = Visibility.Hidden;
            btnQuitarLinea.Visibility = Visibility.Hidden;
            btnRegistrar.Visibility = Visibility.Hidden;
            btnCancelar.Visibility = Visibility.Hidden;
            btnCerra.Visibility = Visibility.Visible;

            dg_canciones.ItemsSource = patron.LineaPatron;

        }

        private void cargarElementosNuevoRegistro()
        {
            tbClave.Text = DAOPatron.ultimoIdRegistradoPatron().ToString();
            tbClave.IsEnabled = false;

            unicoRadio = DAORadio.unicoRadioExistente();
            tbRadio.Text = unicoRadio.Nombre;
            tbRadio.IsEnabled = false;
        }

        public void cargarCategorias()
        {
            categorias = DAOCategoria.obtenerCategorias();
            cbCategoria.ItemsSource = categorias;
        }

        public void cargarGeneros()
        {
            generos = DAOGenero.obtenerGeneros();
            cbGenero.ItemsSource = generos;
        }

        private void cbCambioCategoria(object sender, SelectionChangedEventArgs e)
        {
            mostrartotalCanciones();
        }

        private void cbCambioGenero(object sender, SelectionChangedEventArgs e)
        {
            mostrartotalCanciones();
        }

        public void mostrartotalCanciones()
        {
            if (cbCategoria.SelectedItem != null && cbGenero.SelectedItem != null)
            {
                Categoria categoriaSelecciona = (Categoria)cbCategoria.SelectedItem;
                Genero generoSelecciona = (Genero)cbGenero.SelectedItem;

                lbTotalCanciones.Content = DAOCanciones.obtenerTotalCanciones(categoriaSelecciona.IdCategoria, generoSelecciona.IdGenero).ToString();
            }
        }

        private void btnAgregarLineaPatron(object sender, RoutedEventArgs e)
        {
            if (cbCategoria.SelectedItem != null && cbGenero.SelectedItem != null)
            {
                Categoria categoriaSelecciona = (Categoria)cbCategoria.SelectedItem;
                Genero generoSelecciona = (Genero)cbGenero.SelectedItem;

                LineaPatron lineaPatron = new LineaPatron();
                lineaPatron.PrioridadPatron = contadorLineaPatron;
                lineaPatron.IdCategoria = categoriaSelecciona.IdCategoria;
                lineaPatron.NombreCategoria = categoriaSelecciona.NombreCategoria;
                lineaPatron.IdGenero = generoSelecciona.IdGenero;
                lineaPatron.NombreGenero = generoSelecciona.NombreGenero;
                lineaPatron.NumeroCanciones = int.Parse(lbTotalCanciones.Content.ToString());

                Boolean linePatronRepetidad = false;

                foreach (LineaPatron linea in listaPatron)
                {
                    if (linea.IdCategoria == lineaPatron.IdCategoria && linea.IdGenero == lineaPatron.IdGenero)
                    {
                        linePatronRepetidad = true;
                    }
                }

                if (!linePatronRepetidad)
                {
                    listaPatron.Add(lineaPatron);
                    refrescarTabla(listaPatron);
                    contadorLineaPatron++;
                }
                else
                {
                    MessageBox.Show("La linea patron de Categoria '" + lineaPatron.NombreCategoria + "' y Genero '" + lineaPatron.NombreGenero + "', ya esta registrada", "Linea Patron repetida");
                }

                cbCategoria.SelectedItem = null;
                cbGenero.SelectedItem = null;
                lbTotalCanciones.Content = 0;
            }
            else
            {
                MessageBox.Show("Debe definir una linea de patron", "No se puede añadir la linea");
            }
        }

        private void btnQuitarLineaPatron(object sender, RoutedEventArgs e)
        {
            int lineaPatronSeleccionada = dg_canciones.SelectedIndex;
            if (lineaPatronSeleccionada >= 0)
            {
                LineaPatron lineaPatronAux = (LineaPatron)dg_canciones.SelectedItem;

                List<LineaPatron> nuevaListaLineaPatronAux = new List<LineaPatron>();

                foreach (LineaPatron linea in listaPatron)
                {
                    if (lineaPatronAux.PrioridadPatron != linea.PrioridadPatron)
                    {
                        nuevaListaLineaPatronAux.Add(linea);
                    }
                }

                listaPatron = nuevaListaLineaPatronAux;

                refrescarTabla(listaPatron);

                dg_canciones.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Para eliminar una linea patron debe seleccionarla", "Error al Eliminar");
            }
        }

        private void lineaPatronCambiPrioridad(object sender, DataGridCellEditEndingEventArgs e)
        {

            String celda = dg_canciones.Columns[0].GetCellContent(dg_canciones.Items[dg_canciones.SelectedIndex]).ToString();
            String valorCelda = celda.Remove(0, 33);

            LineaPatron lineaPatronAux = (LineaPatron)dg_canciones.SelectedItem;

            try
            {
                int nuevaPrioridad = Int32.Parse(valorCelda);
                int viejaPrioridad = lineaPatronAux.PrioridadPatron;
                int prioridadNegativa = -1;

                if (nuevaPrioridad < 0)
                {
                    MessageBox.Show("La prioridad debe ser una valor númerico mayor a cero", "Valor Invalido");
                }
                else
                {
                    List<LineaPatron> nuevaListaLineaPatronAux = new List<LineaPatron>();

                    foreach (LineaPatron linea in listaPatron)
                    {
                        if (linea.PrioridadPatron == viejaPrioridad)
                        {
                            linea.PrioridadPatron = nuevaPrioridad;
                        }
                        else if (linea.PrioridadPatron == nuevaPrioridad)
                        {
                            linea.PrioridadPatron = prioridadNegativa;
                        }
                        nuevaListaLineaPatronAux.Add(linea);
                    }

                    List<LineaPatron> nuevaListaLineaPatron = new List<LineaPatron>();

                    foreach (LineaPatron linea in nuevaListaLineaPatronAux)
                    {
                        if (linea.PrioridadPatron == prioridadNegativa)
                        {
                            linea.PrioridadPatron = viejaPrioridad;
                        }
                        nuevaListaLineaPatron.Add(linea);
                    }
                    listaPatron = nuevaListaLineaPatron;
                    refrescarTabla(listaPatron);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("La prioridad debe ser una valor númerico", "Valor Invalido");
            }
        }

        public void refrescarTabla(List<LineaPatron> listaPatronRecibida)
        {
            dg_canciones.ItemsSource = listaPatronRecibida;
            dg_canciones.Items.Refresh();
        }

        private void btnRegistrarPatron(object sender, RoutedEventArgs e)
        {
            Boolean camposValidos = true;
            String nombrePatron = tbNombre.Text;
            int idRadio = unicoRadio.IdRadio;

            if (nombrePatron.Equals(""))
            {
                MessageBox.Show("Debe ingresar un nombre para el patron", "Campos Faltantes");
                camposValidos = false;

            }
            else if (listaPatron.Count < 0)
            {
                MessageBox.Show("Se debe colocar minimo una linea de patron", "Linea patron faltante");

                camposValidos = false;
            }

            if (camposValidos && esNuevo.Equals("nuevo"))
            {
                int resultado = DAOPatron.setPatron(nombrePatron, idRadio, listaPatron);

                if (resultado == 1)
                {
                    MessageBox.Show("Se registro el patron '" + nombrePatron + "'", "Registro");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No se puedo realizar el registro", "Registro");
                    this.Close();
                }
            }

            if (camposValidos && esNuevo.Equals("modificar"))
            {

                int resultadoEliminacion = DAOLineaPatron.eliminarLineasPatron(patron.IdPatron);
                int resultado = 0;
                if (resultadoEliminacion > 0)
                {
                    resultado = DAOPatron.updatePatron(patron.IdPatron, nombrePatron, listaPatron);
                }
                else
                {
                    MessageBox.Show("La modificación salio mal. Ya fue su patron", "Error");
                }

                if (resultado == 1)
                {
                    MessageBox.Show("Se actualizo el patron '" + nombrePatron + "'", "Actualizacion");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No se puedo realizar la actualizacion", "Actualizacion");
                    this.Close();
                }
            }

        }

        private void btnCerrar(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCancelarRegistro(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
