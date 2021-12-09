using Radio.MODELO.DB;
using Radio.MODELO.POCO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radio.MODELO.DAO
{
    class DAOCantantes

    {
        public static List<Cantantes> getCantante()
        {
            List<Cantantes> cantantes = new List<Cantantes>();
            SqlConnection conn = null;

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    SqlDataReader dataReader;
                    String query = "SELECT CNT_ID, CNT_NOMBRE, CNT_TIPO, CNT_FECHAMODIFICACION, CNT_FECHAALTA, CNT_ESTATUS " +
                        "FROM mus_cantantes; ";
                    command = new SqlCommand(query, conn);
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Cantantes cantante = new Cantantes();
                        cantante.CNT_ID1 = (int)((!dataReader.IsDBNull(0)) ? dataReader.GetInt64(0) : 0);
                        cantante.CNT_NOMBRE1 = (!dataReader.IsDBNull(1)) ? dataReader.GetString(1) : "";
                        cantante.CNT_TIPO1 = (!dataReader.IsDBNull(2)) ? dataReader.GetString(2) : "";
                        cantante.CNT_FECHAMODIFICACION1 = (!dataReader.IsDBNull(3)) ? dataReader.GetDateTime(3) : DateTime.MinValue;
                        cantante.CNT_FECHAALTA1 = (!dataReader.IsDBNull(4)) ? dataReader.GetDateTime(4) : DateTime.MinValue;
                        cantante.CNT_ESTATUS1 = (!dataReader.IsDBNull(5)) ? dataReader.GetString(5) : "";
                        cantantes.Add(cantante);
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

            return cantantes;
        }

        public static int camposDuplicados(String nombre, String tipo, String estatus)
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
                    String query = String.Format("SELECT COUNT(*) FROM mus_cantantes WHERE CNT_NOMBRE = '{0}' " +
                    "AND CNT_TIPO = '{1}' AND CNT_ESTATUS = '{2}' ", nombre, tipo, estatus);
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
                    String query = "SELECT MAX(CNT_ID) FROM mus_cantantes; ";
                    command = new SqlCommand(query, conn);
                    numero = command.ExecuteReader();
                    if (numero.Read())
                    {
                        valor = (int)((!numero.IsDBNull(0)) ? numero.GetInt64(0) : 0);
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

        public static String guardarCantante(Cantantes cantante)
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
                    String query = String.Format("INSERT INTO mus_cantantes (CNT_ID, CNT_NOMBRE, CNT_TIPO, CNT_FECHAMODIFICACION, CNT_FECHAALTA, CNT_ESTATUS) " +
                        "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}'); ", cantante.CNT_ID1, cantante.CNT_NOMBRE1, cantante.CNT_TIPO1, nuevaFecha, nuevaFecha, cantante.CNT_ESTATUS1);
                    command = new SqlCommand(query, conn);
                    int validar = command.ExecuteNonQuery();
                    if (validar == 1)
                    {
                        valor = "Cantante registrado con éxito. ";
                    }
                    else
                    {
                        valor = "No es posoble registrar el cantante o grupo, favor de verificar...";
                    }
                    command.Dispose();
                }
            }
            catch (Exception e)
            {
                valor = "Error al registrar un cantante...";
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

        public static String modificarCantante(Cantantes cantante)
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
                    String query = String.Format("UPDATE mus_cantantes SET CNT_NOMBRE = '{0}', CNT_TIPO = '{1}', " +
                        "CNT_FECHAMODIFICACION = '{2}', CNT_ESTATUS = '{3}' " +
                        "WHERE CNT_ID = {4}; ", cantante.CNT_NOMBRE1, cantante.CNT_TIPO1, nuevaFecha, cantante.CNT_ESTATUS1, cantante.CNT_ID1);
                    command = new SqlCommand(query, conn);
                    int validar = command.ExecuteNonQuery();
                    if (validar == 1)
                    {
                        valor = "Cantante actualizado con éxito.";
                        if (cantante.CNT_ESTATUS1 == "ACTIVO")
                        {
                            DAOCanciones.cambiarEstatusCancion(1, (int)cantante.CNT_ID1);
                        }
                        else
                        {
                            DAOCanciones.cambiarEstatusCancion(0, (int)cantante.CNT_ID1);
                        }
                    }
                    else
                    {
                        valor = "No es posible actualizar el cantante, Favor de verificar...";
                    }
                    command.Dispose();
                }
            }
            catch (Exception e)
            {
                valor = "Error al actualizar el cantante...";
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

        public static String eliminarCantante(Int64 idCantante)
        {
            SqlConnection conn = null;
            String valor = "";

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    String query = String.Format("DELETE FROM mus_cantantes WHERE CNT_ID = '{0}' ", idCantante);
                    command = new SqlCommand(query, conn);
                    int validar = command.ExecuteNonQuery();
                    if (validar == 1)
                    {
                        valor = "El cantante se ha eliminado con éxito.";
                    }
                    else
                    {
                        valor = "No es posible eliminar el cantante. Intente de nuevo...";
                    }
                    command.Dispose();
                }
            }
            catch (Exception e)
            {
                valor = "Actualmente el cantante tiene canciones asignadas por lo que no se puede eliminar. " +
                    "Intenta de nuevo asignando un nuevo artista a las canciones...";
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

        public static bool encontrarCantante(String nombre)
        {
            bool existeCantante = false;
            SqlConnection conexion = ConexionBD.getConnection();
            if (conexion != null)
            {
                try
                {
                    SqlCommand comando;
                    SqlDataReader lectorDatos;
                    String consulta = "SELECT * FROM mus_cantantes " +
                        "WHERE CNT_NOMBRE = @Nombre;";
                    comando = new SqlCommand(consulta, conexion);
                    comando.Parameters.AddWithValue("@Nombre", nombre);
                    lectorDatos = comando.ExecuteReader();
                    if (lectorDatos.Read())
                    {
                        existeCantante = true;
                    }
                    lectorDatos.Close();
                    comando.Dispose();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return existeCantante;
                }
                finally
                {
                    if (conexion != null)
                    {
                        conexion.Close();
                    }
                }
            }
            return existeCantante;
        }
    }
}
