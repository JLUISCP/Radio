using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Radio.MODELO.DB;
using Radio.MODELO.POCO;

namespace Radio.MODELO.DAO
{
    class DAOCanciones
    {
        public static int obtenerTotalCanciones(int categoria, int genero)
        {
            SqlConnection conn = null;
            int numeroCanciones = 0;

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    SqlDataReader dataReader;

                    String query = String.Format("Select COUNT(CAN_ID) From mus_canciones Where CAT_ID = '{0}' AND  GNR_ID = '{1}' ;", categoria, genero);

                    command = new SqlCommand(query, conn);
                    dataReader = command.ExecuteReader();

                    if (dataReader.Read())
                    {
                        numeroCanciones = (!dataReader.IsDBNull(0)) ? dataReader.GetInt32(0) : 0;
                    }
                    dataReader.Close();
                    command.Dispose();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return numeroCanciones;
        }
    
        public static List<Cancion> obtenerCancionLineaPatron(int categoria, int genero)
        {
            List<Cancion> canciones = new List<Cancion>();

            SqlConnection conn = null;
            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    SqlDataReader sqlDataReader;
                    String query = String.Format("select C.CAN_ID, C.CAN_TITULO, C.CAN_CLAVE, C.CAN_DIAS, C.CAN_PRIORIDAD, CAN_FECHAALTA, C.CAN_FECHAMODIFICACION, " +
                                   "C.CAN_ESTATUS, C.CAN_FECHABLOQUEO, C.CAN_COMENTARIOS, C.CAN_OBSERVACIONES, C.CNT_ID,  C.CAT_ID ,C.GNR_ID, D.CNT_NOMBRE, " +
                                   "E.CAT_NOMBRE, F.GNR_NOMBRE FROM mus_canciones AS C LEFT JOIN mus_cantantes AS D ON  C.CNT_ID = D.CNT_ID LEFT JOIN mus_categorias AS E " +
                                   "ON C.CAT_ID = E.CAT_ID LEFT JOIN mus_generos AS F ON C.GNR_ID = F.GNR_ID WHERE C.CAT_ID = {0} AND C.GNR_ID = {1};", categoria, genero);

                    command = new SqlCommand(query, conn);
                    sqlDataReader = command.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Cancion cancion = new Cancion();
                        
                        cancion.CancionID = (int)((!sqlDataReader.IsDBNull(0)) ? sqlDataReader.GetInt64(0) : 0);
                        cancion.Titulo = (!sqlDataReader.IsDBNull(1)) ? sqlDataReader.GetString(1) : "";
                        cancion.Clave = (!sqlDataReader.IsDBNull(2)) ? sqlDataReader.GetString(2) : "";
                        cancion.Dia = (!sqlDataReader.IsDBNull(3)) ? sqlDataReader.GetString(3) : "";
                        cancion.Prioridad = (int)((!sqlDataReader.IsDBNull(4)) ? sqlDataReader.GetInt64(4) : 0);
                        cancion.FechaAlta = (!sqlDataReader.IsDBNull(5)) ? sqlDataReader.GetDateTime(5) : DateTime.MinValue;
                        cancion.FechaModificacion = (!sqlDataReader.IsDBNull(6)) ? sqlDataReader.GetDateTime(6) : DateTime.MinValue;
                        //cancion.Estatus = (!sqlDataReader.IsDBNull(7)) ? sqlDataReader.GetInt32(7) : 0;
                        cancion.FechaBloqueo = (!sqlDataReader.IsDBNull(8)) ? sqlDataReader.GetDateTime(8) : DateTime.MinValue;
                        cancion.Comentario = (!sqlDataReader.IsDBNull(9)) ? sqlDataReader.GetString(9) : "";
                        cancion.Observacion = (!sqlDataReader.IsDBNull(10)) ? sqlDataReader.GetString(10) : "";
                        cancion.IdCantante = (int)((!sqlDataReader.IsDBNull(11)) ? sqlDataReader.GetInt64(11) : 0);
                        cancion.IdCategoria = (int)((!sqlDataReader.IsDBNull(12)) ? sqlDataReader.GetInt64(12) : 0);
                        cancion.IdGenero = (int)((!sqlDataReader.IsDBNull(13)) ? sqlDataReader.GetInt64(13) : 0);
                        cancion.NombreCantante = (!sqlDataReader.IsDBNull(14)) ? sqlDataReader.GetString(14) : "";
                        cancion.NombreCategoria = (!sqlDataReader.IsDBNull(15)) ? sqlDataReader.GetString(15) : "";
                        cancion.NombreGenero = (!sqlDataReader.IsDBNull(16)) ? sqlDataReader.GetString(16) : "";

                        canciones.Add(cancion);
                    }
                    sqlDataReader.Close();
                    command.Dispose();

                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return canciones;
        }

