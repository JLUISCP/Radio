using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radio.MODELO.POCO
{
    public class LineaPatron
    {
        private int idLineaPatron;
        private int prioridadPatron;
        private int idCategoria;
        private String nombreCategoria;
        private int idGenero;
        private String nombreGenero;
        private int numeroCanciones;

        public int IdLineaPatron { get => idLineaPatron; set => idLineaPatron = value; }
        public int PrioridadPatron { get => prioridadPatron; set => prioridadPatron = value; }
        public int IdCategoria { get => idCategoria; set => idCategoria = value; }
        public string NombreCategoria { get => nombreCategoria; set => nombreCategoria = value; }
        public int IdGenero { get => idGenero; set => idGenero = value; }
        public string NombreGenero { get => nombreGenero; set => nombreGenero = value; }
        public int NumeroCanciones { get => numeroCanciones; set => numeroCanciones = value; }
    }
}
