using Radio.MODELO.DAO;
using Radio.MODELO.POCO;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Radio.VISTAS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {

        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            String user = txtUsername.Text;
            String pass = txtPassword.Password;
            if(user != "" && pass != "")
            {
                Usuario usuario = LoginDAO.getLogin(user, pass);
                if(usuario != null)
                {
                    if(usuario.IdRol == 1)
                    {
                        CRUDUsuario gestionUsuario = new CRUDUsuario();
                        gestionUsuario.Show();
                        this.Close();
                    }
                    else
                    {
                        MenuPrincipal menuPrincipal = new MenuPrincipal();
                        menuPrincipal.Show();
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Usuario/contraseña incorrectas, favor de verificarlas");
                }
            }
            else
            {
                MessageBox.Show("Favor de proporcionar el usuario y la contraseña");
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch
            {

            }
        }
    }
}