using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radio.MODELO.POCO
{
    class CancionDelDia
    {
        int idCancionDelDia;
        String tituloCancion;
        String claveCancion;
        String nombreCantante;
        String nombreCategoria;
        String nombreGenero;
        String diaDeSemana;
        String nombrePatron;

        public int IdCancionDelDia { get => idCancionDelDia; set => idCancionDelDia = value; }
        public string TituloCancion { get => tituloCancion; set => tituloCancion = value; }
        public string ClaveCancion { get => claveCancion; set => claveCancion = value; }
        public string NombreCantante { get => nombreCantante; set => nombreCantante = value; }
        public string NombreCategoria { get => nombreCategoria; set => nombreCategoria = value; }
        public string NombreGenero { get => nombreGenero; set => nombreGenero = value; }
        public String DiaDeSemana { get => diaDeSemana; set => diaDeSemana = value; }
        public string NombrePatron { get => nombrePatron; set => nombrePatron = value; }
    }
}
