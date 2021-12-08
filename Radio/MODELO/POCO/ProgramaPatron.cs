using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radio.MODELO.POCO
{
    class ProgramaPatron
    {
        private int idprogramaPatron;
        private int idDia;
        private int idPatron;
        private int idCancionActual;

        public int IdprogramaPatron { get => idprogramaPatron; set => idprogramaPatron = value; }
        public int IdDia { get => idDia; set => idDia = value; }
        public int IdPatron { get => idPatron; set => idPatron = value; }
        public int IdCancionActual { get => idCancionActual; set => idCancionActual = value; }
    }
}
