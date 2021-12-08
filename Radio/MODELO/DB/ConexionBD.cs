using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radio.MODELO.DB
{
    class ConexionBD
    {
        private static String SERVER = "estacionradiodb.database.windows.net";
        private static String PORT = "1433";
        private static String DATABASE = "musica";
        private static String USER = "sqlserver";
        private static String PASSWORD = "100radio+";

        public static SqlConnection getConnection()

        {
            SqlConnection conn = null;
            try
            {
                String urlconn = String.Format("Data Source={0},{1};" + "Network Library= DBMSSOCN;" +
                "Initial Catalog={2};" + "User ID={3};" + "Password={4};", SERVER, PORT, DATABASE, USER, PASSWORD);
                conn = new SqlConnection(urlconn);
                conn.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return conn;
        }
    }
}
