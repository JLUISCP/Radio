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
    class DAOProgramaPatron
    {

        public static ProgramaPatron getProgramaPatron(int idDia)
        {
            ProgramaPatron programaPatron = new ProgramaPatron();

            SqlConnection conn = null;

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    SqlDataReader dataReader;

                    String query = String.Format("SELECT * FROM mus_programapatron WHERE HOR_ID = '{0}';", idDia);

                    command = new SqlCommand(query, conn);
                    dataReader = command.ExecuteReader();

                    if (dataReader.Read())
                    {
                        programaPatron.IdprogramaPatron = (!dataReader.IsDBNull(0)) ? dataReader.GetInt32(0) : 0;
                        programaPatron.IdDia = (!dataReader.IsDBNull(1)) ? dataReader.GetInt32(1) : 0;
                        programaPatron.IdPatron = (!dataReader.IsDBNull(2)) ? dataReader.GetInt32(2) : 0;
                        programaPatron.IdCancionActual = (int)((!dataReader.IsDBNull(0)) ? dataReader.GetInt64(0) : 0);
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

            return programaPatron;
        }

        public static int deleteProgramaPatron(int idDia)
        {
            SqlConnection conn = null;
            int resultQuery = 0;

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    String query = String.Format("DELETE FROM mus_programapatron WHERE HOR_ID = {0}; ", idDia);

                    command = new SqlCommand(query, conn);

                    resultQuery = command.ExecuteNonQuery();
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
            return resultQuery;
        }
    }
}
