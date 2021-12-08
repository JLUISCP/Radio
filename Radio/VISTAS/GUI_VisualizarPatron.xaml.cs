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
    /// Lógica de interacción para GUI_VisualizarPatron.xaml
    /// </summary>
    public partial class GUI_VisualizarPatron : Window
    {

        Patron patron;
        int numeroCanciones;
        int numeroDia;

        List<LineaPatron> lineasPatron;
        List<Cancion> cancionesDiaPatron;
        Dictionary<int, List<Cancion>> cancionesLineaPatron;

        List<Cancion> listaCanciones = new List<Cancion>();

        public GUI_VisualizarPatron()
        {
            InitializeComponent();

        }

        public GUI_VisualizarPatron(int numeroDia, Boolean esNuevo, Patron patron)
        {
            this.patron = patron;
            this.numeroDia = numeroDia;


            lineasPatron = new List<LineaPatron>();
            cancionesDiaPatron = new List<Cancion>();
            cancionesLineaPatron = new Dictionary<int, List<Cancion>>();

            InitializeComponent();

            cargarLineasPatron();

        }

        public GUI_VisualizarPatron(int numeroDia, Boolean esNuevo, Patron patron, int cantidadCanciones, List<Cancion> listaCaciones)
        {
            this.patron = patron;
            this.numeroDia = numeroDia;
            this.numeroCanciones = cantidadCanciones;
            this.listaCanciones = listaCaciones;

            lineasPatron = new List<LineaPatron>();
            cancionesDiaPatron = new List<Cancion>();
            cancionesLineaPatron = new Dictionary<int, List<Cancion>>();

            InitializeComponent();
            cargarLineasPatron();
            cargarDatosPatron();
        }

        public void cargarDatosPatron()
        {
            dg_cancion.ItemsSource = listaCanciones;
            tbNumeroCanciones.Text = numeroCanciones.ToString();
        }

        public void cargarLineasPatron()
        {
            lineasPatron = patron.LineaPatron;
            dg_canciones.ItemsSource = lineasPatron;

            foreach (LineaPatron linea in lineasPatron)
            {
                var cancionesObtenidad = DAOCanciones.obtenerCancionLineaPatron(linea.IdCategoria, linea.IdGenero);

                var cancionesObtenidadOrdenada = (List<Cancion>)cancionesObtenidad.OrderBy(x => x.CancionID).ToList();

                cancionesLineaPatron.Add(linea.PrioridadPatron, cancionesObtenidadOrdenada);
            }

            Dictionary<int, List<Cancion>> diccionarioTemporal = new Dictionary<int, List<Cancion>>();

            foreach (KeyValuePair<int, List<Cancion>> lineaPatron in cancionesLineaPatron.OrderBy(key => key.Key))
            {
                diccionarioTemporal.Add(lineaPatron.Key, lineaPatron.Value);
            }
            cancionesLineaPatron = diccionarioTemporal;
        }

        /*
        private void btnModificarPatronAct(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultado = MessageBox.Show("Si modifica el patron. Los cambios solo tendran efecto para este dia del programa", "Confirmar accion", MessageBoxButton.OKCancel);
            if (resultado == MessageBoxResult.OK)
            {
                GUI_RegistrarModificarPatron registrarModificarPatron = new GUI_RegistrarModificarPatron("modificar", patron);
                registrarModificarPatron.Show();
            }

        }
        */
        private void btnGenerarListaMusica(object sender, RoutedEventArgs e)
        {


            String numeroCancionesCadena = tbNumeroCanciones.Text;

            try
            {
                numeroCanciones = Int32.Parse(numeroCancionesCadena);

                if (numeroCanciones >= 1)
                {
                    cancionesDiaPatron.Clear();

                    for (int i = 0; i < numeroCanciones; i++)
                    {

                        foreach (KeyValuePair<int, List<Cancion>> lineaPatron in cancionesLineaPatron)
                        {
                            List<Cancion> listaCancionTemporal = lineaPatron.Value;

                            if (listaCancionTemporal.Count > i)
                            {
                                cancionesDiaPatron.Add(listaCancionTemporal[i]);
                            }
                        }
                    }

                    dg_cancion.ItemsSource = cancionesDiaPatron;
                    dg_cancion.Items.Refresh();
                }
                else
                {
                    if (cancionesDiaPatron.Count > 0)
                    {
                        cancionesDiaPatron.Clear();
                        dg_cancion.ItemsSource = cancionesDiaPatron;
                        dg_cancion.Items.Refresh();
                    }
                }

            }
            catch (FormatException)
            {
                MessageBox.Show("La prioridad debe ser una valor númerico", "Valor Invalido");
            }
        }

        private void btnCancelar(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnRegistra(object sender, RoutedEventArgs e)
        {
            // creo que hace falta el campo del patron dia para cuando se quiera actualizar
            InterfacePatronPrograma enlace = this.Owner as InterfacePatronPrograma;
            if (enlace != null)
            {
                enlace.agregar(numeroDia, numeroCanciones, cancionesDiaPatron);
            }

            this.Close();
        }
    }
}
