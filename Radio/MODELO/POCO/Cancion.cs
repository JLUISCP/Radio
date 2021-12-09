using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radio.MODELO.POCO
{
    public class Cancion
    {
        private int idMusicaDiaPatron;
        private int idProgramaPatron;
        private int cancionID;
        private String titulo;
        private String clave;
        private String dia;
        private int prioridad;
        private DateTime fechaAlta;
        private DateTime fechaModificacion;
        private int estatus;
        private DateTime fechaBloqueo;
        private String comentario;
        private String observacion;
        private int idCantante;
        private String nombreCantante;
        private int idCategoria;
        private String nombreCategoria;
        private int idGenero;
        private String nombreGenero;

        public int IdMusicaDiaPatron { get => idMusicaDiaPatron; set => idMusicaDiaPatron = value; }
        public int IdProgramaPatron { get => idProgramaPatron; set => idProgramaPatron = value; }
        public int CancionID { get => cancionID; set => cancionID = value; }
        public string Titulo { get => titulo; set => titulo = value; }
        public string Clave { get => clave; set => clave = value; }
        public string Dia { get => dia; set => dia = value; }
        public int Prioridad { get => prioridad; set => prioridad = value; }
        public DateTime FechaAlta { get => fechaAlta; set => fechaAlta = value; }
        public DateTime FechaModificacion { get => fechaModificacion; set => fechaModificacion = value; }
        public int Estatus { get => estatus; set => estatus = value; }
        public DateTime FechaBloqueo { get => fechaBloqueo; set => fechaBloqueo = value; }
        public string Comentario { get => comentario; set => comentario = value; }
        public string Observacion { get => observacion; set => observacion = value; }
        public int IdCantante { get => idCantante; set => idCantante = value; }
        public string NombreCantante { get => nombreCantante; set => nombreCantante = value; }
        public int IdCategoria { get => idCategoria; set => idCategoria = value; }
        public string NombreCategoria { get => nombreCategoria; set => nombreCategoria = value; }
        public int IdGenero { get => idGenero; set => idGenero = value; }
        public string NombreGenero { get => nombreGenero; set => nombreGenero = value; }
    }
}
