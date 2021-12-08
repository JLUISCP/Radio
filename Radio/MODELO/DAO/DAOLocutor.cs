using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Radio.MODELO.DB;
using Radio.MODELO.POCO;

namespace Radio.MODELO.DAO
{
    class DAOLocutor
    {
        public static List<Locutor> getLocutor()
        {
            List<Locutor> locutores = new List<Locutor>();
            SqlConnection conn = null;

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    SqlDataReader dataReader;

                    String query = "SELECT LOC_ID, LOC_NOMBRE, mus_locutores.PRO_ID, CASE WHEN PRO_NOMBRE is null THEN 'Sin programa' " +
                                   "WHEN PRO_NOMBRE is not null THEN PRO_NOMBRE END FROM mus_locutores LEFT JOIN mus_programas " +
                                   "ON mus_locutores.PRO_ID = mus_programas.PRO_ID;";

                    command = new SqlCommand(query, conn);
                    dataReader = command.ExecuteReader();

                    int numero = 0;

                    while (dataReader.Read())
                    {
                        numero++;

                        Locutor locutor = new Locutor();

                        locutor.Numero = numero;
                        locutor.IdLocutor = (!dataReader.IsDBNull(0)) ? dataReader.GetInt32(0) : 0;
                        locutor.NombreLocutor = (!dataReader.IsDBNull(1)) ? dataReader.GetString(1) : "";
                        locutor.IdPrograma = (!dataReader.IsDBNull(2)) ? dataReader.GetInt32(2) : -1;
                        locutor.NombrePrograma = (!dataReader.IsDBNull(3)) ? dataReader.GetString(3) : "";
                        locutores.Add(locutor);
                        
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

            return locutores;
        }

        public static List<Locutor> obtenerLocutorPrograma(int idProgra)
        {
            List<Locutor> locutores = new List<Locutor>();
            SqlConnection conn = null;

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    SqlDataReader dataReader;

                    String query = String.Format("SELECT * FROM mus_locutores WHERE PRO_ID = '{0}';", idProgra);

                    command = new SqlCommand(query, conn);
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        Locutor locutor = new Locutor();

                        locutor.IdLocutor = (!dataReader.IsDBNull(0)) ? dataReader.GetInt32(0) : 0;
                        locutor.NombreLocutor = (!dataReader.IsDBNull(1)) ? dataReader.GetString(1) : "";
                        locutor.IdPrograma = (!dataReader.IsDBNull(2)) ? dataReader.GetInt32(2) : 0;
                        locutores.Add(locutor);

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

            return locutores;
        }

        public static int guardarLocutor(String locutor)
        {
            SqlConnection conn = null;
            int idRegistrado = 0;

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    SqlDataReader dataReader;

                    String query = String.Format("INSERT INTO mus_locutores (LOC_NOMBRE) VALUES('{0}'); ", locutor);
                    command = new SqlCommand(query, conn);
                    command.ExecuteNonQuery();

                    command.Dispose();
                    

                    String querySelect = "SELECT MAX(LOC_ID) FROM mus_locutores;";
                    command = new SqlCommand(querySelect, conn);
                    dataReader = command.ExecuteReader();

                    if(dataReader.Read())
                    {
                        idRegistrado = (!dataReader.IsDBNull(0)) ? dataReader.GetInt32(0) : 0;
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
            return idRegistrado;
        }

        public static int modificarNombrelocutor(int idLocutor, String Nombre)
        {
            SqlConnection conn = null;
            int valor = 0;

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    String query = String.Format("UPDATE mus_locutores SET LOC_NOMBRE = '{0}' WHERE LOC_ID = {1}; ", Nombre, idLocutor);
       
                    command = new SqlCommand(query, conn);
                    
                    valor = command.ExecuteNonQuery();
                    
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

        public static int modificarProgramaPerteneciente(int idLocutor, int idPrograma)
        {
            SqlConnection conn = null;
            int valor = 0;

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    String query = String.Format("UPDATE mus_locutores SET PRO_ID = '{0}' WHERE LOC_ID = {1}; ", idPrograma, idLocutor);

                    command = new SqlCommand(query, conn);

                    valor = command.ExecuteNonQuery();

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

        internal static int eliminarNombrelocutor(int idLocutor)
        {
            SqlConnection conn = null;
            int valor = 0;

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    String query = String.Format("DELETE FROM mus_locutores WHERE LOC_ID = {0}; ", idLocutor);

                    command = new SqlCommand(query, conn);

                    valor = command.ExecuteNonQuery();
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

        internal static int eliminarlocutorPrograma(int idPrograma)
        {
            SqlConnection conn = null;
            int valor = 0;

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    String query = String.Format("DELETE FROM mus_locutores WHERE PRO_ID = {0}; ", idPrograma);

                    command = new SqlCommand(query, conn);

                    valor = command.ExecuteNonQuery();
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

    }

}
