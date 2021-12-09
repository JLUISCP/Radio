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
    /// Lógica de interacción para GUI_ProgramaVisualizado.xaml
    /// </summary>
    public partial class GUI_ProgramaVisualizado : Window
    {
        List<Locutor> locutores = new List<Locutor>();
        List<HorarioDelPrograma> horarios = new List<HorarioDelPrograma>();
        List<CancionDelDia> canciones = new List<CancionDelDia>();
        int programaVisualizado;

        public GUI_ProgramaVisualizado()
        {
            InitializeComponent();
        }

        public GUI_ProgramaVisualizado(int programa, String nombrePrograma, int estado, String fecha) : this()
        {
            programaVisualizado = programa;
            mostrarPrograma();
            lb_Programa.Content += nombrePrograma;
            lb_Fecha.Content += fecha;
            if (estado == 0)
            {
                lb_Estado.Content += "Inactivo";
            }
            else if (estado == 1)
            {
                lb_Estado.Content += "Activo";
            }
        }

        private void mostrarPrograma()
        {
            locutores = DAOLocutor.obtenerLocutorPrograma(programaVisualizado);
            horarios = DAOHorarioDelPrograma.consultarHorarios(programaVisualizado);
            canciones = DAOPrograma.consultarCancionesDelPrograma(programaVisualizado);
            if ((locutores == null) || (horarios == null) || (canciones == null))
            {
                MessageBox.Show("No hay conexión a la base de datos por el momento", "Error de conexión", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                dg_Locutores.ItemsSource = locutores;
                dg_Horarios.ItemsSource = horarios;
                dg_Canciones.ItemsSource = canciones;
            }
        }

        private void mostrarDetalles(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dg_Horarios.SelectedItem != null)
            {
                var listaFiltrada = new CollectionViewSource() { Source = canciones };
                listaFiltrada.Filter += new FilterEventHandler(filtrarTabla);
                ICollectionView listaObtenida = listaFiltrada.View;
                dg_Canciones.ItemsSource = listaObtenida;
            }
        }

        public void filtrarTabla(object sender, FilterEventArgs e)
        {
            var cancion = e.Item as CancionDelDia;
            HorarioDelPrograma horario = (HorarioDelPrograma)dg_Horarios.SelectedItem;
            if (cancion != null)
            {
                if (cancion.DiaDeSemana.Contains(horario.DiaDeSemana))
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
