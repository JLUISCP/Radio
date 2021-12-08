using Radio.MODELO.DB;
using Radio.MODELO.POCO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Radio.MODELO.DAO
{
    class RolDAO
    {
        public static List<Rol> getRoles()
        {
            List<Rol> roles = new List<Rol>();
            SqlConnection conn = null;
            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    SqlDataReader sqlDataReader;
                    String query = String.Format("SELECT R.ROl_ID, R.ROl_PUESTO FROM dbo.mus_ROL R WHERE R.ROL_ID <> 1;");
                    command = new SqlCommand(query, conn);
                    sqlDataReader = command.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        Rol rol = new Rol();
                        rol.IdRol = (int)((!sqlDataReader.IsDBNull(0)) ? sqlDataReader.GetInt64(0) : 0);
                        rol.Puesto = (!sqlDataReader.IsDBNull(1)) ? sqlDataReader.GetString(1) : "";
                        roles.Add(rol);
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
            return roles;
        }
    }
}
