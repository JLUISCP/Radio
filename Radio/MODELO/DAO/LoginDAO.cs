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
    class LoginDAO
    {
        public static Usuario getLogin(String nombreUsuario, String contraseña)
        {
            Usuario user = null;
            SqlConnection conn = null;
            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    SqlDataReader sqlDataReader;
                    String query = String.Format("SELECT U.U_ID, U.U_NOMBRE, U.U_USERNAME, U.U_PASSWORD, U.RAD_ID, U.ROL_ID " +
                        "FROM dbo.mus_usuarios U " +
                        "WHERE U.U_USERNAME = '{0}' AND U.U_PASSWORD = '{1}'; ", nombreUsuario, contraseña);
                    Console.WriteLine(query);
                    command = new SqlCommand(query, conn);
                    sqlDataReader = command.ExecuteReader();
                    if (sqlDataReader.Read())
                    {
                        user = new Usuario();
                        user.IdUsuario = (int)((!sqlDataReader.IsDBNull(0)) ? sqlDataReader.GetInt64(0) : 0);
                        user.Nombre = (!sqlDataReader.IsDBNull(1)) ? sqlDataReader.GetString(1) : "";
                        user.NombreUsuario = (!sqlDataReader.IsDBNull(2)) ? sqlDataReader.GetString(2) : "";
                        user.Contraseña = (!sqlDataReader.IsDBNull(3)) ? sqlDataReader.GetString(3) : "";
                        user.IdRadio = (!sqlDataReader.IsDBNull(4)) ? sqlDataReader.GetInt32(4) : 0;
                        user.IdRol = (int)((!sqlDataReader.IsDBNull(5)) ? sqlDataReader.GetInt64(5) : 0);
                    }
                    sqlDataReader.Close();
                    command.Dispose();
                    Console.WriteLine(user);
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
            return user;
        }
    }
}
