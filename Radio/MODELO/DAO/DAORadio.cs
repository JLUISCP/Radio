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
    class DAORadio
    {
        public static RadioPrograma unicoRadioExistente()
        {
            SqlConnection conn = null;
            SqlDataReader datosRadio = null;
            RadioPrograma unicoRadio = new RadioPrograma();

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    String query = "SELECT * FROM mus_estacionesradios; ";
                    command = new SqlCommand(query, conn);
                    datosRadio = command.ExecuteReader();
                    if (datosRadio.Read())
                    {
                        unicoRadio.IdRadio = (!datosRadio.IsDBNull(0)) ? datosRadio.GetInt32(0) : 0;
                        unicoRadio.Nombre = (!datosRadio.IsDBNull(1)) ? datosRadio.GetString(1) : "";
                    }
                    datosRadio.Close();
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
            return unicoRadio;
        }
    }
}
