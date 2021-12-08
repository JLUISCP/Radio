using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radio.MODELO.POCO
{
    class Usuario
    {
        private int idUsuario;
        private String nombre;
        private String nombreUsuario;
        private String contraseña;
        private int idRol;
        private int idRadio;

        public int IdUsuario { get => idUsuario; set => idUsuario = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string NombreUsuario { get => nombreUsuario; set => nombreUsuario = value; }
        public string Contraseña { get => contraseña; set => contraseña = value; }
        public int IdRol { get => idRol; set => idRol = value; }
        public int IdRadio { get => idRadio; set => idRadio = value; }

        public override string ToString()
        {
            return nombre;
        }
    }
}
