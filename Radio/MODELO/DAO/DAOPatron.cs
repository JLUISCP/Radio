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
    class DAOPatron
    {
       public static List<Patron> getPatrones()
        {
            List<Patron> patrones = new List<Patron>();


            SqlConnection conn = null;
            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    SqlDataReader sqlDataReader;
                    String query = "SELECT mus_patrones.PAT_ID, mus_patrones.PAD_NOMBRE, mus_patrones.RAD_ID, mus_estacionesradios.RAD_NOMBRE FROM mus_patrones LEFT JOIN " +
                                   "mus_estacionesradios ON mus_patrones.RAD_ID = mus_estacionesradios.RAD_ID;";

                    command = new SqlCommand(query, conn);
                    sqlDataReader = command.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Patron patron = new Patron();

                        patron.IdPatron = (!sqlDataReader.IsDBNull(0)) ? sqlDataReader.GetInt32(0) : -1;
                        patron.NombrePatron = (!sqlDataReader.IsDBNull(1)) ? sqlDataReader.GetString(1) : "";
                        patron.IdRadio = (!sqlDataReader.IsDBNull(2)) ? sqlDataReader.GetInt32(2) : 0;
                        patron.NombreRadio = (!sqlDataReader.IsDBNull(3)) ? sqlDataReader.GetString(3) : "";

                        patron.LineaPatron = DAOLineaPatron.obtenerLineasPatron(patron.IdPatron);

                        patrones.Add(patron);
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

            return patrones;

        }

        public static int setPatron(String nombrePatron, int idRadio, List<LineaPatron> lineaPatron)
        {

            int resultadoQuery = 0;

            SqlConnection conn = null;

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    String query = "INSERT INTO mus_patrones(PAD_NOMBRE, RAD_ID) VALUES (@PAD_NOMBRE, @RAD_ID)";

                    command = new SqlCommand(query, conn);
                    command.Parameters.Add(new SqlParameter("@PAD_NOMBRE", nombrePatron));
                    command.Parameters.Add(new SqlParameter("@RAD_ID", idRadio));

                    resultadoQuery = command.ExecuteNonQuery();
                    command.Dispose();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR" + e.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

            if (resultadoQuery == 1)
            {
                conn = null;
                try
                {
                    conn = ConexionBD.getConnection();
                    if (conn != null)
                    {

                        SqlCommand command;
                        String query = "INSERT INTO mus_lineapatron (PAT_ID, CAT_ID, GNR_ID, LP_PRIORIDAD) VALUES (@PAT_ID, @CAT_ID, @GNR_ID, @LP_PRIORIDAD)";

                        int idRegistrado = ultimoIdRegistradoPatron() - 1;

                        foreach(LineaPatron linea in lineaPatron)
                        {
                            command = new SqlCommand(query, conn);

                            command.Parameters.Add(new SqlParameter("@PAT_ID", idRegistrado));
                            command.Parameters.Add(new SqlParameter("@CAT_ID", linea.IdCategoria));
                            command.Parameters.Add(new SqlParameter("@GNR_ID", linea.IdGenero));
                            command.Parameters.Add(new SqlParameter("@LP_PRIORIDAD", linea.PrioridadPatron));

                            resultadoQuery = command.ExecuteNonQuery();

                            command.Dispose();
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR" + e.Message);
                }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }
            }
            return resultadoQuery;
        }

        internal static int eliminarPatron(int idPatron)
        {
            SqlConnection conn = null;
            int respuestaConsulta = 0;
            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;

                    String query = String.Format("DELETE FROM mus_patrones WHERE PAT_ID = '{0}';", idPatron);

                    command = new SqlCommand(query, conn);
                    respuestaConsulta = command.ExecuteNonQuery();
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
            return respuestaConsulta;
        }

        public static int ultimoIdRegistradoPatron()
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
                    String query = "SELECT MAX(PAT_ID) FROM mus_patrones; ";
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
            return valor + 1;
        }

        internal static int updatePatron(int idPatron, String nombrePatron, List<LineaPatron> listaPatron)
        {
            int resultadoQuery = 0;

            SqlConnection conn = null;

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    String query = "UPDATE mus_patrones SET PAD_NOMBRE = @PAD_NOMBRE WHERE PAT_ID = @PAT_ID;";
                    
                    command = new SqlCommand(query, conn);

                    command.Parameters.Add(new SqlParameter("@PAD_NOMBRE", nombrePatron));
                    command.Parameters.Add(new SqlParameter("@PAT_ID", idPatron));

                    resultadoQuery = command.ExecuteNonQuery();
                    command.Dispose();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR" + e.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

            conn = null;
            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {

                    SqlCommand command;
                    String query = "INSERT INTO mus_lineapatron (PAT_ID, CAT_ID, GNR_ID, LP_PRIORIDAD) VALUES (@PAT_ID, @CAT_ID, @GNR_ID, @LP_PRIORIDAD)";

                    foreach (LineaPatron linea in listaPatron)
                    {
                        command = new SqlCommand(query, conn);

                        command.Parameters.Add(new SqlParameter("@PAT_ID", idPatron));
                        command.Parameters.Add(new SqlParameter("@CAT_ID", linea.IdCategoria));
                        command.Parameters.Add(new SqlParameter("@GNR_ID", linea.IdGenero));
                        command.Parameters.Add(new SqlParameter("@LP_PRIORIDAD", linea.PrioridadPatron));

                        resultadoQuery = command.ExecuteNonQuery();

                        command.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR" + e.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return resultadoQuery;
        }
    }
}
