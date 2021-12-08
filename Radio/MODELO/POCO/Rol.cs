using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radio.MODELO.POCO
{
    class Rol
    {
        private int idRol;
        private String puesto;

        public int IdRol { get => idRol; set => idRol = value; }
        public string Puesto { get => puesto; set => puesto = value; }

        public override string ToString()
        {
            return puesto;
        }
    }
}
