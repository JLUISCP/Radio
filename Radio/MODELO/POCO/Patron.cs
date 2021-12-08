using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radio.MODELO.POCO
{
    public class Patron
    {
        private int idPatron;
        private String nombrePatron;
        private int idRadio;
        private String nombreRadio;
        private List<LineaPatron> lineaPatron;

        public int IdPatron { get => idPatron; set => idPatron = value; }
        public string NombrePatron { get => nombrePatron; set => nombrePatron = value; }
        public int IdRadio { get => idRadio; set => idRadio = value; }
        public string NombreRadio { get => nombreRadio; set => nombreRadio = value; }
        public List<LineaPatron> LineaPatron { get => lineaPatron; set => lineaPatron = value; }

        public override string ToString()
        {
            return nombrePatron;
        }
    }
}
