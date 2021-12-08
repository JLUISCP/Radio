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


        /// ////////////////////////////////////////////////////

        public static List<Canciones> getCanciones()
        {
            List<Canciones> can01 = new List<Canciones>();
            SqlConnection conn = null;

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    SqlDataReader dataReader;
                    String query = "SELECT C.CAN_ID, C.CAN_TITULO, C.CAN_CLAVE, C.CAN_DIAS, " +
                        "C.CAN_PRIORIDAD, C.CAN_FECHAALTA, C.CAN_FECHAMODIFICACION, C.CAN_ESTATUS, " +
                        "C.CAN_FECHABLOQUEO, C.CAN_COMENTARIOS, C.CAN_OBSERVACIONES, " +
                        "C.CNT_ID, CNT_NOMBRE, C.CAT_ID, CT.CAT_NOMBRE, C.GNR_ID, G.GNR_NOMBRE " +
                        "FROM mus_canciones C " +
                        "INNER JOIN mus_cantantes CN ON C.CNT_ID = CN.CNT_ID " +
                        "INNER JOIN mus_generos G ON C.GNR_ID = G.GNR_ID " +
                        "INNER JOIN dbo.mus_categorias CT ON C.CAT_ID = CT.CAT_ID; ";
                    command = new SqlCommand(query, conn);
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Canciones cancion = new Canciones();
                        cancion.CAN_ID1 = (int)((!dataReader.IsDBNull(0)) ? dataReader.GetInt64(0) : 0);
                        cancion.CAN_TITULO1 = (!dataReader.IsDBNull(1)) ? dataReader.GetString(1) : "";
                        cancion.CAN_CLAVE1 = (!dataReader.IsDBNull(2)) ? dataReader.GetString(2) : "";
                        cancion.CAN_DIAS1 = (!dataReader.IsDBNull(3)) ? dataReader.GetString(3) : "";
                        cancion.CAN_PRIORIDAD1 = (int)((!dataReader.IsDBNull(4)) ? dataReader.GetInt64(4) : 0);
                        cancion.CAN_FECHAALTA1 = (!dataReader.IsDBNull(5)) ? dataReader.GetDateTime(5) : DateTime.MinValue;
                        cancion.CAN_FECHAMODIFICACION1 = (!dataReader.IsDBNull(6)) ? dataReader.GetDateTime(6) : DateTime.MinValue;
                        cancion.CAN_ESTATUS1 = (!dataReader.IsDBNull(7)) ? dataReader.GetInt32(7) : 0;
                        cancion.CAN_FECHABLOQUEO1 = (!dataReader.IsDBNull(8)) ? dataReader.GetDateTime(8) : DateTime.MinValue;
                        cancion.CAN_COMENTARIOS1 = (!dataReader.IsDBNull(9)) ? dataReader.GetString(9) : "";
                        cancion.CAN_OBSERVACIONES1 = (!dataReader.IsDBNull(10)) ? dataReader.GetString(10) : "";
                        cancion.CNT_ID1 = (int)((!dataReader.IsDBNull(11)) ? dataReader.GetInt64(11) : 0);
                        cancion.CNT_NOMBRE1 = (!dataReader.IsDBNull(12)) ? dataReader.GetString(12) : "";
                        cancion.CAT_ID1 = (int)((!dataReader.IsDBNull(13)) ? dataReader.GetInt64(13) : 0);
                        cancion.CAT_NOMBRE1 = (!dataReader.IsDBNull(14) ? dataReader.GetString(14) : "");
                        cancion.GNR_ID1 = (int)((!dataReader.IsDBNull(15)) ? dataReader.GetInt64(15) : 0);
                        cancion.GNR_NOMBRE1 = (!dataReader.IsDBNull(16) ? dataReader.GetString(16) : "");
                        can01.Add(cancion);
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

            return can01;
        }

        public static int claveDuplicada(String clave)
        {
            SqlConnection conn = null;
            SqlDataReader numero = null;
            int valor = 0;

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    String query = String.Format("SELECT COUNT(*) FROM mus_canciones WHERE CAN_CLAVE = '{0}'; ", clave);
                    command = new SqlCommand(query, conn);
                    numero = command.ExecuteReader();
                    if (numero.Read())
                    {
                        valor = (!numero.IsDBNull(0)) ? numero.GetInt32(0) : 0;
                    }
                    numero.Close();
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
            return valor;
        }

        public static int tituloDuplicado(String titulo, Int64 idCantante)
        {
            SqlConnection conn = null;
            SqlDataReader numero = null;
            int valor = 0;

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    String query = String.Format("SELECT COUNT(*) FROM mus_canciones WHERE CAN_TITULO = '{0}' " +
                    "AND CNT_ID = '{1}'; ", titulo, idCantante);
                    command = new SqlCommand(query, conn);
                    numero = command.ExecuteReader();
                    if (numero.Read())
                    {
                        valor = (!numero.IsDBNull(0)) ? numero.GetInt32(0) : 0;
                    }
                    numero.Close();
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
            return valor;
        }

        public static int cancionDuplicada(String clave, String titulo, Int64 idCantante, Int64 idCategoria, Int64 idGenero)
        {
            SqlConnection conn = null;
            SqlDataReader numero = null;
            int valor = 0;

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    String query = String.Format("SELECT COUNT(*) FROM mus_canciones WHERE CAN_CLAVE = '{0}' AND CAN_TITULO = '{1}' " +
                        "AND CNT_ID = '{2}' AND CAT_ID = '{3}' AND GNR_ID = '{4}'; ", clave, titulo, idCantante, idCategoria, idGenero);
                    command = new SqlCommand(query, conn);
                    numero = command.ExecuteReader();
                    if (numero.Read())
                    {
                        valor = (!numero.IsDBNull(0)) ? numero.GetInt32(0) : 0;
                    }
                    numero.Close();
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

            return valor;
        }

        public static int getID()
        {
            SqlConnection conn = null;
            SqlDataReader numero = null;
            int valor = 0;

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    String query = "SELECT MAX(CAN_ID) FROM mus_canciones; ";
                    command = new SqlCommand(query, conn);
                    numero = command.ExecuteReader();
                    if (numero.Read())
                    {
                        valor = (int)((!numero.IsDBNull(0)) ? numero.GetInt64(0) : 0);
                    }
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
            return valor;
        }

        public static String getEstatus(int idCantante)
        {
            SqlConnection conn = null;
            SqlDataReader numero = null;
            String valor = "";

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    String query = String.Format("SELECT CNT_ESTATUS from mus_cantantes " +
                        "where CNT_ID = '{0}'; ", idCantante);
                    command = new SqlCommand(query, conn);
                    numero = command.ExecuteReader();
                    if (numero.Read())
                    {
                        valor = (!numero.IsDBNull(0)) ? numero.GetString(0) : "";
                    }
                    numero.Close();
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
            return valor;
        }

        public static String guardarCancion(Int64 idCancion, String titulo, String clave, String dias, Int64 priordad, int estatus,
            String comentario, String observacion, Int64 idCantante, Int64 idcategoria, Int64 idGenero)
        {
            SqlConnection conn = null;
            String valor = "";

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    String fecha = DateTime.Now.ToString("yyyy-MM-dd");
                    var nuevaFecha = DateTime.Parse(fecha).ToString("yyyy-MM-dd");
                    String query = String.Format("INSERT INTO mus_canciones (CAN_ID, CAN_TITULO, CAN_CLAVE, CAN_DIAS, " +
                        "CAN_PRIORIDAD, CAN_FECHAALTA, CAN_FECHAMODIFICACION, CAN_ESTATUS, CAN_COMENTARIOS, CAN_OBSERVACIONES, " +
                        "CNT_ID, CAT_ID, GNR_ID) values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', " +
                         "'{7}', '{8}', '{9}', '{10}', '{11}', '{12}');", idCancion, titulo, clave, dias, priordad, nuevaFecha, nuevaFecha,
                         estatus, comentario, observacion, idCantante, idcategoria, idGenero);
                    command = new SqlCommand(query, conn);
                    int validar = command.ExecuteNonQuery();
                    if (validar == 1)
                    {
                        valor = "La canción se ha registrado con éxito.";
                    }
                    else
                    {
                        valor = "No es posible registrar la canción, favor de verificar...";
                    }
                }
            }
            catch (Exception e)
            {
                valor = "Error al registrar una canción...";
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return valor;
        }

        public static String modificarCancion(Int64 idCancion, String titulo, String clave, String dias, Int64 prioridad, String comentario
           , String observacion, Int64 idCantante, Int64 idCategoria, Int64 idGenero)
        {
            SqlConnection conn = null;
            String valor = "";

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    String fecha = DateTime.Now.ToString("yyyy-MM-dd");
                    var nuevaFecha = DateTime.Parse(fecha).ToString("yyyy-MM-dd");
                    String query = String.Format("UPDATE mus_canciones SET CAN_TITULO = '{0}', CAN_CLAVE = '{1}', CAN_DIAS = '{2}', " +
                        "CAN_PRIORIDAD = '{3}', CAN_FECHAMODIFICACION = '{4}', CAN_COMENTARIOS = '{5}', CAN_OBSERVACIONES = '{6}', " +
                        "CNT_ID = '{7}', CAT_ID = '{8}', GNR_ID = '{9}'" +
                        "WHERE CAN_ID = '{10}'; ", titulo, clave, dias, prioridad, nuevaFecha, comentario, observacion,
                        idCantante, idCategoria, idGenero, idCancion);
                    command = new SqlCommand(query, conn);
                    int validar = command.ExecuteNonQuery();
                    if (validar == 1)
                    {
                        valor = "Canción actualizada con exito.";
                    }
                    else
                    {
                        valor = "No es posible actualizar la canción, Favor de verificar...";
                    }
                }
            }
            catch (Exception e)
            {
                valor = "Error al actualizar la canción...";
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return valor;
        }

        public static void cambiarEstatusCancion(int ac, int idCantante)
        {
            SqlConnection conn = null;

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    String query = String.Format("UPDATE mus_canciones set CAN_ESTATUS = '{0}' WHERE CNT_ID = '{1}' ", ac, idCantante);
                    command = new SqlCommand(query, conn);
                    command.ExecuteNonQuery();
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
        }

        public static String eliminarCancion(Int64 idCancion)
        {
            SqlConnection conn = null;
            String valor = "";
            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    String query = String.Format("DELETE FROM mus_canciones WHERE CAN_ID = '{0}'; ", idCancion);
                    command = new SqlCommand(query, conn);
                    int validar = command.ExecuteNonQuery();
                    if (validar == 1)
                    {
                        valor = "La cancion fue eliminada con éxito.";
                    }
                    else
                    {
                        valor = "No es posible eliminar el cantante. Intente de nuevo...";
                    }
                }
            }
            catch (Exception e)
            {
                valor = "Error al eliminar la canción, favor de verificar...";
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return valor;
        }
    }
}