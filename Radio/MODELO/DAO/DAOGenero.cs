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
    class DAOGenero
    {
        public static List<Genero> obtenerGeneros()
        {
            List<Genero> generos = new List<Genero>();
            SqlConnection conn = null;

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    SqlDataReader dataReader;

                    String query = "SELECT * FROM mus_generos;";

                    command = new SqlCommand(query, conn);
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        Genero genero = new Genero();

                        genero.IdGenero = (int)((!dataReader.IsDBNull(0)) ? dataReader.GetInt64(0) : 0);
                        genero.NombreGenero = (!dataReader.IsDBNull(1)) ? dataReader.GetString(1) : "";
                        generos.Add(genero);

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
            return generos;
        }
        
        public static List<TimeSpan> obtenerHorasDia(int dia)
        {
            List<TimeSpan> horasDia = new List<TimeSpan>();



            return horasDia;
        }

        /////////////////////////////////////////////////////////////////////////
        ///
        public static List<Generos> getGenero()
        {
            List<Generos> generos = new List<Generos>();
            SqlConnection conn = null;

            try
            {
                conn = ConexionBD.getConnection();

                if (conn != null)
                {
                    SqlCommand command;
                    SqlDataReader dataReader;

                    String query = "SELECT G.GNR_ID, G.GNR_NOMBRE " +
                        "FROM mus_generos G; ";
                    Console.WriteLine(query);
                    command = new SqlCommand(query, conn);
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        Generos genero = new Generos();
                        genero.GNR_ID1 = (int)((!dataReader.IsDBNull(0)) ? dataReader.GetInt64(0) : 0);
                        genero.GNR_NOMBRE1 = (!dataReader.IsDBNull(1)) ? dataReader.GetString(1) : "";
                        generos.Add(genero);
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

            return generos;
        }

        internal static int campoDuplicado()
        {
            throw new NotImplementedException();
        }

        public static int campoDuplicado(String campo)
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
                    String query = String.Format("SELECT COUNT(*) FROM mus_generos WHERE GNR_NOMBRE = '{0}' ", campo);
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

        public static int getId()
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
                    String query = "SELECT MAX(GNR_ID) FROM mus_generos; ";
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

        public static String guardarGenero(Generos genero)
        {
            SqlConnection conn = null;
            String valor = "";

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    String query = String.Format("INSERT INTO mus_generos " +
                            "VALUES('{0}', '{1}'); ", genero.GNR_ID1, genero.GNR_NOMBRE1);
                    command = new SqlCommand(query, conn);
                    int validar = command.ExecuteNonQuery();
                    if (validar == 1)
                    {
                        valor = "Género registrado con éxito.";
                    }
                    else
                    {
                        valor = "No es posible registrar el género, favor de verificar...";
                    }
                }
            }
            catch (Exception e)
            {
                valor = "Error al registrar un género...";
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

        public static String modificarGenero(Generos genero)
        {
            SqlConnection conn = null;
            String valor = "";

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    String query = String.Format("UPDATE mus_generos " +
                            "SET GNR_NOMBRE = '{0}' " +
                            "WHERE GNR_ID = {1}; ", genero.GNR_NOMBRE1, genero.GNR_ID1);
                    command = new SqlCommand(query, conn);
                    int validar = command.ExecuteNonQuery();
                    if (validar == 1)
                    {
                        valor = "Género actualizado con éxito.";
                    }
                    else
                    {
                        valor = "No es posible actualizar el género, favor de verificar...";
                    }
                }
            }
            catch (Exception e)
            {
                valor = "Error al actualizar el género...";
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

        public static bool encontrarGenero(String nombre)
        {
            bool existeGenero = false;
            SqlConnection conexion = ConexionBD.getConnection();
            if (conexion != null)
            {
                try
                {
                    SqlCommand comando;
                    SqlDataReader lectorDatos;
                    String consulta = "SELECT * FROM mus_generos " +
                        "WHERE GNR_NOMBRE = @Nombre;";
                    comando = new SqlCommand(consulta, conexion);
                    comando.Parameters.AddWithValue("@Nombre", nombre);
                    lectorDatos = comando.ExecuteReader();
                    if (lectorDatos.Read())
                    {
                        existeGenero = true;
                    }
                    lectorDatos.Close();
                    comando.Dispose();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return existeGenero;
                }
                finally
                {
                    if (conexion != null)
                    {
                        conexion.Close();
                    }
                }
            }
            return existeGenero;
        }
    }
}
