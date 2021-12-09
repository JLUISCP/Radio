using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radio.MODELO.POCO
{
    public class ContadorDeCancion
    {
        private int idContador;
        private String nombreGenero;
        private String nombreCategoria;
        private int totalCanciones;

        public int IdContador { get => idContador; set => idContador = value; }
        public string NombreGenero { get => nombreGenero; set => nombreGenero = value; }
        public string NombreCategoria { get => nombreCategoria; set => nombreCategoria = value; }
        public int TotalCanciones { get => totalCanciones; set => totalCanciones = value; }
    }
}
