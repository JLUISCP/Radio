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

        public static List<Cancion> consultarCancionesFiltradas(String nombreCantante, String nombreCategoria, String nombreGenero,
            String orden, String estado)
        {
            List<Cancion> canciones = new List<Cancion>();
            if ((nombreCantante != "") && (nombreCategoria != "") && (nombreGenero != ""))
            {
                canciones = usarTodosFiltros(nombreCantante, nombreCategoria, nombreGenero, orden, estado);
            }
            else if ((nombreGenero != "") && (nombreCategoria != ""))
            {
                canciones = filtrarPorGeneroYCategoria(nombreCategoria, nombreGenero, orden, estado);
            }
            else if ((nombreCategoria != "") && (nombreCantante != ""))
            {
                canciones = filtrarPorCantanteYCategoria(nombreCantante, nombreCategoria, orden, estado);
            }
            else if ((nombreGenero != "") && (nombreCantante != ""))
            {
                canciones = filtrarPorCantanteYGenero(nombreGenero, nombreCantante, orden, estado);
            }
            else if (nombreGenero != "")
            {
                canciones = filtrarPorGenero(nombreGenero, orden, estado);
            }
            else if (nombreCantante != "")
            {
                canciones = filtrarPorCantante(nombreCantante, orden, estado);
            }
            else if (nombreCategoria != "")
            {
                canciones = filtrarPorCategoria(nombreCategoria, orden, estado);
            }
            return canciones;
        }

        public static List<Cancion> usarTodosFiltros(String nombreCantante, String nombreCategoria, String nombreGenero,
            String orden, String estado)
        {
            List<Cancion> canciones = new List<Cancion>();
            SqlConnection conexion = ConexionBD.getConnection();
            String consulta = "SELECT c.CAN_ID, c.CAN_CLAVE, c.CAN_TITULO, a.CNT_NOMBRE, g.GNR_NOMBRE, c2.CAT_NOMBRE FROM mus_canciones c, " +
                "(SELECT c.CAN_ID, c2.CNT_NOMBRE FROM mus_canciones c " +
                    "LEFT JOIN mus_cantantes c2 ON c.CNT_ID = c2.CNT_ID " +
                    "WHERE c2.CNT_NOMBRE = @NombreCantante) " +
                    "AS a, " +
                "(SELECT c.CAN_ID, g.GNR_NOMBRE, c.CAT_ID FROM mus_canciones c " +
                    "LEFT JOIN mus_generos g ON c.GNR_ID = g.GNR_ID " +
                    "WHERE g.GNR_NOMBRE = @NombreGenero) " +
                    "AS g " +
                    "LEFT JOIN mus_categorias c2 ON g.CAT_ID = c2.CAT_ID " +
                "WHERE c.CAN_ID = a.CAN_ID AND c.CAN_ID = g.CAN_ID AND c2.CAT_NOMBRE = @NombreCategoria AND c.CAN_ESTATUS = @Estado " +
                "ORDER BY " +
                    "CASE @Orden " +
                        "WHEN 'Clave' THEN c.CAN_CLAVE " +
                        "WHEN 'Cantante' THEN a.CNT_NOMBRE " +
                        "WHEN 'Titulo' THEN c.CAN_TITULO " +
                    "END;";
            if ((nombreCategoria == "*") && (nombreGenero == "*") && (estado == "Todas"))
            {
                consulta = obtenerConsultaConCantanteSinEstado();
            }
            else if ((nombreCategoria == "*") && (nombreGenero == "*"))
            {
                consulta = obtenerConsultaConCantanteYEstado();
            }
            else if ((nombreGenero == "*") && (estado == "Todas"))
            {
                consulta = obtenerConsultaConCantanteYCategoria();
            }
            else if ((nombreCategoria == "*") && (estado == "Todas"))
            {
                consulta = obtenerConsultaConCantanteYGeneroSinEstado();
            }
            else if (nombreGenero == "*")
            {
                consulta = obtenerConsultaConCantanteCategoriaYEstado();
            }
            else if (nombreCategoria == "*")
            {
                consulta = obtenerConsultaConCantanteGeneroYEstado();
            }
            else if (estado == "Todas")
            {
                consulta = "SELECT c.CAN_ID, c.CAN_CLAVE, c.CAN_TITULO, a.CNT_NOMBRE, g.GNR_NOMBRE, c2.CAT_NOMBRE FROM mus_canciones c, " +
                    "(SELECT c.CAN_ID, c2.CNT_NOMBRE FROM mus_canciones c LEFT JOIN mus_cantantes c2 ON c.CNT_ID = c2.CNT_ID " +
                        "WHERE c2.CNT_NOMBRE = @NombreCantante) " +
                        "AS a, " +
                    "(SELECT c.CAN_ID, g.GNR_NOMBRE, c.CAT_ID FROM mus_canciones c " +
                        "LEFT JOIN mus_generos g ON c.GNR_ID = g.GNR_ID WHERE g.GNR_NOMBRE = @NombreGenero) " +
                        "AS g " +
                        "LEFT JOIN mus_categorias c2 ON g.CAT_ID = c2.CAT_ID " +
                    "WHERE c.CAN_ID = a.CAN_ID AND c.CAN_ID = g.CAN_ID AND c2.CAT_NOMBRE = @NombreCategoria " +
                    "ORDER BY " +
                        "CASE @Orden " +
                            "WHEN 'Clave' THEN c.CAN_CLAVE " +
                            "WHEN 'Cantante' THEN a.CNT_NOMBRE " +
                            "WHEN 'Titulo' THEN c.CAN_TITULO " +
                        "END;";
            }
            estado = convertirEstadoAClausula(estado);
            if (conexion != null)
            {
                try
                {
                    SqlCommand comando;
                    SqlDataReader lectorDatos;
                    comando = new SqlCommand(consulta, conexion);
                    comando.Parameters.AddWithValue("@NombreCantante", nombreCantante);
                    if (nombreGenero != "*")
                    {
                        comando.Parameters.AddWithValue("@NombreGenero", nombreGenero);
                    }
                    if (nombreCategoria != "*")
                    {
                        comando.Parameters.AddWithValue("@NombreCategoria", nombreCategoria);
                    }
                    comando.Parameters.AddWithValue("@Orden", orden);
                    if (estado != "Todas")
                    {
                        comando.Parameters.AddWithValue("@Estado", estado);
                    }
                    lectorDatos = comando.ExecuteReader();
                    while (lectorDatos.Read())
                    {
                        canciones.Add(crearCancion(lectorDatos));
                    }
                    lectorDatos.Close();
                    comando.Dispose();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return canciones = null;
                }
                finally
                {
                    if (conexion != null)
                    {
                        conexion.Close();
                    }
                }
            }
            return canciones;
        }

        public static List<Cancion> filtrarPorGeneroYCategoria(String nombreCategoria, String nombreGenero,
            String orden, String estado)
        {
            List<Cancion> canciones = new List<Cancion>();
            SqlConnection conexion = ConexionBD.getConnection();
            String consulta = "SELECT c.CAN_ID, c.CAN_CLAVE, c.CAN_TITULO, a.CNT_NOMBRE, g.GNR_NOMBRE, c2.CAT_NOMBRE FROM mus_canciones c, " +
                "(SELECT c.CAN_ID, c2.CNT_NOMBRE FROM mus_canciones c " +
                    "LEFT JOIN mus_cantantes c2 ON c.CNT_ID = c2.CNT_ID) " +
                    "AS a, " +
                "(SELECT c.CAN_ID, g.GNR_NOMBRE, c.CAT_ID FROM mus_canciones c " +
                    "LEFT JOIN mus_generos g ON c.GNR_ID = g.GNR_ID " +
                    "WHERE g.GNR_NOMBRE = @NombreGenero) " +
                    "AS g " +
                    "LEFT JOIN mus_categorias c2 ON g.CAT_ID = c2.CAT_ID " +
                "WHERE c.CAN_ID = a.CAN_ID AND c.CAN_ID = g.CAN_ID AND c2.CAT_NOMBRE = @NombreCategoria AND c.CAN_ESTATUS = @Estado " +
                "ORDER BY " +
                    "CASE @Orden " +
                        "WHEN 'Clave' THEN c.CAN_CLAVE " +
                        "WHEN 'Cantante' THEN a.CNT_NOMBRE " +
                        "WHEN 'Titulo' THEN c.CAN_TITULO " +
                    "END;";
            if ((nombreGenero == "*") && (nombreCategoria == "*") && (estado == "Todas"))
            {
                consulta = obtenerConsultaSinFiltrosYEstado();
            }
            else if ((nombreCategoria == "*") && (nombreGenero == "*"))
            {
                consulta = obtenerConsultaSinFiltrosConEstado();
            }
            else if ((nombreCategoria == "*") && (estado == "Todas"))
            {
                consulta = obtenerConsultaConGeneroSinEstado();
            }
            else if ((nombreGenero == "*") && (estado == "Todas"))
            {
                consulta = obtenerConsultaConCategoriaSinEstado();
            }
            else if (nombreGenero == "*")
            {
                consulta = obtenerConsultaConCategoriaYEstado();
            }
            else if (nombreCategoria == "*")
            {
                consulta = obtenerConsultaConGeneroYEstado();
            }
            else if (estado == "Todas")
            {
                consulta = "SELECT c.CAN_ID, c.CAN_CLAVE, c.CAN_TITULO, a.CNT_NOMBRE, g.GNR_NOMBRE, c2.CAT_NOMBRE FROM mus_canciones c, " +
                    "(SELECT c.CAN_ID, c2.CNT_NOMBRE FROM mus_canciones c " +
                        "LEFT JOIN mus_cantantes c2 ON c.CNT_ID = c2.CNT_ID) " +
                        "AS a, " +
                    "(SELECT c.CAN_ID, g.GNR_NOMBRE, c.CAT_ID FROM mus_canciones c " +
                        "LEFT JOIN mus_generos g ON c.GNR_ID = g.GNR_ID " +
                        "WHERE g.GNR_NOMBRE = @NombreGenero) " +
                        "AS g " +
                        "LEFT JOIN mus_categorias c2 ON g.CAT_ID = c2.CAT_ID " +
                    "WHERE c.CAN_ID = a.CAN_ID AND c.CAN_ID = g.CAN_ID AND c2.CAT_NOMBRE = @NombreCategoria " +
                    "ORDER BY " +
                        "CASE @Orden " +
                            "WHEN 'Clave' THEN c.CAN_CLAVE " +
                            "WHEN 'Cantante' THEN a.CNT_NOMBRE " +
                            "WHEN 'Titulo' THEN c.CAN_TITULO " +
                        "END;";
            }
            estado = convertirEstadoAClausula(estado);
            if (conexion != null)
            {
                try
                {
                    SqlCommand comando;
                    SqlDataReader lectorDatos;
                    comando = new SqlCommand(consulta, conexion);
                    if (nombreCategoria != "*")
                    {
                        comando.Parameters.AddWithValue("@NombreCategoria", nombreCategoria);
                    }
                    if (nombreGenero != "*")
                    {
                        comando.Parameters.AddWithValue("@NombreGenero", nombreGenero);
                    }
                    comando.Parameters.AddWithValue("@Orden", orden);
                    if (estado != "Todas")
                    {
                        comando.Parameters.AddWithValue("@Estado", estado);
                    }
                    lectorDatos = comando.ExecuteReader();
                    while (lectorDatos.Read())
                    {
                        canciones.Add(crearCancion(lectorDatos));
                    }
                    lectorDatos.Close();
                    comando.Dispose();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return canciones = null;
                }
                finally
                {
                    if (conexion != null)
                    {
                        conexion.Close();
                    }
                }
            }
            return canciones;
        }

        public static List<Cancion> filtrarPorCantanteYCategoria(String nombreCantante, String nombreCategoria, String orden, String estado)
        {
            List<Cancion> canciones = new List<Cancion>();
            SqlConnection conexion = ConexionBD.getConnection();
            String consulta = obtenerConsultaConCantanteCategoriaYEstado();
            if ((nombreCategoria == "*") && (estado == "Todas"))
            {
                consulta = obtenerConsultaConCantanteSinEstado();
            }
            else if (nombreCategoria == "*")
            {
                consulta = obtenerConsultaConCantanteYEstado();
            }
            else if (estado == "Todas")
            {
                consulta = obtenerConsultaConCantanteYCategoria();
            }
            estado = convertirEstadoAClausula(estado);
            if (conexion != null)
            {
                try
                {
                    SqlCommand comando;
                    SqlDataReader lectorDatos;
                    comando = new SqlCommand(consulta, conexion);
                    comando.Parameters.AddWithValue("@NombreCantante", nombreCantante);
                    if (nombreCategoria != "*")
                    {
                        comando.Parameters.AddWithValue("@NombreCategoria", nombreCategoria);
                    }
                    comando.Parameters.AddWithValue("@Orden", orden);
                    if (estado != "Todas")
                    {
                        comando.Parameters.AddWithValue("@Estado", estado);
                    }
                    lectorDatos = comando.ExecuteReader();
                    while (lectorDatos.Read())
                    {
                        canciones.Add(crearCancion(lectorDatos));
                    }
                    lectorDatos.Close();
                    comando.Dispose();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return canciones = null;
                }
                finally
                {
                    if (conexion != null)
                    {
                        conexion.Close();
                    }
                }
            }
            return canciones;
        }

        private static String obtenerConsultaConCantanteCategoriaYEstado()
        {
            return "SELECT c.CAN_ID, c.CAN_CLAVE, c.CAN_TITULO, a.CNT_NOMBRE, g.GNR_NOMBRE, c2.CAT_NOMBRE FROM mus_canciones c, " +
                "(SELECT c.CAN_ID, c2.CNT_NOMBRE FROM mus_canciones c " +
                    "LEFT JOIN mus_cantantes c2 ON c.CNT_ID = c2.CNT_ID " +
                    "WHERE c2.CNT_NOMBRE = @NombreCantante) " +
                    "AS a, " +
                "(SELECT c.CAN_ID, g.GNR_NOMBRE, c.CAT_ID FROM mus_canciones c " +
                    "LEFT JOIN mus_generos g ON c.GNR_ID = g.GNR_ID) " +
                    "AS g " +
                    "LEFT JOIN mus_categorias c2 ON g.CAT_ID = c2.CAT_ID " +
                "WHERE c.CAN_ID = a.CAN_ID AND c.CAN_ID = g.CAN_ID AND c2.CAT_NOMBRE = @NombreCategoria AND c.CAN_ESTATUS = @Estado " +
                "ORDER BY " +
                    "CASE @Orden " +
                        "WHEN 'Clave' THEN c.CAN_CLAVE " +
                        "WHEN 'Cantante' THEN a.CNT_NOMBRE " +
                        "WHEN 'Titulo' THEN c.CAN_TITULO " +
                    "END;";
        }

        private static String obtenerConsultaConCantanteYCategoria()
        {
            return "SELECT c.CAN_ID, c.CAN_CLAVE, c.CAN_TITULO, a.CNT_NOMBRE, g.GNR_NOMBRE, c2.CAT_NOMBRE FROM mus_canciones c, " +
                "(SELECT c.CAN_ID, c2.CNT_NOMBRE FROM mus_canciones c " +
                    "LEFT JOIN mus_cantantes c2 ON c.CNT_ID = c2.CNT_ID " +
                    "WHERE c2.CNT_NOMBRE = @NombreCantante) " +
                    "AS a, " +
                "(SELECT c.CAN_ID, g.GNR_NOMBRE, c.CAT_ID FROM mus_canciones c " +
                    "LEFT JOIN mus_generos g ON c.GNR_ID = g.GNR_ID) " +
                    "AS g " +
                    "LEFT JOIN mus_categorias c2 ON g.CAT_ID = c2.CAT_ID " +
                "WHERE c.CAN_ID = a.CAN_ID AND c.CAN_ID = g.CAN_ID AND c2.CAT_NOMBRE = @NombreCategoria " +
                "ORDER BY " +
                    "CASE @Orden " +
                        "WHEN 'Clave' THEN c.CAN_CLAVE " +
                        "WHEN 'Cantante' THEN a.CNT_NOMBRE " +
                        "WHEN 'Titulo' THEN c.CAN_TITULO " +
                    "END;";
        }

        public static List<Cancion> filtrarPorCantanteYGenero(String nombreGenero, String nombreCantante, String orden, String estado)
        {
            List<Cancion> canciones = new List<Cancion>();
            SqlConnection conexion = ConexionBD.getConnection();
            String consulta = obtenerConsultaConCantanteGeneroYEstado();
            if ((nombreGenero == "*") && (estado == "Todas"))
            {
                consulta = obtenerConsultaConCantanteSinEstado();
            }
            else if (nombreGenero == "*")
            {
                consulta = obtenerConsultaConCantanteYEstado();
            }
            else if (estado == "Todas")
            {
                consulta = obtenerConsultaConCantanteYGeneroSinEstado();
            }
            estado = convertirEstadoAClausula(estado);
            if (conexion != null)
            {
                try
                {
                    SqlCommand comando;
                    SqlDataReader lectorDatos;
                    comando = new SqlCommand(consulta, conexion);
                    comando.Parameters.AddWithValue("@NombreCantante", nombreCantante);
                    if (nombreGenero != "*")
                    {
                        comando.Parameters.AddWithValue("@NombreGenero", nombreGenero);
                    }
                    comando.Parameters.AddWithValue("@Orden", orden);
                    if (estado != "Todas")
                    {
                        comando.Parameters.AddWithValue("@Estado", estado);
                    }
                    lectorDatos = comando.ExecuteReader();
                    while (lectorDatos.Read())
                    {
                        canciones.Add(crearCancion(lectorDatos));
                    }
                    lectorDatos.Close();
                    comando.Dispose();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return canciones = null;
                }
                finally
                {
                    if (conexion != null)
                    {
                        conexion.Close();
                    }
                }
            }
            return canciones;
        }

        private static String obtenerConsultaConCantanteGeneroYEstado()
        {
            return "SELECT c.CAN_ID, c.CAN_CLAVE, c.CAN_TITULO, a.CNT_NOMBRE, g.GNR_NOMBRE, c2.CAT_NOMBRE FROM mus_canciones c, " +
                "(SELECT c.CAN_ID, c2.CNT_NOMBRE FROM mus_canciones c " +
                    "LEFT JOIN mus_cantantes c2 ON c.CNT_ID = c2.CNT_ID " +
                    "WHERE c2.CNT_NOMBRE = @NombreCantante) " +
                    "AS a, " +
                "(SELECT c.CAN_ID, g.GNR_NOMBRE, c.CAT_ID FROM mus_canciones c " +
                    "LEFT JOIN mus_generos g ON c.GNR_ID = g.GNR_ID " +
                    "WHERE g.GNR_NOMBRE = @NombreGenero) " +
                    "AS g " +
                    "LEFT JOIN mus_categorias c2 ON g.CAT_ID = c2.CAT_ID " +
                "WHERE c.CAN_ID = a.CAN_ID AND c.CAN_ID = g.CAN_ID AND c.CAN_ESTATUS = @Estado " +
                "ORDER BY " +
                    "CASE @Orden " +
                        "WHEN 'Clave' THEN c.CAN_CLAVE " +
                        "WHEN 'Cantante' THEN a.CNT_NOMBRE " +
                        "WHEN 'Titulo' THEN c.CAN_TITULO " +
                    "END;";
        }

        private static String obtenerConsultaConCantanteYGeneroSinEstado()
        {
            return "SELECT c.CAN_ID, c.CAN_CLAVE, c.CAN_TITULO, a.CNT_NOMBRE, g.GNR_NOMBRE, c2.CAT_NOMBRE FROM mus_canciones c, " +
                "(SELECT c.CAN_ID, c2.CNT_NOMBRE FROM mus_canciones c " +
                    "LEFT JOIN mus_cantantes c2 ON c.CNT_ID = c2.CNT_ID " +
                    "WHERE c2.CNT_NOMBRE = @NombreCantante) " +
                    "AS a, " +
                "(SELECT c.CAN_ID, g.GNR_NOMBRE, c.CAT_ID FROM mus_canciones c " +
                    "LEFT JOIN mus_generos g ON c.GNR_ID = g.GNR_ID " +
                    "WHERE g.GNR_NOMBRE = @NombreGenero) " +
                    "AS g " +
                    "LEFT JOIN mus_categorias c2 ON g.CAT_ID = c2.CAT_ID " +
                "WHERE c.CAN_ID = a.CAN_ID AND c.CAN_ID = g.CAN_ID " +
                "ORDER BY " +
                    "CASE @Orden " +
                        "WHEN 'Clave' THEN c.CAN_CLAVE " +
                        "WHEN 'Cantante' THEN a.CNT_NOMBRE " +
                        "WHEN 'Titulo' THEN c.CAN_TITULO " +
                    "END;";
        }

        public static List<Cancion> filtrarPorGenero(String nombreGenero, String orden, String estado)
        {
            List<Cancion> canciones = new List<Cancion>();
            SqlConnection conexion = ConexionBD.getConnection();
            String consulta = obtenerConsultaConGeneroYEstado();
            if ((nombreGenero == "*") && (estado == "Todas"))
            {
                consulta = obtenerConsultaSinFiltrosYEstado();
            }
            else if (nombreGenero == "*")
            {
                consulta = obtenerConsultaSinFiltrosConEstado();
            }
            else if (estado == "Todas")
            {
                consulta = obtenerConsultaConGeneroSinEstado();
            }
            estado = convertirEstadoAClausula(estado);
            if (conexion != null)
            {
                try
                {
                    SqlCommand comando;
                    SqlDataReader lectorDatos;
                    comando = new SqlCommand(consulta, conexion);
                    if (nombreGenero != "*")
                    {
                        comando.Parameters.AddWithValue("@NombreGenero", nombreGenero);
                    }
                    comando.Parameters.AddWithValue("@Orden", orden);
                    if (estado != "Todas")
                    {
                        comando.Parameters.AddWithValue("@Estado", estado);
                    }
                    lectorDatos = comando.ExecuteReader();
                    while (lectorDatos.Read())
                    {
                        canciones.Add(crearCancion(lectorDatos));
                    }
                    lectorDatos.Close();
                    comando.Dispose();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return canciones = null;
                }
                finally
                {
                    if (conexion != null)
                    {
                        conexion.Close();
                    }
                }
            }
            return canciones;
        }

        private static String obtenerConsultaConGeneroYEstado()
        {
            return "SELECT c.CAN_ID, c.CAN_CLAVE, c.CAN_TITULO, a.CNT_NOMBRE, g.GNR_NOMBRE, c2.CAT_NOMBRE FROM mus_canciones c, " +
                "(SELECT c.CAN_ID, c2.CNT_NOMBRE FROM mus_canciones c " +
                    "LEFT JOIN mus_cantantes c2 ON c.CNT_ID = c2.CNT_ID) " +
                    "AS a, " +
                "(SELECT c.CAN_ID, g.GNR_NOMBRE, c.CAT_ID FROM mus_canciones c " +
                    "LEFT JOIN mus_generos g ON c.GNR_ID = g.GNR_ID " +
                    "WHERE g.GNR_NOMBRE = @NombreGenero) " +
                    "AS g " +
                    "LEFT JOIN mus_categorias c2 ON g.CAT_ID = c2.CAT_ID " +
                "WHERE c.CAN_ID = a.CAN_ID AND c.CAN_ID = g.CAN_ID AND c.CAN_ESTATUS = @Estado " +
                "ORDER BY " +
                    "CASE @Orden " +
                        "WHEN 'Clave' THEN c.CAN_CLAVE " +
                        "WHEN 'Cantante' THEN a.CNT_NOMBRE " +
                        "WHEN 'Titulo' THEN c.CAN_TITULO " +
                    "END;";
        }

        private static String obtenerConsultaConGeneroSinEstado()
        {
            return "SELECT c.CAN_ID, c.CAN_CLAVE, c.CAN_TITULO, a.CNT_NOMBRE, g.GNR_NOMBRE, c2.CAT_NOMBRE FROM mus_canciones c, " +
                "(SELECT c.CAN_ID, c2.CNT_NOMBRE FROM mus_canciones c " +
                    "LEFT JOIN mus_cantantes c2 ON c.CNT_ID = c2.CNT_ID) " +
                    "AS a, " +
                "(SELECT c.CAN_ID, g.GNR_NOMBRE, c.CAT_ID FROM mus_canciones c " +
                    "LEFT JOIN mus_generos g ON c.GNR_ID = g.GNR_ID " +
                    "WHERE g.GNR_NOMBRE = @NombreGenero) " +
                    "AS g " +
                    "LEFT JOIN mus_categorias c2 ON g.CAT_ID = c2.CAT_ID " +
                "WHERE c.CAN_ID = a.CAN_ID AND c.CAN_ID = g.CAN_ID " +
                "ORDER BY " +
                    "CASE @Orden " +
                        "WHEN 'Clave' THEN c.CAN_CLAVE " +
                        "WHEN 'Cantante' THEN a.CNT_NOMBRE " +
                        "WHEN 'Titulo' THEN c.CAN_TITULO " +
                    "END;";
        }

        public static List<Cancion> filtrarPorCantante(String nombreCantante, String orden, String estado)
        {
            List<Cancion> canciones = new List<Cancion>();
            SqlConnection conexion = ConexionBD.getConnection();
            String consulta = obtenerConsultaConCantanteYEstado();
            if (estado == "Todas")
            {
                consulta = obtenerConsultaConCantanteSinEstado();
            }
            estado = convertirEstadoAClausula(estado);
            if (conexion != null)
            {
                try
                {
                    SqlCommand comando;
                    SqlDataReader lectorDatos;
                    comando = new SqlCommand(consulta, conexion);
                    comando.Parameters.AddWithValue("@NombreCantante", nombreCantante);
                    comando.Parameters.AddWithValue("@Orden", orden);
                    if (estado != "Todas")
                    {
                        comando.Parameters.AddWithValue("@Estado", estado);
                    }
                    lectorDatos = comando.ExecuteReader();
                    while (lectorDatos.Read())
                    {
                        canciones.Add(crearCancion(lectorDatos));
                    }
                    lectorDatos.Close();
                    comando.Dispose();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return canciones = null;
                }
                finally
                {
                    if (conexion != null)
                    {
                        conexion.Close();
                    }
                }
            }
            return canciones;
        }

        private static String obtenerConsultaConCantanteYEstado()
        {
            return "SELECT c.CAN_ID, c.CAN_CLAVE, c.CAN_TITULO, a.CNT_NOMBRE, g.GNR_NOMBRE, c2.CAT_NOMBRE FROM mus_canciones c, " +
                "(SELECT c.CAN_ID, c2.CNT_NOMBRE FROM mus_canciones c " +
                    "LEFT JOIN mus_cantantes c2 ON c.CNT_ID = c2.CNT_ID " +
                    "WHERE c2.CNT_NOMBRE = @NombreCantante) " +
                    "AS a, " +
                "(SELECT c.CAN_ID, g.GNR_NOMBRE, c.CAT_ID FROM mus_canciones c " +
                    "LEFT JOIN mus_generos g ON c.GNR_ID = g.GNR_ID) " +
                    "AS g " +
                    "LEFT JOIN mus_categorias c2 ON g.CAT_ID = c2.CAT_ID " +
                "WHERE c.CAN_ID = a.CAN_ID AND c.CAN_ID = g.CAN_ID AND c.CAN_ESTATUS = @Estado " +
                "ORDER BY " +
                    "CASE @Orden " +
                        "WHEN 'Clave' THEN c.CAN_CLAVE " +
                        "WHEN 'Cantante' THEN a.CNT_NOMBRE " +
                        "WHEN 'Titulo' THEN c.CAN_TITULO " +
                    "END;";
        }

        private static String obtenerConsultaConCantanteSinEstado()
        {
            return "SELECT c.CAN_ID, c.CAN_CLAVE, c.CAN_TITULO, a.CNT_NOMBRE, g.GNR_NOMBRE, c2.CAT_NOMBRE FROM mus_canciones c, " +
                "(SELECT c.CAN_ID, c2.CNT_NOMBRE FROM mus_canciones c " +
                    "LEFT JOIN mus_cantantes c2 ON c.CNT_ID = c2.CNT_ID " +
                    "WHERE c2.CNT_NOMBRE = @NombreCantante) " +
                    "AS a, " +
                "(SELECT c.CAN_ID, g.GNR_NOMBRE, c.CAT_ID FROM mus_canciones c " +
                    "LEFT JOIN mus_generos g ON c.GNR_ID = g.GNR_ID) " +
                    "AS g " +
                    "LEFT JOIN mus_categorias c2 ON g.CAT_ID = c2.CAT_ID " +
                "WHERE c.CAN_ID = a.CAN_ID AND c.CAN_ID = g.CAN_ID " +
                "ORDER BY " +
                    "CASE @Orden " +
                        "WHEN 'Clave' THEN c.CAN_CLAVE " +
                        "WHEN 'Cantante' THEN a.CNT_NOMBRE " +
                        "WHEN 'Titulo' THEN c.CAN_TITULO " +
                    "END;";
        }

        public static List<Cancion> filtrarPorCategoria(String nombreCategoria, String orden, String estado)
        {
            List<Cancion> canciones = new List<Cancion>();
            SqlConnection conexion = ConexionBD.getConnection();
            String consulta = obtenerConsultaConCategoriaYEstado();
            if ((nombreCategoria == "*") && (estado == "Todas"))
            {
                consulta = obtenerConsultaSinFiltrosYEstado();
            }
            else if (nombreCategoria == "*")
            {
                consulta = obtenerConsultaSinFiltrosConEstado();
            }
            else if (estado == "Todas")
            {
                consulta = obtenerConsultaConCategoriaSinEstado();
            }
            estado = convertirEstadoAClausula(estado);
            if (conexion != null)
            {
                try
                {
                    SqlCommand comando;
                    SqlDataReader lectorDatos;
                    comando = new SqlCommand(consulta, conexion);
                    if (nombreCategoria != "*")
                    {
                        comando.Parameters.AddWithValue("@NombreCategoria", nombreCategoria);
                    }
                    comando.Parameters.AddWithValue("@Orden", orden);
                    if (estado != "Todas")
                    {
                        comando.Parameters.AddWithValue("@Estado", estado);
                    }
                    lectorDatos = comando.ExecuteReader();
                    while (lectorDatos.Read())
                    {
                        canciones.Add(crearCancion(lectorDatos));
                    }
                    lectorDatos.Close();
                    comando.Dispose();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return canciones = null;
                }
                finally
                {
                    if (conexion != null)
                    {
                        conexion.Close();
                    }
                }
            }
            return canciones;
        }

        private static String obtenerConsultaConCategoriaYEstado()
        {
            return "SELECT c.CAN_ID, c.CAN_CLAVE, c.CAN_TITULO, a.CNT_NOMBRE, g.GNR_NOMBRE, c2.CAT_NOMBRE FROM mus_canciones c, " +
                "(SELECT c.CAN_ID, c2.CNT_NOMBRE FROM mus_canciones c " +
                    "LEFT JOIN mus_cantantes c2 ON c.CNT_ID = c2.CNT_ID) " +
                    "AS a, " +
                "(SELECT c.CAN_ID, g.GNR_NOMBRE, c.CAT_ID FROM mus_canciones c " +
                    "LEFT JOIN mus_generos g ON c.GNR_ID = g.GNR_ID) " +
                    "AS g " +
                    "LEFT JOIN mus_categorias c2 ON g.CAT_ID = c2.CAT_ID " +
                "WHERE c.CAN_ID = a.CAN_ID AND c.CAN_ID = g.CAN_ID AND c2.CAT_NOMBRE = @NombreCategoria AND c.CAN_ESTATUS = @Estado " +
                "ORDER BY " +
                    "CASE @Orden " +
                        "WHEN 'Clave' THEN c.CAN_CLAVE " +
                        "WHEN 'Cantante' THEN a.CNT_NOMBRE " +
                        "WHEN 'Titulo' THEN c.CAN_TITULO " +
                    "END;";
        }

        private static String obtenerConsultaSinFiltrosYEstado()
        {
            return "SELECT c.CAN_ID, c.CAN_CLAVE, c.CAN_TITULO, a.CNT_NOMBRE, g.GNR_NOMBRE, c2.CAT_NOMBRE FROM mus_canciones c, " +
                "(SELECT c.CAN_ID, c2.CNT_NOMBRE FROM mus_canciones c " +
                    "LEFT JOIN mus_cantantes c2 ON c.CNT_ID = c2.CNT_ID) " +
                    "AS a, " +
                "(SELECT c.CAN_ID, g.GNR_NOMBRE, c.CAT_ID FROM mus_canciones c " +
                    "LEFT JOIN mus_generos g ON c.GNR_ID = g.GNR_ID) " +
                    "AS g " +
                    "LEFT JOIN mus_categorias c2 ON g.CAT_ID = c2.CAT_ID " +
                "WHERE c.CAN_ID = a.CAN_ID AND c.CAN_ID = g.CAN_ID " +
                "ORDER BY " +
                    "CASE @Orden " +
                        "WHEN 'Clave' THEN c.CAN_CLAVE " +
                        "WHEN 'Cantante' THEN a.CNT_NOMBRE " +
                        "WHEN 'Titulo' THEN c.CAN_TITULO " +
                    "END;";
        }

        private static String obtenerConsultaSinFiltrosConEstado()
        {
            return "SELECT c.CAN_ID, c.CAN_CLAVE, c.CAN_TITULO, a.CNT_NOMBRE, g.GNR_NOMBRE, c2.CAT_NOMBRE FROM mus_canciones c, " +
                "(SELECT c.CAN_ID, c2.CNT_NOMBRE FROM mus_canciones c " +
                    "LEFT JOIN mus_cantantes c2 ON c.CNT_ID = c2.CNT_ID) " +
                    "AS a, " +
                "(SELECT c.CAN_ID, g.GNR_NOMBRE, c.CAT_ID FROM mus_canciones c " +
                    "LEFT JOIN mus_generos g ON c.GNR_ID = g.GNR_ID) " +
                    "AS g " +
                    "LEFT JOIN mus_categorias c2 ON g.CAT_ID = c2.CAT_ID " +
                "WHERE c.CAN_ID = a.CAN_ID AND c.CAN_ID = g.CAN_ID AND c.CAN_ESTATUS = @Estado " +
                "ORDER BY " +
                    "CASE @Orden " +
                        "WHEN 'Clave' THEN c.CAN_CLAVE " +
                        "WHEN 'Cantante' THEN a.CNT_NOMBRE " +
                        "WHEN 'Titulo' THEN c.CAN_TITULO " +
                    "END;";
        }

        private static String obtenerConsultaConCategoriaSinEstado()
        {
            return "SELECT c.CAN_ID, c.CAN_CLAVE, c.CAN_TITULO, a.CNT_NOMBRE, g.GNR_NOMBRE, c2.CAT_NOMBRE FROM mus_canciones c, " +
                "(SELECT c.CAN_ID, c2.CNT_NOMBRE FROM mus_canciones c " +
                    "LEFT JOIN mus_cantantes c2 ON c.CNT_ID = c2.CNT_ID) " +
                    "AS a, " +
                "(SELECT c.CAN_ID, g.GNR_NOMBRE, c.CAT_ID FROM mus_canciones c " +
                    "LEFT JOIN mus_generos g ON c.GNR_ID = g.GNR_ID) " +
                    "AS g " +
                    "LEFT JOIN mus_categorias c2 ON g.CAT_ID = c2.CAT_ID " +
                "WHERE c.CAN_ID = a.CAN_ID AND c.CAN_ID = g.CAN_ID AND c2.CAT_NOMBRE = @NombreCategoria " +
                "ORDER BY " +
                    "CASE @Orden " +
                        "WHEN 'Clave' THEN c.CAN_CLAVE " +
                        "WHEN 'Cantante' THEN a.CNT_NOMBRE " +
                        "WHEN 'Titulo' THEN c.CAN_TITULO " +
                    "END;";
        }

        private static Cancion crearCancion(SqlDataReader lectorDatos)
        {
            Cancion cancion = new Cancion();
            cancion.CancionID = (!lectorDatos.IsDBNull(0)) ? (int) lectorDatos.GetInt64(0) : 0;
            cancion.Clave = (!lectorDatos.IsDBNull(1)) ? lectorDatos.GetString(1) : "";
            cancion.Titulo = (!lectorDatos.IsDBNull(2)) ? lectorDatos.GetString(2) : "";
            cancion.NombreCantante = (!lectorDatos.IsDBNull(3)) ? lectorDatos.GetString(3) : "";
            cancion.NombreGenero = (!lectorDatos.IsDBNull(4)) ? lectorDatos.GetString(4) : "";
            cancion.NombreCategoria = (!lectorDatos.IsDBNull(5)) ? lectorDatos.GetString(5) : "";
            return cancion;
        }

        public static List<Cancion> consultarCancionesNoFiltradas(String orden, String estado)
        {
            List<Cancion> canciones = new List<Cancion>();
            SqlConnection conexion = ConexionBD.getConnection();
            String consulta = "SELECT c.CAN_ID, c.CAN_CLAVE, c.CAN_TITULO, c2.CNT_NOMBRE FROM mus_canciones c " +
                    "LEFT JOIN mus_cantantes c2 ON c.CNT_ID = c2.CNT_ID " +
                "WHERE c.CAN_ESTATUS = @Estado " +
                "ORDER BY " +
                    "CASE @Orden " +
                        "WHEN 'Clave' THEN c.CAN_CLAVE " +
                        "WHEN 'Cantante' THEN c2.CNT_NOMBRE " +
                        "WHEN 'Titulo' THEN c.CAN_TITULO " +
                    "END;";
            if (estado == "Todas")
            {
                consulta = "SELECT c.CAN_ID, c.CAN_CLAVE, c.CAN_TITULO, c2.CNT_NOMBRE FROM mus_canciones c " +
                        "LEFT JOIN mus_cantantes c2 ON c.CNT_ID = c2.CNT_ID " +
                    "ORDER BY " +
                        "CASE @Orden " +
                            "WHEN 'Clave' THEN c.CAN_CLAVE " +
                            "WHEN 'Cantante' THEN c2.CNT_NOMBRE " +
                            "WHEN 'Titulo' THEN c.CAN_TITULO " +
                        "END;";
            }
            estado = convertirEstadoAClausula(estado);
            if (conexion != null)
            {
                try
                {
                    SqlCommand comando;
                    SqlDataReader lectorDatos;
                    comando = new SqlCommand(consulta, conexion);
                    comando.Parameters.AddWithValue("@Orden", orden);
                    if (estado != "Todas")
                    {
                        comando.Parameters.AddWithValue("@Estado", estado);
                    }
                    lectorDatos = comando.ExecuteReader();
                    while (lectorDatos.Read())
                    {
                        Cancion cancion = new Cancion();
                        cancion.CancionID = (!lectorDatos.IsDBNull(0)) ? (int) lectorDatos.GetInt64(0) : 0;
                        cancion.Clave = (!lectorDatos.IsDBNull(1)) ? lectorDatos.GetString(1) : "";
                        cancion.Titulo = (!lectorDatos.IsDBNull(2)) ? lectorDatos.GetString(2) : "";
                        cancion.NombreCantante = (!lectorDatos.IsDBNull(3)) ? lectorDatos.GetString(3) : "";
                        canciones.Add(cancion);
                    }
                    lectorDatos.Close();
                    comando.Dispose();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return canciones = null;
                }
                finally
                {
                    if (conexion != null)
                    {
                        conexion.Close();
                    }
                }
            }
            return canciones;
        }

        private static String convertirEstadoAClausula(String estado)
        {
            if (estado == "Activas")
            {
                estado = "1";
            }
            else if (estado == "Inactivas")
            {
                estado = "0";
            }
            return estado;
        }
    }
}