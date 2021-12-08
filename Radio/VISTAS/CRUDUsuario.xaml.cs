using Radio.MODELO.DAO;
using Radio.MODELO.POCO;
using System.Windows;
using System.Windows.Controls;

namespace Radio.VISTAS
{
    /// <summary>
    /// Lógica de interacción para CRUDUsuario.xaml
    /// </summary>
    public partial class CRUDUsuario : Window
    {
        private Usuario usuarioSeleccionado;
        public CRUDUsuario()
        {
            InitializeComponent();
            cargarRoles();
            cargarUsuarios();
            btnEliminar.IsEnabled = false;
            btnModificar.IsEnabled = false;
        }

        private void cargarUsuarios()
        {
            dg_Usuarios.AutoGenerateColumns = false;
            dg_Usuarios.ItemsSource = UsuarioDAO.getUsuarios();
        }

        private void cargarRoles()
        {
            cbRol.ItemsSource = RolDAO.getRoles();
        }

        private void dg_Usuarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dg_Usuarios.SelectedItem != null)
            {
                usuarioSeleccionado = (Usuario)dg_Usuarios.SelectedItem;
                btnEliminar.IsEnabled = true;
                btnModificar.IsEnabled = true;
                btnRegistrar.IsEnabled = false;
                cargarInformacion();
            }
        }

        private void cargarInformacion()
        {
            tbNombre.Text = usuarioSeleccionado.Nombre;
            tbNombreUsuario.Text = usuarioSeleccionado.NombreUsuario;
            tbContraseña.Text = usuarioSeleccionado.Contraseña;
            if(usuarioSeleccionado.IdRol == 2)
            {
                cbRol.SelectedIndex = 0;
            }
            if (usuarioSeleccionado.IdRol == 3)
            {
                cbRol.SelectedIndex = 1;
            }
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            limpiarCampos();
        }

        private void limpiarCampos()
        {
            tbNombre.Text = "";
            tbNombreUsuario.Text = "";
            tbContraseña.Text = "";
            cbRol.SelectedItem = null;
            btnEliminar.IsEnabled = false;
            btnModificar.IsEnabled = false;
            btnRegistrar.IsEnabled = true;
            dg_Usuarios.SelectedItem = null;
        }

        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            if (!camposVacios())
            {
                Usuario usuario = new Usuario();
                usuario.Nombre = tbNombre.Text;
                usuario.NombreUsuario = tbNombreUsuario.Text;
                usuario.Contraseña = tbContraseña.Text;
                usuario.IdRadio = 1;
                usuario.IdRol = ((Rol)cbRol.SelectedItem).IdRol;
                UsuarioDAO.registrarUsuario(usuario);
                MessageBox.Show("Usuario registrado");
                cargarUsuarios();
                limpiarCampos();
            }
            else
            {
                MessageBox.Show("Favor se ingresar todos los datos");
            }
        }

        private bool camposVacios()
        {
            bool camposVacios = false;
            if (tbNombre.Text == "")
                camposVacios = true;
            if (tbNombreUsuario.Text == "")
                camposVacios = true;
            if (tbContraseña.Text == "")
                camposVacios = true;
            if (cbRol.SelectedItem == null)
                camposVacios = true;

            return camposVacios;
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (!camposVacios())
            {
                usuarioSeleccionado.Nombre = tbNombre.Text;
                usuarioSeleccionado.NombreUsuario = tbNombreUsuario.Text;
                usuarioSeleccionado.Contraseña = tbContraseña.Text;
                usuarioSeleccionado.IdRol = ((Rol)cbRol.SelectedItem).IdRol;
                UsuarioDAO.actualizarUsuario(usuarioSeleccionado);
                MessageBox.Show("Usuario modificado");
                cargarUsuarios();
                limpiarCampos();
            }
            else
            {
                MessageBox.Show("Favor se ingresar todos los datos");
            }
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            UsuarioDAO.eliminarUsuario(usuarioSeleccionado.IdUsuario);
            MessageBox.Show("Usuario eliminado");
            limpiarCampos();
            cargarUsuarios();
        }
    }
}
