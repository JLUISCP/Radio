using Radio.MODELO.DAO;
using Radio.MODELO.POCO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Radio.VISTAS
{
    /// <summary>
    /// Lógica de interacción para MenuPrincipal.xaml
    /// </summary>
    public partial class MenuPrincipal : Window
    {
        List<Programa> programas = new List<Programa>();

        public MenuPrincipal()
        {
            InitializeComponent();
            mostrarProgramas();
        }

        public void mostrarProgramas()
        {
            programas = DAOPrograma.getProgramas();
            dg_Programas.ItemsSource = programas;

        }

        private void clicBuscar(object sender, RoutedEventArgs e)
        {
            var listaFiltrada = new CollectionViewSource() { Source = programas };
            listaFiltrada.Filter += new FilterEventHandler(filtrarTabla);
            ICollectionView listaObtenida = listaFiltrada.View;
            dg_Programas.ItemsSource = listaObtenida;
        }

        public void filtrarTabla(object sender, FilterEventArgs e)
        {
            String nombre = tb_Buscar.Text;
            var programa = e.Item as Programa;
            if (programa != null)
            {
                if ((programa.NombrePrograma.ToUpper().Contains(nombre.ToUpper()) || programa.NombrePrograma.ToUpper().Contains(nombre.ToUpper()) ||
                    programa.FechaInicio.ToUpper().Contains(nombre.ToUpper())) && programa.EstadoPrograma.Equals(cb_Estado.SelectedIndex))
                {
                    e.Accepted = true;
                }
                else
                {
                    e.Accepted = false;
                }
            }
        }

        private void seleccionarFila(object sender, MouseButtonEventArgs e)
        {
            if (dg_Programas.SelectedItem != null)
            {
                Programa programa = (Programa)dg_Programas.SelectedItem;
                if (programa != null)
                {
                    GUI_ProgramaVisualizado programaVisualizado = new GUI_ProgramaVisualizado(programa.IdPrograma, programa.NombrePrograma,
                    programa.EstadoPrograma, programa.FechaInicio);
                    programaVisualizado.ShowDialog();
                }
            }
        }

        private void cambiarEstado(object sender, SelectionChangedEventArgs e)
        {
            var listaFiltrada = new CollectionViewSource() { Source = programas };
            listaFiltrada.Filter += new FilterEventHandler(filtrarTabla);
            ICollectionView listaObtenida = listaFiltrada.View;
            dg_Programas.ItemsSource = listaObtenida;
        }

        private void btn_Programas_Click(object sender, RoutedEventArgs e)
        {
            GUI_VisualizarProgramas visualizarPrograma = new GUI_VisualizarProgramas();
            visualizarPrograma.ShowDialog();
        }
        private void btn_patron_Click(object sender, RoutedEventArgs e)
        {
            GUI_VisualizarPatrones visualizarPatrones = new GUI_VisualizarPatrones();
            visualizarPatrones.ShowDialog();
        }

        private void btn_Programacion_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_Canciones_Click(object sender, RoutedEventArgs e)
        {
            GUI_Canciones canciones = new GUI_Canciones();
            canciones.ShowDialog();
        }

        private void btn_Cantantes_Click(object sender, RoutedEventArgs e)
        {
            GUI_Cantantes cantantes = new GUI_Cantantes();
            cantantes.ShowDialog();
        }

        private void btn_Reportes_Click(object sender, RoutedEventArgs e)
        {
            GUI_ReporteDeCanciones reporte = new GUI_ReporteDeCanciones();
            reporte.ShowDialog();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}