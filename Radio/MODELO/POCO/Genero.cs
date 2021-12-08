using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radio.MODELO.POCO
{
    class Genero
    {
        private int idGenero;
        private String nombreGenero;

        public int IdGenero { get => idGenero; set => idGenero = value; }
        public string NombreGenero { get => nombreGenero; set => nombreGenero = value; }

        public override string ToString()
        {
            return nombreGenero;
        }
    }
}
