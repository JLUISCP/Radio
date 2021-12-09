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
    /// Lógica de interacción para GUI_ReporteGenerado.xaml
    /// </summary>
    public partial class GUI_ReporteGenerado : Window
    {
        private List<Cancion> canciones = new List<Cancion>();
        private List<ContadorDeCancion> contadores = new List<ContadorDeCancion>();

        public GUI_ReporteGenerado()
        {
            InitializeComponent();
            canciones = new List<Cancion>();
            contadores = new List<ContadorDeCancion>();
        }

        public GUI_ReporteGenerado(String titulo, String contenido, int valor) : this()
        {
            lb_Titulo.Content = titulo;
            lb_Guid.Content = Guid.NewGuid();
            lb_Fecha.Content += DateTime.Today.ToString("dd/MM/yyyy");
            if((valor == 1) || (valor == 3))
            {
                DataGridTextColumn columna1 = new DataGridTextColumn();
                columna1.Header = "Género";
                columna1.Binding = new Binding("NombreGenero");
                dg_Registros.Columns.Add(columna1);
                DataGridTextColumn columna2 = new DataGridTextColumn();
                columna2.Header = "Categoría";
                columna2.Binding = new Binding("NombreCategoria");
                dg_Registros.Columns.Add(columna2);
            }
            if (valor == 1)
            {
                DataGridTextColumn columna3 = new DataGridTextColumn();
                columna3.Header = "Canciones";
                columna3.Binding = new Binding("TotalCanciones");
                dg_Registros.Columns.Add(columna3);
                contarCancionesNoUtilizadas();
                if (contadores != null)
                {
                    lb_Contenido.Content = contenido + contadores.Count().ToString();
                }
            }
        }

        public GUI_ReporteGenerado(String orden, String estado, String titulo, String contenido, int valor) : this(titulo, contenido, valor)
        {
            DataGridTextColumn columna1 = new DataGridTextColumn();
            columna1.Header = "Clave";
            columna1.Binding = new Binding("Clave");
            dg_Registros.Columns.Add(columna1);
            DataGridTextColumn columna2 = new DataGridTextColumn();
            columna2.Header = "Titulo";
            columna2.Binding = new Binding("Titulo");
            dg_Registros.Columns.Add(columna2);
            DataGridTextColumn columna3 = new DataGridTextColumn();
            columna3.Header = "Cantante";
            columna3.Binding = new Binding("NombreCantante");
            dg_Registros.Columns.Add(columna3);
            if (valor == 2)
            {
                mostrarReporte(null, null, null, orden, estado);
                if (canciones != null)
                {
                    lb_Contenido.Content = contenido + canciones.Count().ToString();
                }
            }

        }

        public GUI_ReporteGenerado(String nombreCantante, String nombreCategoria, String nombreGenero, String orden, String estado,
            String titulo, String contenido, int valor) : this(orden, estado, titulo, contenido, valor)
        {
            mostrarReporte(nombreCantante, nombreCategoria, nombreGenero, orden, estado);
            if (canciones != null)
            {
                lb_Contenido.Content = contenido + canciones.Count().ToString();
            }
        }

        public void contarCancionesNoUtilizadas()
        {
            contadores = DAOContadorDeCancion.consultarCancionesNoUtilizadas();
            if (contadores == null)
            {
                MessageBox.Show("No hay conexión a la base de datos por el momento.", "Error de conexión", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                dg_Registros.ItemsSource = contadores;
            }

        }

        public void mostrarReporte(String nombreCantante, String nombreCategoria, String nombreGenero, String orden, String estado)
        {
            if ((nombreCantante == null) && (nombreCategoria == null) && (nombreGenero == null))
            {
                canciones = DAOCanciones.consultarCancionesNoFiltradas(orden, estado);
            }
            else
            {
                canciones = DAOCanciones.consultarCancionesFiltradas(nombreCantante, nombreCategoria, nombreGenero, orden, estado);
            }
            if (canciones == null)
            {
                MessageBox.Show("No hay conexión a la base de datos por el momento.", "Error de conexión", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                dg_Registros.ItemsSource = canciones;
            }
        }
    }
}
