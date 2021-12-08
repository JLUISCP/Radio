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
    }
}
