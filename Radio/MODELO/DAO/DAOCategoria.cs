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
    }
}
