using Radio.INTERFAZ;
using Radio.MODELO.DAO;
using Radio.MODELO.POCO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Lógica de interacción para GUI_ListaDeRegistros.xaml
    /// </summary>
    public partial class GUI_ListaDeRegistros : Window
    {
        private int tipoDeLista;
        private List<Genero> generos = new List<Genero>();
        private List<Categoria> categorias = new List<Categoria>();
        private List<Cantantes> cantantes = new List<Cantantes>();
        private IObservador notificacion;

        public GUI_ListaDeRegistros()
        {
            InitializeComponent();
            generos = new List<Genero>();
            categorias = new List<Categoria>();
            cantantes = new List<Cantantes>();
        }

        public GUI_ListaDeRegistros(String titulo, int valor, IObservador notificacion) : this()
        {
            Title = titulo;
            tipoDeLista = valor;
            this.notificacion = notificacion;
            DataGridTextColumn columna1 = new DataGridTextColumn();
            dg_Registros.AutoGenerateColumns = false;
            if (valor == 1)
            {
                columna1.Header = "Géneros";
                mostrarGeneros();
                columna1.Binding = new Binding("NombreGenero");
            }
            else if (valor == 2)
            {
                columna1.Header = "Categorías";
                mostrarCategorias();
                columna1.Binding = new Binding("NombreCategoria");
            }
            else if (valor == 3)
            {
                columna1.Header = "Cantantes";
                mostrarCantantes();
                columna1.Binding = new Binding("CNT_NOMBRE1");
            }
            dg_Registros.Columns.Add(columna1);
        }

        private void mostrarGeneros()
        {
            generos = DAOGenero.obtenerGeneros();
            if (generos == null)
            {
                MessageBox.Show("No hay conexión por el momento.", "Sin conexión", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                dg_Registros.ItemsSource = generos;
            }
        }

        private void mostrarCategorias()
        {
            categorias = DAOCategoria.obtenerCategorias();
            if (categorias == null)
            {
                MessageBox.Show("No hay conexión por el momento.", "Sin conexión", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                dg_Registros.ItemsSource = categorias;
            }
        }

        private void mostrarCantantes()
        {
            cantantes = DAOCantantes.getCantante();
            if (cantantes == null)
            {
                MessageBox.Show("No hay conexión por el momento.", "Sin conexión", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                dg_Registros.ItemsSource = cantantes;
            }
        }

        private void seleccionarFila(object sender, MouseButtonEventArgs e)
        {
            if (dg_Registros.SelectedItem != null)
            {
                String nombreSeleccionado = (String)dg_Registros.SelectedItem.ToString();
                if (nombreSeleccionado != "")
                {
                    notificacion.actualizarInformacion(nombreSeleccionado, tipoDeLista);
                    this.Close();
                }
            }
        }

        private void clicBuscar(object sender, RoutedEventArgs e)
        {
            if (tipoDeLista == 1)
            {
                var listaFiltrada = new CollectionViewSource() { Source = generos };
                listaFiltrada.Filter += new FilterEventHandler(filtrarTabla);
                ICollectionView listaObtenida = listaFiltrada.View;
                dg_Registros.ItemsSource = listaObtenida;
            }
            else if (tipoDeLista == 2)
            {
                var listaFiltrada = new CollectionViewSource() { Source = categorias };
                listaFiltrada.Filter += new FilterEventHandler(filtrarTabla);
                ICollectionView listaObtenida = listaFiltrada.View;
                dg_Registros.ItemsSource = listaObtenida;
            }
            else if (tipoDeLista == 3)
            {
                var listaFiltrada = new CollectionViewSource() { Source = cantantes };
                listaFiltrada.Filter += new FilterEventHandler(filtrarTabla);
                ICollectionView listaObtenida = listaFiltrada.View;
                dg_Registros.ItemsSource = listaObtenida;
            }
        }

        public void filtrarTabla(object sender, FilterEventArgs e)
        {
            String nombre = tb_Buscar.Text.ToUpper();
            if (tipoDeLista == 1)
            {
                var genero = e.Item as Genero;
                if (genero != null)
                {
                    if (genero.NombreGenero.Contains(nombre))
                    {
                        e.Accepted = true;
                    }
                    else
                    {
                        e.Accepted = false;
                    }
                }
            }
            else if (tipoDeLista == 2)
            {
                var categoria = e.Item as Categoria;
                if (categoria != null)
                {
                    if (categoria.NombreCategoria.Contains(nombre))
                    {
                        e.Accepted = true;
                    }
                    else
                    {
                        e.Accepted = false;
                    }
                }
            }
            else if (tipoDeLista == 3)
            {
                var cantante = e.Item as Cantantes;
                if (cantante != null)
                {
                    if (cantante.CNT_NOMBRE1.Contains(nombre))
                    {
                        e.Accepted = true;
                    }
                    else
                    {
                        e.Accepted = false;
                    }
                }
            }
        }
    }
}
