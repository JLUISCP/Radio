using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radio.MODELO.POCO
{
    class HorarioDelPrograma
    {
        int idHorario;
        int numeroCanciones;
        String diaDeSemana;
        String nombrePatron;
        String horaInicio;
        String horaFin;

        public int NumeroCanciones { get => numeroCanciones; set => numeroCanciones = value; }
        public string DiaDeSemana { get => diaDeSemana; set => diaDeSemana = value; }
        public int IdHorario { get => idHorario; set => idHorario = value; }
        public string NombrePatron { get => nombrePatron; set => nombrePatron = value; }
        public string HoraInicio { get => horaInicio; set => horaInicio = value; }
        public string HoraFin { get => horaFin; set => horaFin = value; }
    }
}
