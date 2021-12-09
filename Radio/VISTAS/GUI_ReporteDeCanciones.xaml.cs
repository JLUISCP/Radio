using Radio.INTERFAZ;
using Radio.MODELO.DAO;
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
    /// Lógica de interacción para GUI_ReporteDeCanciones.xaml
    /// </summary>
    public partial class GUI_ReporteDeCanciones : Window, IObservador
    {
        bool estadoDeComponenteUI = true;
        String titulo = "Lista de registros";
        int diseñoDeVentana = 0;
        String orden = "";
        String estado = "";

        public GUI_ReporteDeCanciones()
        {
            InitializeComponent();
            rb_Clave.IsChecked = true;
            rb_Todas.IsChecked = true;
            btn_MasGeneros.IsEnabled = false;
            btn_MasCategorias.IsEnabled = false;
            btn_MasCantantes.IsEnabled = false;
            tb_Genero.IsEnabled = false;
            tb_Categoria.IsEnabled = false;
            tb_Cantante.IsEnabled = false;
        }

        private void clicReestablecer(object sender, RoutedEventArgs e)
        {
            chb_NoUtilizadas.IsChecked = false;
            chb_Cantante.IsChecked = false;
            chb_Categoria.IsChecked = false;
            chb_Genero.IsChecked = false;
            rb_Todas.IsChecked = true;
            rb_Clave.IsChecked = true;
        }

        private void clicNoUtilizadas(object sender, RoutedEventArgs e)
        {
            if (chb_NoUtilizadas.IsChecked == true)
            {
                estadoDeComponenteUI = false;
                estaHabilitadoComponentesUI(estadoDeComponenteUI);
                estaHabilitadoComponentesDeCantante(estadoDeComponenteUI);
                estaHabilitadoComponentesDeCategoria(estadoDeComponenteUI);
                estaHabilitadoComponentesDeGenero(estadoDeComponenteUI);
            }
            else
            {
                estadoDeComponenteUI = true;
                estaHabilitadoComponentesUI(estadoDeComponenteUI);
                if (chb_Genero.IsChecked == true)
                {
                    estaHabilitadoComponentesDeGenero(estadoDeComponenteUI);
                }
                if (chb_Categoria.IsChecked == true)
                {
                    estaHabilitadoComponentesDeCategoria(estadoDeComponenteUI);
                }
                if (chb_Cantante.IsChecked == true)
                {
                    estaHabilitadoComponentesDeCantante(estadoDeComponenteUI);
                }
            }
        }

        private void estaHabilitadoComponentesUI(bool estadoDeComponente)
        {
            chb_Cantante.IsEnabled = estadoDeComponente;
            chb_Categoria.IsEnabled = estadoDeComponente;
            chb_Genero.IsEnabled = estadoDeComponente;
            rb_Activas.IsEnabled = estadoDeComponente;
            rb_Cantante.IsEnabled = estadoDeComponente;
            rb_Clave.IsEnabled = estadoDeComponente;
            rb_Inactivas.IsEnabled = estadoDeComponente;
            rb_Titulo.IsEnabled = estadoDeComponente;
            rb_Todas.IsEnabled = estadoDeComponente;
        }

        private void clicGeneroCheckBox(object sender, RoutedEventArgs e)
        {
            if (chb_Genero.IsChecked == true)
            {
                estadoDeComponenteUI = true;
                estaHabilitadoComponentesDeGenero(estadoDeComponenteUI);
            }
            else
            {
                estadoDeComponenteUI = false;
                tb_Genero.Clear();
                estaHabilitadoComponentesDeGenero(estadoDeComponenteUI);
            }
        }

        private void estaHabilitadoComponentesDeGenero(bool estadoDeComponenteDeGenero)
        {
            tb_Genero.IsEnabled = estadoDeComponenteDeGenero;
            btn_MasGeneros.IsEnabled = estadoDeComponenteDeGenero;
        }

        private void clicCategoriaCheckBox(object sender, RoutedEventArgs e)
        {
            if (chb_Categoria.IsChecked == true)
            {
                estadoDeComponenteUI = true;
                estaHabilitadoComponentesDeCategoria(estadoDeComponenteUI);
            }
            else
            {
                estadoDeComponenteUI = false;
                tb_Categoria.Clear();
                estaHabilitadoComponentesDeCategoria(estadoDeComponenteUI);
            }
        }

        private void estaHabilitadoComponentesDeCategoria(bool estadoDeComponenteDeCategoria)
        {
            tb_Categoria.IsEnabled = estadoDeComponenteDeCategoria;
            btn_MasCategorias.IsEnabled = estadoDeComponenteDeCategoria;
        }

        private void clicCantanteCheckBox(object sender, RoutedEventArgs e)
        {
            if (chb_Cantante.IsChecked == true)
            {
                estadoDeComponenteUI = true;
                estaHabilitadoComponentesDeCantante(estadoDeComponenteUI);
            }
            else
            {
                estadoDeComponenteUI = false;
                tb_Cantante.Clear();
                estaHabilitadoComponentesDeCantante(estadoDeComponenteUI);
            }
        }

        private void estaHabilitadoComponentesDeCantante(bool estadoDeComponenteDeCantante)
        {
            tb_Cantante.IsEnabled = estadoDeComponenteDeCantante;
            btn_MasCantantes.IsEnabled = estadoDeComponenteDeCantante;
        }

        private void clicMasGeneros(object sender, RoutedEventArgs e)
        {
            titulo = "Lista de géneros";
            diseñoDeVentana = 1;
            mostrarListaDeRegistros(titulo, diseñoDeVentana);
        }

        private void clicMasCategorias(object sender, RoutedEventArgs e)
        {
            titulo = "Lista de categorías";
            diseñoDeVentana = 2;
            mostrarListaDeRegistros(titulo, diseñoDeVentana);
        }

        private void clicMasCantantes(object sender, RoutedEventArgs e)
        {
            titulo = "Lista de cantantes";
            diseñoDeVentana = 3;
            mostrarListaDeRegistros(titulo, diseñoDeVentana);
        }

        private void mostrarListaDeRegistros(String titulo, int diseñoDeVentana)
        {
            GUI_ListaDeRegistros ventanaListaDeRegistros = new GUI_ListaDeRegistros(titulo, diseñoDeVentana, this);
            ventanaListaDeRegistros.ShowDialog();
        }

        private void seleccionarOrdenDeCanciones(object sender, RoutedEventArgs e)
        {
            if (rb_Clave.IsChecked == true)
            {
                orden = (String)rb_Clave.Content;
            }
            else if (rb_Titulo.IsChecked == true)
            {
                orden = (String)rb_Titulo.Content;
            }
            else
            {
                orden = (String)rb_Cantante.Content;
            }
        }

        private void seleccionarEstadoDeCanciones(object sender, RoutedEventArgs e)
        {
            if (rb_Todas.IsChecked == true)
            {
                estado = (String)rb_Todas.Content;
            }
            else if (rb_Activas.IsChecked == true)
            {
                estado = (String)rb_Activas.Content;
            }
            else
            {
                estado = (String)rb_Inactivas.Content;
            }
        }

        private void clicGenerarReporte(object sender, RoutedEventArgs e)
        {
            String cantante = tb_Cantante.Text.ToUpper();
            String categoria = tb_Categoria.Text.ToUpper();
            String genero = tb_Genero.Text.ToUpper();
            if (chb_NoUtilizadas.IsChecked == true)
            {
                mostrarReporte(cantante, categoria, genero);
            }
            else if ((chb_Cantante.IsChecked == false) && (chb_Categoria.IsChecked == false) && (chb_Genero.IsChecked == false))
            {
                mostrarReporte(cantante, categoria, genero);
            }
            else
            {
                validarCamposVacios(cantante, categoria, genero);
            }
        }

        private void validarCamposVacios(String nombreCantante, String nombreCategoria, String nombreGenero)
        {
            bool estaVacio = false;
            if ((chb_Cantante.IsChecked == true) && (nombreCantante.Length == 0))
            {
                estaVacio = true;
            }
            if ((chb_Categoria.IsChecked == true) && (nombreCategoria.Length == 0))
            {
                estaVacio = true;
            }
            if ((chb_Genero.IsChecked == true) && (nombreGenero.Length == 0))
            {
                estaVacio = true;
            }
            if (!estaVacio)
            {
                validarInformacionExistente(nombreCantante, nombreCategoria, nombreGenero);
            }
            else
            {
                MessageBox.Show("Los filtros seleccionados están vacíos. Selecciona aquellos que usará.", "Campos vacíos.",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void validarInformacionExistente(String nombreCantante, String nombreCategoria, String nombreGenero)
        {
            bool existeCantante = DAOCantantes.encontrarCantante(nombreCantante);
            bool existeCategoria = DAOCategoria.encontrarCategoria(nombreCategoria);
            bool existeGenero = DAOGenero.encontrarGenero(nombreGenero);
            if ((existeCantante == true) || (existeCategoria == true) || (existeGenero == true))
            {
                mostrarReporte(nombreGenero, nombreCantante, nombreCategoria);
            }
            else
            {
                MessageBox.Show("Verifique que la información registrada exista en el sistema.", "Información inexistente",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void mostrarReporte(String nombreGenero, String nombreCantante, String nombreCategoria)
        {
            String contenidoDeReporte = "";
            if (chb_NoUtilizadas.IsChecked == true)
            {
                contenidoDeReporte = "Radio: 40 principales\nTotal de registros: ";
                titulo = "Reporte de canciones no utilizadas";
                diseñoDeVentana = 1;
                GUI_ReporteGenerado ventanaDeReporteDeCancionesNoUtilizadas = new GUI_ReporteGenerado(titulo, contenidoDeReporte, diseñoDeVentana);
                ventanaDeReporteDeCancionesNoUtilizadas.ShowDialog();
            }
            else if ((chb_Cantante.IsChecked == false) && (chb_Categoria.IsChecked == false) && (chb_Genero.IsChecked == false))
            {
                contenidoDeReporte = "Total de registros: ";
                titulo = "Reporte general de canciones";
                diseñoDeVentana = 2;
                GUI_ReporteGenerado ventanaDeReporteGeneralDeCanciones = new GUI_ReporteGenerado(orden, estado, titulo, contenidoDeReporte,
                    diseñoDeVentana);
                ventanaDeReporteGeneralDeCanciones.ShowDialog();
            }
            else
            {
                if (chb_Genero.IsChecked == true)
                {
                    contenidoDeReporte += "<Género = " + nombreGenero + ">";
                }
                if (chb_Categoria.IsChecked == true)
                {
                    contenidoDeReporte += "<Categoría = " + nombreCategoria + ">";
                }
                if (chb_Cantante.IsChecked == true)
                {
                    contenidoDeReporte += "<Cantante = " + nombreCantante + ">";
                }
                contenidoDeReporte += "\n<Orden = " + orden + "><Estado = " + estado + "> Total de registros: ";
                titulo = "Reporte de canciones específicas";
                diseñoDeVentana = 3;
                GUI_ReporteGenerado ventanaDeReporteGeneralDeCanciones = new GUI_ReporteGenerado(nombreCantante, nombreCategoria, nombreGenero,
                    orden, estado, titulo, contenidoDeReporte, diseñoDeVentana);
                ventanaDeReporteGeneralDeCanciones.ShowDialog();
            }
        }

        public void actualizarInformacion(string resultadoDeVentanaOrigen, int ventanaOrigen)
        {
            if (ventanaOrigen == 1)
            {
                tb_Genero.Text = resultadoDeVentanaOrigen;
            }
            else if (ventanaOrigen == 2)
            {
                tb_Categoria.Text = resultadoDeVentanaOrigen;
            }
            else if (ventanaOrigen == 3)
            {
                tb_Cantante.Text = resultadoDeVentanaOrigen;
            }
        }
    }
}
