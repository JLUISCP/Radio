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
    class DAOCategoria
    {
        public static List<Categoria> obtenerCategorias()
        {
            List<Categoria> categorias = new List<Categoria>();
            SqlConnection conn = null;

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    SqlDataReader dataReader;

                    String query = "SELECT * FROM mus_categorias;";

                    command = new SqlCommand(query, conn);
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        Categoria categoria = new Categoria();

                        categoria.IdCategoria = (int)((!dataReader.IsDBNull(0)) ? dataReader.GetInt64(0) : 0);
                        categoria.NombreCategoria = (!dataReader.IsDBNull(1)) ? dataReader.GetString(1) : "";
                        categorias.Add(categoria);

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

            return categorias;
        }

        ////////////////////////////////////////////////////////

        public static List<Categorias> getCategoria()
        {
            List<Categorias> categorias = new List<Categorias>();
            SqlConnection conn = null;

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    SqlDataReader dataReader;

                    String query = "SELECT C.CAT_ID, C.CAT_NOMBRE " +
                        "FROM mus_categorias C; ";
                    Console.WriteLine(query);
                    command = new SqlCommand(query, conn);
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        Categorias cat = new Categorias();
                        cat.CAT_ID1 = (int)((!dataReader.IsDBNull(0)) ? dataReader.GetInt64(0) : 0);
                        cat.CAT_NOMBRE1 = (!dataReader.IsDBNull(1)) ? dataReader.GetString(1) : "";
                        categorias.Add(cat);
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

            return categorias;
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
                    String query = String.Format("SELECT COUNT(*) FROM mus_categorias WHERE CAT_NOMBRE = '{0}' ", campo);
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
                    String query = "SELECT MAX(CAT_ID) FROM mus_categorias; ";
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

        public static String guardarCategoria(Categorias categoria)
        {
            SqlConnection conn = null;
            String valor = "";

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    String query = String.Format("INSERT INTO mus_categorias " +
                        "VALUES('{0}', '{1}'); ", categoria.CAT_ID1, categoria.CAT_NOMBRE1);
                    Console.WriteLine(query);
                    command = new SqlCommand(query, conn);
                    int validar = command.ExecuteNonQuery();
                    if (validar == 1)
                    {
                        valor = "Categoría registrada con éxito.";
                    }
                    else
                    {
                        valor = "No es posible registrar la categoria, favor de verificar...";
                    }
                }
            }
            catch (Exception e)
            {
                valor = "Error al registrar una categoría...";
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

        public static String modificarCategoria(Categorias categoria)
        {
            SqlConnection conn = null;
            String valor = "";

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    String query = String.Format("UPDATE mus_categorias " +
                        "SET CAT_NOMBRE = '{0}' " +
                        "WHERE CAT_ID = {1}; ", categoria.CAT_NOMBRE1, categoria.CAT_ID1);
                    command = new SqlCommand(query, conn);
                    int validar = command.ExecuteNonQuery();
                    if (validar == 1)
                    {
                        valor = "Categoria actualizada con éxito.";
                    }
                    else
                    {
                        valor = "No es posible actualizar la categoria, favor de verificar...";
                    }
                }
            }
            catch (Exception e)
            {
                valor = "Error al actualizar la categoria...";
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

        public static bool encontrarCategoria(String nombre)
        {
            bool existeCategoria = false;
            SqlConnection conexion = ConexionBD.getConnection();
            if (conexion != null)
            {
                try
                {
                    SqlCommand comando;
                    SqlDataReader lectorDatos;
                    String consulta = "SELECT * FROM mus_categorias " +
                        "WHERE CAT_NOMBRE = @Nombre;";
                    comando = new SqlCommand(consulta, conexion);
                    comando.Parameters.AddWithValue("@Nombre", nombre);
                    lectorDatos = comando.ExecuteReader();
                    if (lectorDatos.Read())
                    {
                        existeCategoria = true;
                    }
                    lectorDatos.Close();
                    comando.Dispose();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return existeCategoria;
                }
                finally
                {
                    if (conexion != null)
                    {
                        conexion.Close();
                    }
                }
            }
            return existeCategoria;
        }
    }
}
