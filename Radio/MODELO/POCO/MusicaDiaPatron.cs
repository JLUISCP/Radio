using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radio.MODELO.POCO
{
    class MusicaDiaPatron
    { 
        private int idMusicaDiaPatron;
        private int idCanción;
        private int idProgramaPatroon;

        public int IdMusicaDiaPatron { get => idMusicaDiaPatron; set => idMusicaDiaPatron = value; }
        public int IdCanción { get => idCanción; set => idCanción = value; }
        public int IdProgramaPatroon { get => idProgramaPatroon; set => idProgramaPatroon = value; }
    }
}
