using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Radio.MODELO.POCO
{
    public class Programa
    {
        // Tabla Programa
        private int idPrograma;
        private String fechaBloqueo;
        private String fechaInicio;
        private String nombrePrograma;
        private int estadoPrograma;
        private String estadoProgramaNombre = "Inactivo";
        private byte[] imagen;
        private int idRadio;

        //Tabla Radio
        private String nombreRadio;

        //Tabla Locutor
        private List<Locutor> locutor;

        // Tabla Dias Operacion
        private List<DiasOperacion> diasOperacion;

        public int IdPrograma { get => idPrograma; set => idPrograma = value; }
        public string FechaBloqueo { get => fechaBloqueo; set => fechaBloqueo = value; }
        public string FechaInicio { get => fechaInicio; set => fechaInicio = value; }
        public string NombrePrograma { get => nombrePrograma; set => nombrePrograma = value; }
        public int EstadoPrograma { get => estadoPrograma; set => estadoPrograma = value; }
        public string EstadoProgramaNombre { get => estadoProgramaNombre; set => estadoProgramaNombre = value; }
        public byte[] Imagen { get => imagen; set => imagen = value; }
        public int IdRadio { get => idRadio; set => idRadio = value; }
        public string NombreRadio { get => nombreRadio; set => nombreRadio = value; }
        public List<Locutor> Locutor { get => locutor; set => locutor = value; }
        internal List<DiasOperacion> DiasOperacion { get => diasOperacion; set => diasOperacion = value; }
    }
}
