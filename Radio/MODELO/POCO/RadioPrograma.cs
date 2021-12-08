using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radio.MODELO.POCO
{
    class RadioPrograma
    {
        private int idRadio;
        private String nombre;

        public int IdRadio { get => idRadio; set => idRadio = value; }
        public string Nombre { get => nombre; set => nombre = value; }
    }
}
