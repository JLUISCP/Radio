using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radio.MODELO.POCO
{
    public class Locutor
    {
        private int numero;
        private int idLocutor;
        private String nombreLocutor;
        private int idPrograma;
        private String nombrePrograma ;

        public int Numero { get => numero; set => numero = value; }
        public int IdLocutor { get => idLocutor; set => idLocutor = value; }
        public string NombreLocutor { get => nombreLocutor; set => nombreLocutor = value; }
        public int IdPrograma { get => idPrograma; set => idPrograma = value; }
        public string NombrePrograma { get => nombrePrograma; set => nombrePrograma = value; }

        public override string ToString()
        {
            return nombreLocutor;
        }
    }
}
