using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radio.MODELO.POCO
{
    class DiasOperacion
    {
        private int idDia;
        private int numCanciones;
        private int numeroDia;
        private TimeSpan horaFinal;
        private TimeSpan horaInicio;
        private int estadoDia;
        private int idPatron;
        private int idProgramaDia;

        //Tabla Programa Patron
        private ProgramaPatron programaPatron;
        private List<Cancion> cancionesProgramaPatron;

        public int IdDia { get => idDia; set => idDia = value; }
        public int NumCanciones { get => numCanciones; set => numCanciones = value; }
        public int NumeroDia { get => numeroDia; set => numeroDia = value; }
        public TimeSpan HoraFinal { get => horaFinal; set => horaFinal = value; }
        public TimeSpan HoraInicio { get => horaInicio; set => horaInicio = value; }
        public int EstadoDia { get => estadoDia; set => estadoDia = value; }
        public int IdPatron { get => idPatron; set => idPatron = value; }
        public int IdProgramaDia { get => idProgramaDia; set => idProgramaDia = value; }
        public List<Cancion> CancionesProgramaPatron { get => cancionesProgramaPatron; set => cancionesProgramaPatron = value; }
        internal ProgramaPatron ProgramaPatron { get => programaPatron; set => programaPatron = value; }
    }
}
