using Microsoft.Win32;
using Radio.MODELO.DAO;
using Radio.MODELO.POCO;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Lógica de interacción para GUI_RegistrarModificarPrograma.xaml
    /// </summary>
    public partial class GUI_RegistrarModificarPrograma : Window, InterfacePatronPrograma
    {
        Programa programa;
        Programa programaVisualizar;
        Patron patronSeleccionado;
        RadioPrograma radio;
        List<Locutor> locutores = new List<Locutor>(DAOLocutor.getLocutor());
        List<Patron> patron;


        Boolean esNuevo;

        Boolean diaLunesActivo;
        Boolean diaMartesActivo;
        Boolean diaMiercolesActivo;
        Boolean diaJuevesActivo;
        Boolean diaViernesActivo;
        Boolean diaSabadoActivo;
        Boolean diaDomingoActivo;

        MemoryStream imagenLogo;
        byte[] imageData;

        Boolean[] diaActivo = new Boolean[7];
        int[] idDia = new int[7];
        int[] numCaciones = new int[7] { -1, -1, -1, -1, -1, -1, -1 };
        int[] numeroDia = new int[7];
        TimeSpan[] horaInicio = new TimeSpan[7];
        TimeSpan[] horaFinal = new TimeSpan[7];
        Boolean[] estadoDia = new Boolean[7];
        int[] idPatron = new int[7];
        int[] claveProgramaDia = new int[7];
        List<Cancion>[] listaCaciones = new List<Cancion>[7];

        int idLocutorUno;
        int idLocutorDos;
        int estadoPrograma;

        int idProgramaValidarHorarios;

        public GUI_RegistrarModificarPrograma()
        {
            InitializeComponent();
        }

        public GUI_RegistrarModificarPrograma(Boolean esNuevo, Programa programa) : this()
        {

            this.programa = programa;
            this.esNuevo = esNuevo;
            patron = new List<Patron>();
            patronSeleccionado = new Patron();
            InitializeComponent();
            cargarPatrones();

            if (esNuevo)
            {
                lbTituloVentana.Content = "Registrando Patrón";
                diaOperacionesDesabilitado();
                idProgramaValidarHorarios = -1;
            }
            else
            {
                lbTituloVentana.Content = "Modificando Patrón";
                cargarInformacionPrograma();
                idProgramaValidarHorarios = programa.IdPrograma;
            }
        }

        public GUI_RegistrarModificarPrograma(Programa programaVisualizar) : this()
        {

            this.programaVisualizar = programaVisualizar;
            patron = new List<Patron>();
            patronSeleccionado = new Patron();
            lbTituloVentana.Content = "Visualización de  Patrón";
            InitializeComponent();
            cargarPatrones();
            visualizarPrograma();
        }

        public void cargarPatrones()
        {
            patron = DAOPatron.getPatrones();
            cbLunesPatron.ItemsSource = patron;
            cbMartesPatron.ItemsSource = patron;
            cbMiercolesPatron.ItemsSource = patron;
            cbJuevesPatron.ItemsSource = patron;
            cbViernesPatron.ItemsSource = patron;
            cbSabadoPatron.ItemsSource = patron;
            cbDomingoPatron.ItemsSource = patron;
        }

        private void cargarInformacionPrograma()
        {
            btnCerrar.Visibility = Visibility.Hidden;
            EstadoRegistro.Visibility = Visibility.Hidden;

            imgLogo.Source = imgByteImage(programa.Imagen);

            tbClave.Text = programa.IdPrograma.ToString();
            tbClave.IsEnabled = false;

            tbRadio.Text = programa.NombreRadio;
            tbRadio.IsEnabled = false;

            tbPrograma.Text = programa.NombrePrograma;

            estaActivo.Visibility = Visibility.Visible;
            Inactivo.Visibility = Visibility.Visible;

            if (programa.EstadoPrograma == 1)
            {
                estaActivo.IsChecked = true;
            }
            else
            {
                Inactivo.IsChecked = true;
            }

            if (programa.Locutor.Count > 0)
            {
                tbLocutorUno.Text = "";
                tbLocutorDos.Text = "";
            }
            if (programa.Locutor.Count > 0)
            {
                tbLocutorUno.Text = programa.Locutor[0].NombreLocutor;
                lbLocutorUno.ItemsSource = null;
                lbLocutorUno.Visibility = Visibility.Collapsed;

                if (programa.Locutor.Count > 1)
                {
                    tbLocutorDos.Text = programa.Locutor[1].NombreLocutor;
                    lbLocutorDos.ItemsSource = null;
                    lbLocutorDos.Visibility = Visibility.Collapsed;
                }
            }

            if (programa.DiasOperacion[0].EstadoDia == 1)
            {
                chkSiLunes.IsChecked = true;
            }
            else
            {
                chkNoLunes.IsChecked = true;
            }
            tbLunesHoraInicio.Text = timeSpanAString(programa.DiasOperacion[0].HoraInicio);
            tbLunesHoraFinal.Text = timeSpanAString(programa.DiasOperacion[0].HoraFinal);
            indexPatronSeleccionado(programa.DiasOperacion[0].IdPatron, cbLunesPatron);

            if (programa.DiasOperacion[1].EstadoDia == 1)
            {
                chkSiMartes.IsChecked = true;
            }
            else
            {
                chkNoMartes.IsChecked = true;
            }
            tbMartesHoraInicio.Text = timeSpanAString(programa.DiasOperacion[1].HoraInicio);
            tbMartesHoraFinal.Text = timeSpanAString(programa.DiasOperacion[1].HoraFinal);
            indexPatronSeleccionado(programa.DiasOperacion[1].IdPatron, cbMartesPatron);

            if (programa.DiasOperacion[2].EstadoDia == 1)
            {
                chkSimiercoles.IsChecked = true;
            }
            else
            {
                chkNomiercoles.IsChecked = true;
            }
            tbMiercolesHoraInicio.Text = timeSpanAString(programa.DiasOperacion[2].HoraInicio);
            tbMiercolesHoraFinal.Text = timeSpanAString(programa.DiasOperacion[2].HoraFinal);
            indexPatronSeleccionado(programa.DiasOperacion[2].IdPatron, cbMiercolesPatron);

            if (programa.DiasOperacion[3].EstadoDia == 1)
            {
                chkSiJueves.IsChecked = true;
            }
            else
            {
                chkNoJueves.IsChecked = true;
            }
            tbJuevesHoraInicio.Text = timeSpanAString(programa.DiasOperacion[3].HoraInicio);
            tbJuevesHoraFinal.Text = timeSpanAString(programa.DiasOperacion[3].HoraFinal);
            indexPatronSeleccionado(programa.DiasOperacion[3].IdPatron, cbJuevesPatron);

            if (programa.DiasOperacion[4].EstadoDia == 1)
            {
                chkSiViernes.IsChecked = true;
            }
            else
            {
                chkNoViernes.IsChecked = true;
            }
            tbViernesHoraInicio.Text = timeSpanAString(programa.DiasOperacion[4].HoraInicio);
            tbViernesHoraFinal.Text = timeSpanAString(programa.DiasOperacion[4].HoraFinal);
            indexPatronSeleccionado(programa.DiasOperacion[4].IdPatron, cbViernesPatron);

            if (programa.DiasOperacion[5].EstadoDia == 1)
            {
                chkSiSabado.IsChecked = true;
            }
            else
            {
                chkNoSabado.IsChecked = true;
            }
            tbSabadoHoraInicio.Text = timeSpanAString(programa.DiasOperacion[5].HoraInicio);
            tbSabadoHoraFinal.Text = timeSpanAString(programa.DiasOperacion[5].HoraFinal);
            indexPatronSeleccionado(programa.DiasOperacion[5].IdPatron, cbSabadoPatron);

            if (programa.DiasOperacion[6].EstadoDia == 1)
            {
                chkSiDomingo.IsChecked = true;
            }
            else
            {
                chkNoDomingo.IsChecked = true;
            }
            tbDomingoHoraInicio.Text = timeSpanAString(programa.DiasOperacion[6].HoraInicio);
            tbDomingoHoraFinal.Text = timeSpanAString(programa.DiasOperacion[6].HoraFinal);
            indexPatronSeleccionado(programa.DiasOperacion[2].IdPatron, cbDomingoPatron);

            for (int i = 0; i < 7; i++)
            {
                int cantidadDeCanciones = programa.DiasOperacion[i].NumCanciones;

                if (cantidadDeCanciones > 0)
                {
                    numCaciones[i] = cantidadDeCanciones;
                    listaCaciones[i] = programa.DiasOperacion[i].CancionesProgramaPatron;
                }
            }

        }

        private void visualizarPrograma()
        {
            btnGestionLocutores.Visibility = Visibility.Hidden;
            btnSubirImagen.Visibility = Visibility.Hidden;
            btnRegistrarVisualizar.Visibility = Visibility.Hidden;
            btnCancelarVisualizar.Visibility = Visibility.Hidden;
            btnCerrar.Visibility = Visibility.Visible;

            imgLogo.Source = imgByteImage(programaVisualizar.Imagen);

            tbClave.Text = programaVisualizar.IdPrograma.ToString();
            tbClave.IsEnabled = false;

            tbRadio.Text = programaVisualizar.NombreRadio;
            tbRadio.IsEnabled = false;

            tbPrograma.Text = programaVisualizar.NombrePrograma;
            tbPrograma.IsEnabled = false;

            estaActivo.Visibility = Visibility.Hidden;
            Inactivo.Visibility = Visibility.Hidden;
            EstadoRegistro.Content = programaVisualizar.EstadoProgramaNombre;


            if (programaVisualizar.Locutor.Count > 0)
            {
                tbLocutorUno.Text = "";
                tbLocutorUno.IsEnabled = false;
                tbLocutorDos.Text = "";
                tbLocutorDos.IsEnabled = false;
            }
            if (programaVisualizar.Locutor.Count > 0)
            {

                tbLocutorUno.Text = programaVisualizar.Locutor[0].NombreLocutor;
                tbLocutorUno.IsEnabled = false;
                lbLocutorUno.ItemsSource = null;
                lbLocutorUno.Visibility = Visibility.Collapsed;

                if (programaVisualizar.Locutor.Count > 1)
                {
                    tbLocutorDos.Text = programaVisualizar.Locutor[1].NombreLocutor;
                    tbLocutorDos.IsEnabled = false;
                    lbLocutorDos.ItemsSource = null;
                    lbLocutorDos.Visibility = Visibility.Collapsed;
                }

            }

            chkSiLunes.IsEnabled = false;
            chkNoLunes.IsEnabled = false;
            if (programaVisualizar.DiasOperacion[0].EstadoDia == 1)
            {
                chkSiLunes.IsChecked = true;
                tbLunesHoraInicio.IsReadOnly = true;
                tbLunesHoraFinal.IsReadOnly = true;
                cbLunesPatron.IsEnabled = false;
            }
            else
            {
                chkNoLunes.IsChecked = true;
            }
            tbLunesHoraInicio.Text = timeSpanAString(programaVisualizar.DiasOperacion[0].HoraInicio);
            tbLunesHoraFinal.Text = timeSpanAString(programaVisualizar.DiasOperacion[0].HoraFinal);
            indexPatronSeleccionado(programaVisualizar.DiasOperacion[0].IdPatron, cbLunesPatron);

            chkSiMartes.IsEnabled = false;
            chkNoMartes.IsEnabled = false;
            if (programaVisualizar.DiasOperacion[1].EstadoDia == 1)
            {
                chkSiMartes.IsChecked = true;
                tbMartesHoraInicio.IsReadOnly = true;
                tbMartesHoraFinal.IsReadOnly = true;
                cbMartesPatron.IsEnabled = false;
            }
            else
            {
                chkNoMartes.IsChecked = true;
            }
            tbMartesHoraInicio.Text = timeSpanAString(programaVisualizar.DiasOperacion[1].HoraInicio);
            tbMartesHoraFinal.Text = timeSpanAString(programaVisualizar.DiasOperacion[1].HoraFinal);
            indexPatronSeleccionado(programaVisualizar.DiasOperacion[1].IdPatron, cbMartesPatron);

            chkSimiercoles.IsEnabled = false;
            chkNomiercoles.IsEnabled = false;
            if (programaVisualizar.DiasOperacion[2].EstadoDia == 1)
            {
                chkSimiercoles.IsChecked = true;
                tbMiercolesHoraInicio.IsReadOnly = true;
                tbMiercolesHoraFinal.IsReadOnly = true;
                cbMiercolesPatron.IsEnabled = false;
            }
            else
            {
                chkNomiercoles.IsChecked = true;
            }
            tbMiercolesHoraInicio.Text = timeSpanAString(programaVisualizar.DiasOperacion[2].HoraInicio);
            tbMiercolesHoraFinal.Text = timeSpanAString(programaVisualizar.DiasOperacion[2].HoraFinal);
            indexPatronSeleccionado(programaVisualizar.DiasOperacion[2].IdPatron, cbMiercolesPatron);

            chkSiJueves.IsEnabled = false;
            chkNoJueves.IsEnabled = false;
            if (programaVisualizar.DiasOperacion[3].EstadoDia == 1)
            {
                chkSiJueves.IsChecked = true;
                tbJuevesHoraInicio.IsReadOnly = true;
                tbJuevesHoraFinal.IsReadOnly = true;
                cbJuevesPatron.IsEnabled = false;
            }
            else
            {
                chkNoJueves.IsChecked = true;
            }
            tbJuevesHoraInicio.Text = timeSpanAString(programaVisualizar.DiasOperacion[3].HoraInicio);
            tbJuevesHoraFinal.Text = timeSpanAString(programaVisualizar.DiasOperacion[3].HoraFinal);
            indexPatronSeleccionado(programaVisualizar.DiasOperacion[3].IdPatron, cbJuevesPatron);

            chkSiViernes.IsEnabled = false;
            chkNoViernes.IsEnabled = false;
            if (programaVisualizar.DiasOperacion[4].EstadoDia == 1)
            {
                chkSiViernes.IsChecked = true;
                tbViernesHoraInicio.IsReadOnly = true;
                tbViernesHoraFinal.IsReadOnly = true;
                cbViernesPatron.IsEnabled = false;
            }
            else
            {
                chkNoViernes.IsChecked = true;
            }
            tbViernesHoraInicio.Text = timeSpanAString(programaVisualizar.DiasOperacion[4].HoraInicio);
            tbViernesHoraFinal.Text = timeSpanAString(programaVisualizar.DiasOperacion[4].HoraFinal);
            indexPatronSeleccionado(programaVisualizar.DiasOperacion[4].IdPatron, cbViernesPatron);

            chkSiSabado.IsEnabled = false;
            chkNoSabado.IsEnabled = false;
            if (programaVisualizar.DiasOperacion[5].EstadoDia == 1)
            {
                chkSiSabado.IsChecked = true;
                tbSabadoHoraInicio.IsReadOnly = true;
                tbSabadoHoraFinal.IsReadOnly = true;
                cbSabadoPatron.IsEnabled = false;
            }
            else
            {
                chkNoSabado.IsChecked = true;
            }
            tbSabadoHoraInicio.Text = timeSpanAString(programaVisualizar.DiasOperacion[5].HoraInicio);
            tbSabadoHoraFinal.Text = timeSpanAString(programaVisualizar.DiasOperacion[5].HoraFinal);
            indexPatronSeleccionado(programaVisualizar.DiasOperacion[5].IdPatron, cbSabadoPatron);

            chkSiDomingo.IsEnabled = false;
            chkNoDomingo.IsEnabled = false;
            if (programaVisualizar.DiasOperacion[6].EstadoDia == 1)
            {
                chkSiDomingo.IsChecked = true;
                tbDomingoHoraInicio.IsReadOnly = true;
                tbDomingoHoraFinal.IsReadOnly = true;
                cbDomingoPatron.IsEnabled = false;
            }
            else
            {
                chkNoDomingo.IsChecked = true;
            }
            tbDomingoHoraInicio.Text = timeSpanAString(programaVisualizar.DiasOperacion[6].HoraInicio);
            tbDomingoHoraFinal.Text = timeSpanAString(programaVisualizar.DiasOperacion[6].HoraFinal);
            indexPatronSeleccionado(programaVisualizar.DiasOperacion[6].IdPatron, cbDomingoPatron);

            for (int i = 0; i < 7; i++)
            {
                int cantidadDeCanciones = programaVisualizar.DiasOperacion[i].NumCanciones;

                if (cantidadDeCanciones > 0)
                {
                    numCaciones[i] = cantidadDeCanciones;
                    listaCaciones[i] = programaVisualizar.DiasOperacion[i].CancionesProgramaPatron;
                }
            }
        }

        private void btnRegistrar(object sender, RoutedEventArgs e)
        {
            String programaText = tbPrograma.Text;
            String locutorUno = tbLocutorUno.Text;
            String locutorDos = tbLocutorDos.Text;

            int intCbLunesPatron = cbLunesPatron.SelectedIndex;
            int intCbMartesPatron = cbMartesPatron.SelectedIndex;
            int intCbMiercolesPatron = cbMiercolesPatron.SelectedIndex;
            int intCbJuevesPatron = cbJuevesPatron.SelectedIndex;
            int intCbViernesPatron = cbViernesPatron.SelectedIndex;
            int intCbSabadoPatron = cbSabadoPatron.SelectedIndex;
            int intCbDomingoPatron = cbDomingoPatron.SelectedIndex;
            int intLunesIdPatron = -1;
            int intMartesIdPatron = -1;
            int intMiercolesIdPatron = -1;
            int intJuevesIdPatron = -1;
            int intViernesIdPatron = -1;
            int intSabadoIdPatron = -1;
            int intDomingoIdPatron = -1;

            String lunesHorarioInicio = tbLunesHoraInicio.Text;
            String lunesHorarioFinal = tbLunesHoraFinal.Text;
            String martesHorarioInicio = tbMartesHoraInicio.Text;
            String martesHorarioFinal = tbMartesHoraFinal.Text;
            String miercolesHorarioInicio = tbMiercolesHoraInicio.Text;
            String miercolesHorarioFinal = tbMiercolesHoraFinal.Text;
            String juevesHorarioInicio = tbJuevesHoraInicio.Text;
            String juevesHorarioFinal = tbJuevesHoraFinal.Text;
            String viernesHorarioInicio = tbViernesHoraInicio.Text;
            String viernesHorarioFinal = tbViernesHoraFinal.Text;
            String sabadoHorarioInicio = tbSabadoHoraInicio.Text;
            String sabadoHorarioFinal = tbSabadoHoraFinal.Text;
            String domingoHorarioInicio = tbDomingoHoraInicio.Text;
            String domingoHorarioFinal = tbDomingoHoraFinal.Text;

            Boolean registrarLunes;
            Boolean registrarMartes;
            Boolean registrarMiercoles;
            Boolean registrarJueves;
            Boolean registrarViernes;
            Boolean registrarSabado;
            Boolean registrarDomingo;

            Boolean camposValidos = true;


            if (programaText.Equals(""))
            {
                MessageBox.Show("Favor de verificar el campo Programa", "Campo vacio");
                camposValidos = false;
            }

            if (!locutorUno.Equals("") && !locutorDos.Equals(""))
            {
                if (locutorUno.ToLower().Equals(locutorDos.ToLower()))
                {
                    MessageBox.Show("Un locutor no puede estar registrado dos veces en el mis programa", "Locutor Duplicado");
                    camposValidos = false;
                }
            }

            if (diaLunesActivo && camposValidos)
            {
                registrarLunes = validarDatosDias("Lunes", lunesHorarioInicio, lunesHorarioFinal, intCbLunesPatron, idProgramaValidarHorarios);

                if (!registrarLunes)
                {
                    camposValidos = false;
                }
                else
                {
                    var patronLunes = (Patron)cbLunesPatron.SelectedItem;
                    intLunesIdPatron = patronLunes.IdPatron;

                    if (lunesHorarioInicio.Equals("24:00"))
                    {
                        lunesHorarioInicio = "23:59";
                    }
                    if (lunesHorarioFinal.Equals("24:00"))
                    {
                        lunesHorarioFinal = "23:59";
                    }
                }
            }
            if (diaMartesActivo && camposValidos)
            {
                registrarMartes = validarDatosDias("Martes", martesHorarioInicio, martesHorarioFinal, intCbMartesPatron, idProgramaValidarHorarios);

                if (!registrarMartes)
                {
                    camposValidos = false;
                }
                else
                {
                    var patronMartes = (Patron)cbMartesPatron.SelectedItem;
                    intMartesIdPatron = patronMartes.IdPatron;
                    if (martesHorarioInicio.Equals("24:00"))
                    {
                        martesHorarioInicio = "23:59";
                    }
                    if (martesHorarioFinal.Equals("24:00"))
                    {
                        martesHorarioFinal = "23:59";
                    }

                }
            }
            if (diaMiercolesActivo && camposValidos)
            {
                registrarMiercoles = validarDatosDias("Miercoles", miercolesHorarioInicio, miercolesHorarioFinal, intCbMiercolesPatron, idProgramaValidarHorarios);

                if (!registrarMiercoles)
                {
                    camposValidos = false;
                }
                else
                {
                    var patronMiercoles = (Patron)cbMiercolesPatron.SelectedItem;
                    intMiercolesIdPatron = patronMiercoles.IdPatron;

                    if (miercolesHorarioInicio.Equals("24:00"))
                    {
                        miercolesHorarioInicio = "23:59";
                    }
                    if (miercolesHorarioFinal.Equals("24:00"))
                    {
                        miercolesHorarioFinal = "23:59";
                    }

                }
            }
            if (diaJuevesActivo && camposValidos)
            {
                registrarJueves = validarDatosDias("Jueves", juevesHorarioInicio, juevesHorarioFinal, intCbJuevesPatron, idProgramaValidarHorarios);

                if (!registrarJueves)
                {
                    camposValidos = false;
                }
                else
                {
                    var patronJueves = (Patron)cbJuevesPatron.SelectedItem;
                    intJuevesIdPatron = patronJueves.IdPatron;

                    if (juevesHorarioInicio.Equals("24:00"))
                    {
                        juevesHorarioInicio = "23:59";
                    }
                    if (juevesHorarioFinal.Equals("24:00"))
                    {
                        juevesHorarioFinal = "23:59";
                    }

                }
            }
            if (diaViernesActivo && camposValidos)
            {
                registrarViernes = validarDatosDias("Viernes", viernesHorarioInicio, viernesHorarioFinal, intCbViernesPatron, idProgramaValidarHorarios);

                if (!registrarViernes)
                {
                    camposValidos = false;
                }
                else
                {
                    var patronViernes = (Patron)cbViernesPatron.SelectedItem;
                    intViernesIdPatron = patronViernes.IdPatron;

                    if (viernesHorarioInicio.Equals("24:00"))
                    {
                        viernesHorarioInicio = "23:59";
                    }
                    if (viernesHorarioFinal.Equals("24:00"))
                    {
                        viernesHorarioFinal = "23:59";
                    }
                }
            }
            if (diaSabadoActivo && camposValidos)
            {
                registrarSabado = validarDatosDias("Sabado", sabadoHorarioInicio, sabadoHorarioFinal, intCbSabadoPatron, idProgramaValidarHorarios);

                if (!registrarSabado)
                {
                    camposValidos = false;
                }
                else
                {
                    var patronSabado = (Patron)cbSabadoPatron.SelectedItem;
                    intSabadoIdPatron = patronSabado.IdPatron;
                    if (sabadoHorarioInicio.Equals("24:00"))
                    {
                        sabadoHorarioInicio = "23:59";
                    }
                    if (sabadoHorarioFinal.Equals("24:00"))
                    {
                        sabadoHorarioFinal = "23:59";
                    }
                }
            }
            if (diaDomingoActivo && camposValidos)
            {
                registrarDomingo = validarDatosDias("Domingo", domingoHorarioInicio, domingoHorarioFinal, intCbDomingoPatron, idProgramaValidarHorarios);

                if (!registrarDomingo)
                {
                    camposValidos = false;
                }
                else
                {
                    var patronDomingo = (Patron)cbDomingoPatron.SelectedItem;
                    intDomingoIdPatron = patronDomingo.IdPatron;
                    if (domingoHorarioInicio.Equals("24:00"))
                    {
                        domingoHorarioInicio = "23:59";
                    }
                    if (domingoHorarioFinal.Equals("24:00"))
                    {
                        domingoHorarioFinal = "23:59";
                    }

                }
            }

            if (esNuevo && camposValidos)
            {
                int estado = 1;
                int idRadioObtenido = radio.IdRadio;


                byte[] imagenGuardar = imageData;

                if (imagenGuardar != null || imagenGuardar.Length == 0)
                {
                    var uriImg = new BitmapImage(new Uri(@"../RECURSOS/logo.png", UriKind.RelativeOrAbsolute));
                    imagAuxiliar.Source = uriImg;
                    imageData = getPNGFromImageControl(imgLogo.Source as BitmapImage);
                }

                diaActivo[0] = diaLunesActivo;
                idDia[0] = 0;
                numeroDia[0] = 1;
                horaInicio[0] = TimeSpan.Parse(lunesHorarioInicio);
                horaFinal[0] = TimeSpan.Parse(lunesHorarioFinal);
                estadoDia[0] = diaLunesActivo;
                idPatron[0] = intLunesIdPatron;
                claveProgramaDia[0] = 0;


                diaActivo[1] = diaMartesActivo;
                idDia[1] = 1;
                numeroDia[1] = 2;
                horaInicio[1] = TimeSpan.Parse(martesHorarioInicio);
                horaFinal[1] = TimeSpan.Parse(martesHorarioFinal);
                estadoDia[1] = diaMartesActivo;
                idPatron[1] = intMartesIdPatron;
                claveProgramaDia[1] = 0;

                diaActivo[2] = diaMiercolesActivo;
                idDia[2] = 0;
                numeroDia[2] = 3;
                horaInicio[2] = TimeSpan.Parse(miercolesHorarioInicio);
                horaFinal[2] = TimeSpan.Parse(miercolesHorarioFinal);
                estadoDia[2] = diaMiercolesActivo;
                idPatron[2] = intMiercolesIdPatron;
                claveProgramaDia[2] = 0;

                diaActivo[3] = diaJuevesActivo;
                idDia[3] = 0;
                numeroDia[3] = 4;
                horaInicio[3] = TimeSpan.Parse(juevesHorarioInicio);
                horaFinal[3] = TimeSpan.Parse(juevesHorarioFinal);
                estadoDia[3] = diaJuevesActivo;
                idPatron[3] = intJuevesIdPatron;
                claveProgramaDia[3] = 0;

                diaActivo[4] = diaViernesActivo;
                idDia[4] = 0;
                numeroDia[4] = 5;
                horaInicio[4] = TimeSpan.Parse(viernesHorarioInicio);
                horaFinal[4] = TimeSpan.Parse(viernesHorarioFinal);
                estadoDia[4] = diaViernesActivo;
                idPatron[4] = intViernesIdPatron;
                claveProgramaDia[4] = 0;

                diaActivo[5] = diaSabadoActivo;
                idDia[5] = 0;
                numeroDia[5] = 6;
                horaInicio[5] = TimeSpan.Parse(sabadoHorarioInicio);
                horaFinal[5] = TimeSpan.Parse(sabadoHorarioFinal);
                estadoDia[5] = diaSabadoActivo;
                idPatron[5] = intSabadoIdPatron;
                claveProgramaDia[5] = 0;

                diaActivo[6] = diaDomingoActivo;
                idDia[6] = 0;
                numeroDia[6] = 7;
                horaInicio[6] = TimeSpan.Parse(domingoHorarioInicio);
                horaFinal[6] = TimeSpan.Parse(domingoHorarioFinal);
                estadoDia[6] = diaDomingoActivo;
                idPatron[6] = intDomingoIdPatron;
                claveProgramaDia[6] = 0;



                int resultado = DAOPrograma.setReporte(programaText, estado, imagenGuardar, idRadioObtenido, idLocutorUno, idLocutorDos,
                                                       diaActivo, idDia, numCaciones, numeroDia, horaInicio, horaFinal, estadoDia, idPatron,
                                                       claveProgramaDia, listaCaciones);

                if (resultado == 1)
                {
                    MessageBox.Show("Se registro el programa '" + programaText + "'", "Registro");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No se puedo realizar el registro", "Registro");
                    this.Close();
                }
            }

            if (!esNuevo && camposValidos)
            {

                int idProgramaRadio = programa.IdPrograma;
                int estado = estadoPrograma;
                int idRadioObtenido = programa.IdRadio;




                if (imageData == null)
                {
                    imageData = programa.Imagen;
                }

                byte[] imagenGuardar = imageData;

                foreach (Locutor locutor in DAOLocutor.getLocutor())
                {
                    if (!locutorUno.ToLower().Equals(locutor.NombreLocutor))
                    {
                        if (locutor.IdPrograma == programa.IdPrograma)
                        {
                            DAOLocutor.eliminarNombrelocutor(locutor.IdLocutor);
                        }
                    }
                }

                diaActivo[0] = diaLunesActivo;
                idDia[0] = programa.DiasOperacion[0].IdDia;
                numeroDia[0] = 1;
                horaInicio[0] = TimeSpan.Parse(lunesHorarioInicio);
                horaFinal[0] = TimeSpan.Parse(lunesHorarioFinal);
                estadoDia[0] = diaLunesActivo;
                idPatron[0] = intLunesIdPatron;
                claveProgramaDia[0] = programa.DiasOperacion[0].IdProgramaDia;

                diaActivo[1] = diaMartesActivo;
                idDia[1] = programa.DiasOperacion[1].IdDia;
                numeroDia[1] = 2;
                horaInicio[1] = TimeSpan.Parse(martesHorarioInicio);
                horaFinal[1] = TimeSpan.Parse(martesHorarioFinal);
                estadoDia[1] = diaMartesActivo;
                idPatron[1] = intMartesIdPatron;
                claveProgramaDia[1] = programa.DiasOperacion[1].IdProgramaDia;

                diaActivo[2] = diaMiercolesActivo;
                idDia[2] = programa.DiasOperacion[2].IdDia;
                numeroDia[2] = 3;
                horaInicio[2] = TimeSpan.Parse(miercolesHorarioInicio);
                horaFinal[2] = TimeSpan.Parse(miercolesHorarioFinal);
                estadoDia[2] = diaMiercolesActivo;
                idPatron[2] = intMiercolesIdPatron;
                claveProgramaDia[2] = programa.DiasOperacion[2].IdProgramaDia;

                diaActivo[3] = diaJuevesActivo;
                idDia[3] = programa.DiasOperacion[3].IdDia;
                numeroDia[3] = 4;
                horaInicio[3] = TimeSpan.Parse(juevesHorarioInicio);
                horaFinal[3] = TimeSpan.Parse(juevesHorarioFinal);
                estadoDia[3] = diaJuevesActivo;
                idPatron[3] = intJuevesIdPatron;
                claveProgramaDia[3] = programa.DiasOperacion[3].IdProgramaDia;

                diaActivo[4] = diaViernesActivo;
                idDia[4] = programa.DiasOperacion[4].IdDia;
                numeroDia[4] = 5;
                horaInicio[4] = TimeSpan.Parse(viernesHorarioInicio);
                horaFinal[4] = TimeSpan.Parse(viernesHorarioFinal);
                estadoDia[4] = diaViernesActivo;
                idPatron[4] = intViernesIdPatron;
                claveProgramaDia[4] = programa.DiasOperacion[4].IdProgramaDia;

                diaActivo[5] = diaSabadoActivo;
                idDia[5] = programa.DiasOperacion[5].IdDia;
                numeroDia[5] = 6;
                horaInicio[5] = TimeSpan.Parse(sabadoHorarioInicio);
                horaFinal[5] = TimeSpan.Parse(sabadoHorarioFinal);
                estadoDia[5] = diaSabadoActivo;
                idPatron[5] = intSabadoIdPatron;
                claveProgramaDia[5] = programa.DiasOperacion[5].IdProgramaDia;

                diaActivo[6] = diaDomingoActivo;
                idDia[6] = programa.DiasOperacion[6].IdDia;
                numeroDia[6] = 7;
                horaInicio[6] = TimeSpan.Parse(domingoHorarioInicio);
                horaFinal[6] = TimeSpan.Parse(domingoHorarioFinal);
                estadoDia[6] = diaDomingoActivo;
                idPatron[6] = intDomingoIdPatron;
                claveProgramaDia[6] = programa.DiasOperacion[6].IdProgramaDia;



                int resultado = DAOPrograma.updateReporte(idProgramaRadio, programaText, estado, imagenGuardar, idRadioObtenido, idLocutorUno, idLocutorDos,
                                                       diaActivo, idDia, numCaciones, numeroDia, horaInicio, horaFinal, estadoDia, idPatron, claveProgramaDia,
                                                       listaCaciones);

                if (resultado == 1)
                {
                    MessageBox.Show("Se actualizo el programa '" + programaText + "'", "Registro");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No se puedo actualiza el programa '" + programaText + "'", "Registro");
                    this.Close();
                }
            }

        }

        private void btnGestionarLocutores(object sender, RoutedEventArgs e)
        {
            GUI_GestionarLocutores gestionarLocutores = new GUI_GestionarLocutores();
            gestionarLocutores.ShowDialog();
        }

        public void gestionarLocutores()
        {

            tbLocutorUno.TextChanged += new TextChangedEventHandler(tbLocutorUno_TextChanged);
            lbLocutorUno.SelectionChanged += new SelectionChangedEventHandler(lbLocutorUno_SelectionChanged);
            tbLocutorDos.TextChanged += new TextChangedEventHandler(tbLocutorDos_TextChanged);
            lbLocutorDos.SelectionChanged += new SelectionChangedEventHandler(lbLocutorDos_SelectionChanged);

        }

        private void btnCancelar(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCerrarVisualizar(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void diaOperacionesDesabilitado()
        {

            var uriImg = new BitmapImage(new Uri(@"../RECURSOS/logo.png", UriKind.RelativeOrAbsolute));
            imgLogo.Source = uriImg;
            imageData = getPNGFromImageControl(uriImg);

            tbClave.IsEnabled = false;
            tbClave.Text = DAOPrograma.ultimoIdRegistrado().ToString();

            tbRadio.IsEnabled = false;
            radio = DAORadio.unicoRadioExistente();
            tbRadio.Text = radio.Nombre;

            chkNoLunes.IsChecked = true;
            chkNoMartes.IsChecked = true;
            chkNomiercoles.IsChecked = true;
            chkNoJueves.IsChecked = true;
            chkNoViernes.IsChecked = true;
            chkNoSabado.IsChecked = true;
            chkNoDomingo.IsChecked = true;
            estaActivo.Visibility = Visibility.Hidden;
            Inactivo.Visibility = Visibility.Hidden;
            EstadoRegistro.Content = "En Registro";

            mostrarSugerenciaHoras(tbLunesHoraInicio);
            mostrarSugerenciaHoras(tbLunesHoraFinal);
            mostrarSugerenciaHoras(tbMartesHoraInicio);
            mostrarSugerenciaHoras(tbMartesHoraFinal);
            mostrarSugerenciaHoras(tbMiercolesHoraInicio);
            mostrarSugerenciaHoras(tbMiercolesHoraFinal);
            mostrarSugerenciaHoras(tbJuevesHoraInicio);
            mostrarSugerenciaHoras(tbJuevesHoraFinal);
            mostrarSugerenciaHoras(tbViernesHoraInicio);
            mostrarSugerenciaHoras(tbViernesHoraFinal);
            mostrarSugerenciaHoras(tbSabadoHoraInicio);
            mostrarSugerenciaHoras(tbSabadoHoraFinal);
            mostrarSugerenciaHoras(tbDomingoHoraInicio);
            mostrarSugerenciaHoras(tbDomingoHoraFinal);

        }

        private void rbCheckLunes(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Content.Equals("No"))
            {
                tbLunesHoraInicio.IsEnabled = false;
                tbLunesHoraFinal.IsEnabled = false;
                cbLunesPatron.IsEnabled = false;

                diaLunesActivo = false;
            }
            else
            {
                tbLunesHoraInicio.IsEnabled = true;
                tbLunesHoraFinal.IsEnabled = true;
                cbLunesPatron.IsEnabled = true;

                diaLunesActivo = true;
            }
        }

        private void rbCheckMartes(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Content.Equals("No"))
            {
                tbMartesHoraInicio.IsEnabled = false;
                tbMartesHoraFinal.IsEnabled = false;
                cbMartesPatron.IsEnabled = false;

                diaMartesActivo = false;
            }
            else
            {
                tbMartesHoraInicio.IsEnabled = true;
                tbMartesHoraFinal.IsEnabled = true;
                cbMartesPatron.IsEnabled = true;

                diaMartesActivo = true;
            }
        }

        private void rbCheckMiercoles(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Content.Equals("No"))
            {
                tbMiercolesHoraInicio.IsEnabled = false;
                tbMiercolesHoraFinal.IsEnabled = false;
                cbMiercolesPatron.IsEnabled = false;

                diaMiercolesActivo = false;
            }
            else
            {
                tbMiercolesHoraInicio.IsEnabled = true;
                tbMiercolesHoraFinal.IsEnabled = true;
                cbMiercolesPatron.IsEnabled = true;

                diaMiercolesActivo = true;

            }
        }

        private void rbCheckJueves(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Content.Equals("No"))
            {
                tbJuevesHoraInicio.IsEnabled = false;
                tbJuevesHoraFinal.IsEnabled = false;
                cbJuevesPatron.IsEnabled = false;

                diaJuevesActivo = false;

            }
            else
            {
                tbJuevesHoraInicio.IsEnabled = true;
                tbJuevesHoraFinal.IsEnabled = true;
                cbJuevesPatron.IsEnabled = true;

                diaJuevesActivo = true;
            }
        }

        private void rbCheckViernes(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Content.Equals("No"))
            {
                tbViernesHoraInicio.IsEnabled = false;
                tbViernesHoraFinal.IsEnabled = false;
                cbViernesPatron.IsEnabled = false;

                diaViernesActivo = false;
            }
            else
            {
                tbViernesHoraInicio.IsEnabled = true;
                tbViernesHoraFinal.IsEnabled = true;
                cbViernesPatron.IsEnabled = true;

                diaViernesActivo = true;
            }
        }

        private void rbCheckSabado(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Content.Equals("No"))
            {
                tbSabadoHoraInicio.IsEnabled = false;
                tbSabadoHoraFinal.IsEnabled = false;
                cbSabadoPatron.IsEnabled = false;

                diaSabadoActivo = false;
            }
            else
            {
                tbSabadoHoraInicio.IsEnabled = true;
                tbSabadoHoraFinal.IsEnabled = true;
                cbSabadoPatron.IsEnabled = true;

                diaSabadoActivo = true;
            }
        }

        private void rbCheckDomingo(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Content.Equals("No"))
            {
                tbDomingoHoraInicio.IsEnabled = false;
                tbDomingoHoraFinal.IsEnabled = false;
                cbDomingoPatron.IsEnabled = false;

                diaDomingoActivo = false;
            }
            else
            {
                tbDomingoHoraInicio.IsEnabled = true;
                tbDomingoHoraFinal.IsEnabled = true;
                cbDomingoPatron.IsEnabled = true;

                diaDomingoActivo = true;
            }
        }

        private void rbCheckedActivo(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Content.Equals("Activo"))
            {
                estadoPrograma = 1;
            }
            else
            {
                estadoPrograma = 0;
            }
        }

        public Boolean validarDatosDias(String dia, String horaInicio, String horaFinal, int indexPatron, int idprograma)
        {
            int resultadoHoraInicio = 0;
            int resultadoHoraFinal = 0;

            if (horaInicio.Length == 5)
            {
                if (int.Parse(horaInicio[0].ToString()) >= 0 && int.Parse(horaInicio[0].ToString()) <= 2)
                {
                    if (int.Parse(horaInicio[1].ToString()) >= 0 && int.Parse(horaInicio[1].ToString()) <= 9)
                    {
                        if (horaInicio[2].ToString().Equals(":"))
                        {
                            if (int.Parse(horaInicio[3].ToString()) >= 0 && int.Parse(horaInicio[3].ToString()) <= 5)
                            {
                                if (int.Parse(horaInicio[4].ToString()) >= 0 && int.Parse(horaInicio[4].ToString()) <= 9)
                                {
                                    resultadoHoraInicio = 1;
                                }
                            }
                        }
                    }
                }
            }
            if (horaFinal.Length == 5)
            {
                if (int.Parse(horaFinal[0].ToString()) >= 0 && int.Parse(horaFinal[0].ToString()) <= 2)
                {
                    if (int.Parse(horaFinal[1].ToString()) >= 0 && int.Parse(horaFinal[1].ToString()) <= 9)
                    {
                        if (horaFinal[2].ToString().Equals(":"))
                        {
                            if (int.Parse(horaFinal[3].ToString()) >= 0 && int.Parse(horaFinal[3].ToString()) <= 5)
                            {
                                if (int.Parse(horaFinal[4].ToString()) >= 0 && int.Parse(horaFinal[4].ToString()) <= 9)
                                {
                                    resultadoHoraFinal = 1;
                                }
                            }
                        }
                    }
                }
            }

            if (resultadoHoraInicio == 0)
            {
                MessageBox.Show("La hora de inicio introducida para el dia:'" + dia + "' es incorrecta", "Horas no validad");
                return false;
            }
            if (resultadoHoraFinal == 0)
            {
                MessageBox.Show("La hora de finalización introducida para el dia:'" + dia + "' es incorrecta", "Horas no validad");
                return false;
            }

            if (resultadoHoraInicio == 1 && resultadoHoraFinal == 1 && indexPatron >= 0 && indexPatron != null)
            {

                Boolean setranslapa = validaHora(dia, horaInicio, horaFinal, idprograma);

                if (setranslapa)
                {
                    MessageBox.Show("Las horas introducidas  dia:'" + dia + "' ya son utilizadas por otro programa. Favor de Veririfcar los horarios", "Horas utilizadas");
                    return false;
                }
                else
                {
                    return true;
                }
            }

            MessageBox.Show("No se esta utilizando un patron para el dia '" + dia + "'. Debe escojer un patron", "Patron no seleccionado");
            return false;
        }

        public void mostrarSugerenciaHoras(TextBox textBox)
        {

            textBox.Foreground = Brushes.Gray;
            textBox.Text = "00:00";
            textBox.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(tb_GotKeyboardFocus);
            textBox.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(tb_LostKeyboardFocus);
        }

        private void tb_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox)
            {
                if (((TextBox)sender).Foreground == Brushes.Gray)
                {
                    ((TextBox)sender).Text = "";
                    ((TextBox)sender).Foreground = Brushes.Black;
                }
            }
        }

        private void tb_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox)
            {
                if (((TextBox)sender).Text.Trim().Equals(""))
                {
                    ((TextBox)sender).Foreground = Brushes.Gray;
                    ((TextBox)sender).Text = "00:00";
                }
            }
        }

        private void btnCargarImagen(object sender, RoutedEventArgs e)
        {
            Stream stream;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos PNG|*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    if ((stream = openFileDialog.OpenFile()) != null)
                    {
                        using (stream)
                        {
                            imageData = new byte[stream.Length];
                            stream.Read(imageData, 0, (int)stream.Length);
                            imagenLogo = new MemoryStream(imageData);
                            BitmapImage bitImg = new BitmapImage();
                            bitImg.BeginInit();
                            bitImg.StreamSource = imagenLogo;
                            bitImg.EndInit();
                            imgLogo.Source = bitImg;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: No se puede subir la imagen " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("No se selecciono ninguna imagen, favor de seleccionar una", "Imagen no seleccionada");
            }
        }

        private void tbLocutorUno_TextChanged(object sender, TextChangedEventArgs e)
        {
            String tbBuscar = tbLocutorUno.Text;
            List<Locutor> autolist = new List<Locutor>();

            foreach (Locutor item in locutores)
            {
                if (item.ToString().ToUpper().StartsWith(tbBuscar.ToUpper()) && item.ToString() != null && !item.ToString().Equals(""))
                {
                    autolist.Add(item);
                }
            }
            if (autolist.Count > 0)
            {
                lbLocutorUno.ItemsSource = autolist;
                lbLocutorUno.Visibility = Visibility.Visible;
            }
            if (autolist.Count == 0)
            {
                lbLocutorUno.Visibility = Visibility.Collapsed;
                lbLocutorUno.ItemsSource = null;
            }
            if (tbLocutorUno.Text == "")
            {
                lbLocutorUno.Visibility = Visibility.Collapsed;
                lbLocutorUno.ItemsSource = null;
            }

        }

        private void lbLocutorUno_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbLocutorUno.ItemsSource != null)
            {
                if (lbLocutorUno.SelectedIndex != -1)
                {
                    tbLocutorUno.Text = lbLocutorUno.SelectedItem.ToString();
                    lbLocutorUno.Visibility = Visibility.Collapsed;
                    lbLocutorUno.ItemsSource = null;
                }
            }
        }

        private void tbLocutorDos_TextChanged(object sender, TextChangedEventArgs e)
        {
            String tbBuscar = tbLocutorDos.Text;
            List<Locutor> autolist = new List<Locutor>();

            foreach (Locutor item in locutores)
            {
                if (item.ToString().ToUpper().StartsWith(tbBuscar.ToUpper()) && item.ToString() != null && !item.ToString().Equals(""))
                {
                    autolist.Add(item);
                }
            }
            if (autolist.Count > 0)
            {
                lbLocutorDos.ItemsSource = autolist;
                lbLocutorDos.Visibility = Visibility.Visible;
            }
            if (tbLocutorDos.Text == "")
            {
                lbLocutorDos.Visibility = Visibility.Collapsed;
                lbLocutorDos.ItemsSource = null;
            }
            if (autolist.Count == 0)
            {
                lbLocutorDos.Visibility = Visibility.Collapsed;
                lbLocutorDos.ItemsSource = null;
            }
        }

        private void lbLocutorDos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbLocutorDos.ItemsSource != null)
            {
                if (lbLocutorDos.SelectedIndex != -1)
                {
                    tbLocutorDos.Text = lbLocutorDos.SelectedItem.ToString();
                    lbLocutorDos.Visibility = Visibility.Collapsed;
                    lbLocutorDos.ItemsSource = null;
                }
            }
        }

        private void serPerdioElFocusLocutorUno(object sender, RoutedEventArgs e)
        {
            String tbBuscar = tbLocutorUno.Text;
            List<Locutor> autolist = new List<Locutor>();

            if (!tbBuscar.Equals(""))
            {

                foreach (Locutor item in locutores)
                {
                    if (item.ToString().ToUpper().StartsWith(tbBuscar.ToUpper()) && item.ToString() != null && !item.ToString().Equals(""))
                    {
                        autolist.Add(item);
                    }
                }
                if (autolist.Count == 0)
                {
                    MessageBoxResult opcionSeleccionada = MessageBox.Show("¿Desea registra al locutor: '" + tbBuscar + "' ?", "Confirmacion Registro", MessageBoxButton.OKCancel);
                    if (opcionSeleccionada == MessageBoxResult.OK)
                    {
                        int resultado = DAOLocutor.guardarLocutor(tbBuscar);

                        idLocutorUno = resultado;


                        if (resultado != 0)
                        {
                            MessageBox.Show("Se registro el locutor", "Locutor registrado");
                        }
                        else
                        {
                            MessageBox.Show("No se puedo registrar el locutor", "Locutor no registrado");
                        }

                    }
                    else
                    {
                        tbLocutorUno.Text = "";
                    }

                    locutores = DAOLocutor.getLocutor();
                }
                else
                {
                    if (autolist[0].IdPrograma != -1)
                    {
                        MessageBox.Show("El locutor ya pertenece a un programa. El Locutor sera reasignado", "Locutor con programa");
                        idLocutorUno = autolist[0].IdLocutor;
                    }
                    else
                    {
                        idLocutorUno = autolist[0].IdLocutor;
                    }

                }
            }
        }

        private void serPerdioElFocusLocutorDos(object sender, RoutedEventArgs e)
        {
            String tbBuscar = tbLocutorDos.Text;
            List<Locutor> autolist = new List<Locutor>();

            if (!tbBuscar.Equals(""))
            {

                foreach (Locutor item in locutores)
                {
                    if (item.ToString().ToUpper().StartsWith(tbBuscar.ToUpper()) && item.ToString() != null && !item.ToString().Equals(""))
                    {
                        autolist.Add(item);
                    }
                }
                if (autolist.Count == 0)
                {
                    MessageBoxResult opcionSeleccionada = MessageBox.Show("¿Desea registra al locutor: '" + tbBuscar + "' ?", "Confirmacion Registro", MessageBoxButton.OKCancel);
                    if (opcionSeleccionada == MessageBoxResult.OK)
                    {
                        int resultado = DAOLocutor.guardarLocutor(tbBuscar);

                        idLocutorDos = resultado;

                        if (resultado != 0)
                        {
                            MessageBox.Show("Se registro el locutor", "Locutor registrado");
                        }
                        else
                        {
                            MessageBox.Show("No se puedo registrar el locutor", "Locutor no registrado");
                        }
                    }
                    else
                    {
                        tbLocutorDos.Text = "";
                    }
                }
                else
                {
                    if (autolist[0].IdPrograma != -1)
                    {
                        MessageBox.Show("El locutor ya pertenece a un programa. El Locutor sera reasignado", "Locutor con programa");
                        idLocutorDos = autolist[0].IdLocutor;
                    }
                    else
                    {
                        idLocutorDos = autolist[0].IdLocutor;
                    }
                }
            }
        }

        public byte[] getPNGFromImageControl(BitmapImage imageC)
        {
            MemoryStream memStream = new MemoryStream();
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageC));
            encoder.Save(memStream);
            return memStream.ToArray();
        }

        public BitmapImage imgByteImage(byte[] imagen)
        {
            var ms = new MemoryStream(imagen);
            BitmapImage bitImg = new BitmapImage();
            bitImg.BeginInit();
            bitImg.StreamSource = ms;
            bitImg.EndInit();

            return bitImg;
        }

        public String timeSpanAString(TimeSpan hora)
        {
            TimeSpan fechaVacia = new TimeSpan(00, 00, 00);

            if (hora == fechaVacia)
            {
                return "00:00";
            }
            return DateTime.Today.Add(hora).ToString("HH:mm");
        }

        public void indexPatronSeleccionado(int idPatron, ComboBox comboBox)
        {
            var conbobosx = comboBox;

            Boolean encontrado = true;
            int contador = 0;
            while (encontrado)
            {
                if (patron[contador].IdPatron == idPatron)
                {
                    comboBox.SelectedIndex = contador;
                    encontrado = false;
                }
                contador++;
                if (contador == patron.Count)
                {
                    encontrado = false;
                }
            }
        }

        private void btnVisualizarPatronLunes(object sender, RoutedEventArgs e)
        {
            if (cbLunesPatron.SelectedIndex >= 0)
            {
                patronSeleccionado = (Patron)cbLunesPatron.SelectedItem;

                if (numCaciones[0] == -1)
                {
                    GUI_VisualizarPatron visualizarPatron = new GUI_VisualizarPatron(1, esNuevo, patronSeleccionado);
                    visualizarPatron.Owner = this;
                    visualizarPatron.ShowDialog();
                }
                else
                {
                    GUI_VisualizarPatron visualizarPatron = new GUI_VisualizarPatron(1, esNuevo, patronSeleccionado, numCaciones[0], listaCaciones[0]);
                    visualizarPatron.Owner = this;
                    visualizarPatron.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("No se tiene patron seleccionado", "Error Patron");
            }
        }

        private void btnVisualizarPatronMartes(object sender, RoutedEventArgs e)
        {
            if (cbMartesPatron.SelectedIndex >= 0)
            {
                patronSeleccionado = (Patron)cbMartesPatron.SelectedItem;

                if (numCaciones[1] == -1)
                {
                    GUI_VisualizarPatron visualizarPatron = new GUI_VisualizarPatron(2, esNuevo, patronSeleccionado);
                    visualizarPatron.Owner = this;
                    visualizarPatron.ShowDialog();
                }
                else
                {
                    GUI_VisualizarPatron visualizarPatron = new GUI_VisualizarPatron(2, esNuevo, patronSeleccionado, numCaciones[1], listaCaciones[1]);
                    visualizarPatron.Owner = this;
                    visualizarPatron.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("No se tiene patron seleccionado", "Error Patron");
            }
        }

        private void btnVisualizarPatronMiercoles(object sender, RoutedEventArgs e)
        {
            if (cbMiercolesPatron.SelectedIndex >= 0)
            {
                patronSeleccionado = (Patron)cbMiercolesPatron.SelectedItem;

                if (numCaciones[2] == -1)
                {
                    GUI_VisualizarPatron visualizarPatron = new GUI_VisualizarPatron(3, esNuevo, patronSeleccionado);
                    visualizarPatron.Owner = this;
                    visualizarPatron.ShowDialog();
                }
                else
                {
                    GUI_VisualizarPatron visualizarPatron = new GUI_VisualizarPatron(3, esNuevo, patronSeleccionado, numCaciones[2], listaCaciones[2]);
                    visualizarPatron.Owner = this;
                    visualizarPatron.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("No se tiene patron seleccionado", "Error Patron");
            }
        }

        private void btnVisualizarPatronJueves(object sender, RoutedEventArgs e)
        {
            if (cbJuevesPatron.SelectedIndex >= 0)
            {
                patronSeleccionado = (Patron)cbJuevesPatron.SelectedItem;

                if (numCaciones[3] == -1)
                {
                    GUI_VisualizarPatron visualizarPatron = new GUI_VisualizarPatron(4, esNuevo, patronSeleccionado);
                    visualizarPatron.Owner = this;
                    visualizarPatron.ShowDialog();
                }
                else
                {
                    GUI_VisualizarPatron visualizarPatron = new GUI_VisualizarPatron(4, esNuevo, patronSeleccionado, numCaciones[3], listaCaciones[3]);
                    visualizarPatron.Owner = this;
                    visualizarPatron.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("No se tiene patron seleccionado", "Error Patron");
            }
        }

        private void btnVisualizarPatronViernes(object sender, RoutedEventArgs e)
        {
            if (cbViernesPatron.SelectedIndex >= 0)
            {
                patronSeleccionado = (Patron)cbViernesPatron.SelectedItem;

                if (numCaciones[4] == -1)
                {
                    GUI_VisualizarPatron visualizarPatron = new GUI_VisualizarPatron(5, esNuevo, patronSeleccionado);
                    visualizarPatron.Owner = this;
                    visualizarPatron.ShowDialog();
                }
                else
                {
                    GUI_VisualizarPatron visualizarPatron = new GUI_VisualizarPatron(5, esNuevo, patronSeleccionado, numCaciones[4], listaCaciones[4]);
                    visualizarPatron.Owner = this;
                    visualizarPatron.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("No se tiene patron seleccionado", "Error Patron");
            }
        }

        private void btnVisualizarPatronSabado(object sender, RoutedEventArgs e)
        {
            if (cbSabadoPatron.SelectedIndex >= 0)
            {
                patronSeleccionado = (Patron)cbSabadoPatron.SelectedItem;

                if (numCaciones[5] == -1)
                {
                    GUI_VisualizarPatron visualizarPatron = new GUI_VisualizarPatron(6, esNuevo, patronSeleccionado);
                    visualizarPatron.Owner = this;
                    visualizarPatron.ShowDialog();
                }
                else
                {
                    GUI_VisualizarPatron visualizarPatron = new GUI_VisualizarPatron(6, esNuevo, patronSeleccionado, numCaciones[5], listaCaciones[5]);
                    visualizarPatron.Owner = this;
                    visualizarPatron.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("No se tiene patron seleccionado", "Error Patron");
            }
        }

        private void btnVisualizarPatronDomingo(object sender, RoutedEventArgs e)
        {
            if (cbDomingoPatron.SelectedIndex >= 0)
            {
                patronSeleccionado = (Patron)cbDomingoPatron.SelectedItem;

                if (numCaciones[6] == -1)
                {
                    GUI_VisualizarPatron visualizarPatron = new GUI_VisualizarPatron(7, esNuevo, patronSeleccionado);
                    visualizarPatron.Owner = this;
                    visualizarPatron.ShowDialog();
                }
                else
                {
                    GUI_VisualizarPatron visualizarPatron = new GUI_VisualizarPatron(7, esNuevo, patronSeleccionado, numCaciones[6], listaCaciones[6]);
                    visualizarPatron.Owner = this;
                    visualizarPatron.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("No se tiene patron seleccionado", "Error Patron");
            }
        }

        public void agregar(int numeroDia, int numeroCanciones, List<Cancion> listaCanciones)
        {
            // se asignan los valores de la seleccion de la cantidad de musica a su respectivo patron

            switch (numeroDia)
            {
                case 1:
                    numCaciones[0] = numeroCanciones;
                    listaCaciones[0] = listaCanciones;
                    break;
                case 2:
                    numCaciones[1] = numeroCanciones;
                    listaCaciones[1] = listaCanciones;
                    break;
                case 3:
                    numCaciones[2] = numeroCanciones;
                    listaCaciones[2] = listaCanciones;
                    break;
                case 4:
                    numCaciones[3] = numeroCanciones;
                    listaCaciones[3] = listaCanciones;
                    break;
                case 5:
                    numCaciones[4] = numeroCanciones;
                    listaCaciones[4] = listaCanciones;
                    break;
                case 6:
                    numCaciones[5] = numeroCanciones;
                    listaCaciones[5] = listaCanciones;
                    break;
                case 7:
                    numCaciones[6] = numeroCanciones;
                    listaCaciones[6] = listaCanciones;
                    break;

            }
        }

        public Boolean validaHora(String dia, String horaInicio, String horaFinal, int idPrograma)
        {
            Boolean seTranslapa = false;

            if (horaInicio.Equals("24:00"))
            {
                horaInicio = "23:59";
            }
            if (horaFinal.Equals("24:00"))
            {
                horaFinal = "23:59";
            }

            var horaComienzo = TimeSpan.Parse(horaInicio);
            var horaTerminación = TimeSpan.Parse(horaFinal);

            List<DiasOperacion> diasOperacions = new List<DiasOperacion>();

            switch (dia)
            {
                case "Lunes":
                    diasOperacions = DAODiasOperacion.obtenerHorasDia(1, idPrograma);
                    foreach (DiasOperacion diaoperacion in diasOperacions)
                    {
                        if (horaComienzo >= diaoperacion.HoraInicio && horaComienzo <= diaoperacion.HoraFinal)
                        {
                            seTranslapa = true;
                        }

                        if (horaTerminación >= diaoperacion.HoraInicio && horaTerminación <= diaoperacion.HoraFinal)
                        {
                            seTranslapa = true;
                        }
                    }
                    break;
                case "Martes":
                    diasOperacions = DAODiasOperacion.obtenerHorasDia(2, idPrograma);
                    foreach (DiasOperacion diaoperacion in diasOperacions)
                    {
                        if (horaComienzo >= diaoperacion.HoraInicio && horaComienzo <= diaoperacion.HoraFinal)
                        {
                            seTranslapa = true;
                        }

                        if (horaTerminación >= diaoperacion.HoraInicio && horaTerminación <= diaoperacion.HoraFinal)
                        {
                            seTranslapa = true;
                        }

                    }
                    break;
                case "Miercoles":
                    diasOperacions = DAODiasOperacion.obtenerHorasDia(3, idPrograma);
                    foreach (DiasOperacion diaoperacion in diasOperacions)
                    {
                        if (horaComienzo >= diaoperacion.HoraInicio && horaComienzo <= diaoperacion.HoraFinal)
                        {
                            seTranslapa = true;
                        }

                        if (horaTerminación >= diaoperacion.HoraInicio && horaTerminación <= diaoperacion.HoraFinal)
                        {
                            seTranslapa = true;
                        }

                    }
                    break;
                case "Jueves":
                    diasOperacions = DAODiasOperacion.obtenerHorasDia(4, idPrograma);
                    foreach (DiasOperacion diaoperacion in diasOperacions)
                    {
                        if (horaComienzo >= diaoperacion.HoraInicio && horaComienzo <= diaoperacion.HoraFinal)
                        {
                            seTranslapa = true;
                        }

                        if (horaTerminación >= diaoperacion.HoraInicio && horaTerminación <= diaoperacion.HoraFinal)
                        {
                            seTranslapa = true;
                        }

                    }
                    break;
                case "Viernes":
                    diasOperacions = DAODiasOperacion.obtenerHorasDia(5, idPrograma);
                    foreach (DiasOperacion diaoperacion in diasOperacions)
                    {
                        if (horaComienzo >= diaoperacion.HoraInicio && horaComienzo <= diaoperacion.HoraFinal)
                        {
                            seTranslapa = true;
                        }

                        if (horaTerminación >= diaoperacion.HoraInicio && horaTerminación <= diaoperacion.HoraFinal)
                        {
                            seTranslapa = true;
                        }

                    }
                    break;
                case "Sabado":
                    diasOperacions = DAODiasOperacion.obtenerHorasDia(6, idPrograma);
                    foreach (DiasOperacion diaoperacion in diasOperacions)
                    {
                        if (horaComienzo >= diaoperacion.HoraInicio && horaComienzo <= diaoperacion.HoraFinal)
                        {
                            seTranslapa = true;
                        }

                        if (horaTerminación >= diaoperacion.HoraInicio && horaTerminación <= diaoperacion.HoraFinal)
                        {
                            seTranslapa = true;
                        }

                    }
                    break;
                case "Domingo":
                    diasOperacions = DAODiasOperacion.obtenerHorasDia(7, idPrograma);
                    foreach (DiasOperacion diaoperacion in diasOperacions)
                    {
                        if (horaComienzo >= diaoperacion.HoraInicio && horaComienzo <= diaoperacion.HoraFinal)
                        {
                            seTranslapa = true;
                        }

                        if (horaTerminación >= diaoperacion.HoraInicio && horaTerminación <= diaoperacion.HoraFinal)
                        {
                            seTranslapa = true;
                        }

                    }
                    break;
            }

            return seTranslapa;
        }

    }
}