        public static List<Cancion> obtenerCancionDiaPatron(int programaPatron)
        {
            List<Cancion> canciones = new List<Cancion>();

            SqlConnection conn = null;
            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    SqlDataReader sqlDataReader;
                    String query = String.Format("select C.CAN_ID, C.CAN_TITULO, C.CAN_CLAVE, C.CAN_DIAS, C.CAN_PRIORIDAD, CAN_FECHAALTA, C.CAN_FECHAMODIFICACION, C.CAN_ESTATUS," +
                                                 " C.CAN_FECHABLOQUEO, C.CAN_COMENTARIOS, C.CAN_OBSERVACIONES, C.CNT_ID,  C.CAT_ID ,C.GNR_ID, D.CNT_NOMBRE, E.CAT_NOMBRE," +
                                                 " F.GNR_NOMBRE, G.MUSDIAPAT_ID, G.PROPAT_ID FROM mus_canciones AS C LEFT JOIN mus_cantantes AS D ON  C.CNT_ID = D.CNT_ID" +
                                                 " LEFT JOIN mus_categorias AS E ON C.CAT_ID = E.CAT_ID LEFT JOIN mus_generos AS F ON C.GNR_ID = F.GNR_ID LEFT JOIN" +
                                                 " mus_musicadiapatron AS G ON C.CAN_ID = G.CAN_ID WHERE G.PROPAT_ID = '{0}'", programaPatron);

                    command = new SqlCommand(query, conn);
                    sqlDataReader = command.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Cancion cancion = new Cancion();

                        cancion.CancionID = (int)((!sqlDataReader.IsDBNull(0)) ? sqlDataReader.GetInt64(0) : 0);
                        cancion.Titulo = (!sqlDataReader.IsDBNull(1)) ? sqlDataReader.GetString(1) : "";
                        cancion.Clave = (!sqlDataReader.IsDBNull(2)) ? sqlDataReader.GetString(2) : "";
                        cancion.Dia = (!sqlDataReader.IsDBNull(3)) ? sqlDataReader.GetString(3) : "";
                        cancion.Prioridad = (int)((!sqlDataReader.IsDBNull(4)) ? sqlDataReader.GetInt64(4) : 0);
                        cancion.FechaAlta = (!sqlDataReader.IsDBNull(5)) ? sqlDataReader.GetDateTime(5) : DateTime.MinValue;
                        cancion.FechaModificacion = (!sqlDataReader.IsDBNull(6)) ? sqlDataReader.GetDateTime(6) : DateTime.MinValue;
                        //cancion.Estatus = (!sqlDataReader.IsDBNull(7)) ? sqlDataReader.GetInt32(7) : 0;
                        cancion.FechaBloqueo = (!sqlDataReader.IsDBNull(8)) ? sqlDataReader.GetDateTime(8) : DateTime.MinValue;
                        cancion.Comentario = (!sqlDataReader.IsDBNull(9)) ? sqlDataReader.GetString(9) : "";
                        cancion.Observacion = (!sqlDataReader.IsDBNull(10)) ? sqlDataReader.GetString(10) : "";
                        cancion.IdCantante = (int)((!sqlDataReader.IsDBNull(11)) ? sqlDataReader.GetInt64(11) : 0);
                        cancion.IdCategoria = (int)((!sqlDataReader.IsDBNull(12)) ? sqlDataReader.GetInt64(12) : 0);
                        cancion.IdGenero = (int)((!sqlDataReader.IsDBNull(13)) ? sqlDataReader.GetInt64(13) : 0);
                        cancion.NombreCantante = (!sqlDataReader.IsDBNull(14)) ? sqlDataReader.GetString(14) : "";
                        cancion.NombreCategoria = (!sqlDataReader.IsDBNull(15)) ? sqlDataReader.GetString(15) : "";
                        cancion.NombreGenero = (!sqlDataReader.IsDBNull(16)) ? sqlDataReader.GetString(16) : "";
                        cancion.IdMusicaDiaPatron = (!sqlDataReader.IsDBNull(17)) ? sqlDataReader.GetInt32(17) : 0;
                        cancion.IdProgramaPatron = (!sqlDataReader.IsDBNull(18)) ? sqlDataReader.GetInt32(18) : 0;

                        canciones.Add(cancion);
                    }
                    sqlDataReader.Close();
                    command.Dispose();

                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return canciones;
        }
    }
}