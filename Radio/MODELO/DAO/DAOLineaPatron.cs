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
    class DAOLineaPatron
    {
        public static List<LineaPatron> obtenerLineasPatron(int idPatron)
        {
            List<LineaPatron> lineasPatron = new List<LineaPatron>();
            SqlConnection conn = null;

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    SqlDataReader dataReader;

                    String query = String.Format("SELECT A.LP_ID, A.LP_PRIORIDAD, A.CAT_ID, A.GNR_ID, B.CAT_NOMBRE, C.GNR_NOMBRE FROM " +
                                                 "mus_lineapatron AS A LEFT JOIN mus_categorias AS B ON  A.CAT_ID = B.CAT_ID LEFT JOIN " +
                                                 "mus_generos AS C ON A.GNR_ID = C.GNR_ID WHERE A.PAT_ID = '{0}';", idPatron);

                    command = new SqlCommand(query, conn);
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        LineaPatron lineaPatron = new LineaPatron();

                        lineaPatron.IdLineaPatron = (!dataReader.IsDBNull(0)) ? dataReader.GetInt32(0) : 0;
                        lineaPatron.PrioridadPatron = (!dataReader.IsDBNull(1)) ? dataReader.GetInt32(1) : 0;
                        lineaPatron.IdCategoria = (int)((!dataReader.IsDBNull(2)) ? dataReader.GetInt64(2) : 0);
                        lineaPatron.IdGenero = (int)((!dataReader.IsDBNull(3)) ? dataReader.GetInt64(3) : 0);
                        lineaPatron.NombreCategoria = (!dataReader.IsDBNull(4)) ? dataReader.GetString(4) : "";
                        lineaPatron.NombreGenero = (!dataReader.IsDBNull(5)) ? dataReader.GetString(5) : "";

                        lineaPatron.NumeroCanciones = DAOCanciones.obtenerTotalCanciones(lineaPatron.IdCategoria, lineaPatron.IdGenero);

                        lineasPatron.Add(lineaPatron);
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
            return lineasPatron;
        }


        internal static int eliminarLineasPatron(int idPatron)
        {
            SqlConnection conn = null;
            int valor = 0;

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    String query = String.Format("DELETE FROM mus_lineapatron WHERE PAT_ID = {0}; ", idPatron);

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
