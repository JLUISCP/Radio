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
    class DAOContadorDeCancion
    {
        public static List<ContadorDeCancion> consultarCancionesNoUtilizadas()
        {
            List<ContadorDeCancion> contadores = new List<ContadorDeCancion>();
            SqlConnection conexion = ConexionBD.getConnection();
            if (conexion != null)
            {
                try
                {
                    String consulta = "SELECT COUNT(c.CAN_ID), a.GNR_NOMBRE, b.CAT_NOMBRE FROM mus_canciones c " +
                            "LEFT JOIN mus_musicadiapatron mdp ON c.CAN_ID = mdp.CAN_ID, " +
                        "(SELECT * FROM mus_generos) " +
                            "AS a, " +
                        "(SELECT * FROM mus_categorias) " +
                            "AS b " +
                        "WHERE c.GNR_ID = a.GNR_ID AND c.CAT_ID = b.CAT_ID AND mdp.CAN_ID is null " +
                        "GROUP BY a.GNR_NOMBRE, b.CAT_NOMBRE;";
                    SqlCommand comando;
                    SqlDataReader lectorDatos;
                    comando = new SqlCommand(consulta, conexion);
                    lectorDatos = comando.ExecuteReader();
                    int iterador = 1;
                    while (lectorDatos.Read())
                    {
                        ContadorDeCancion contador = new ContadorDeCancion();
                        contador.IdContador = iterador;
                        contador.TotalCanciones = (!lectorDatos.IsDBNull(0)) ? lectorDatos.GetInt32(0) : 0;
                        contador.NombreGenero = (!lectorDatos.IsDBNull(1)) ? lectorDatos.GetString(1) : "";
                        contador.NombreCategoria = (!lectorDatos.IsDBNull(2)) ? lectorDatos.GetString(2) : "";
                        iterador++;
                        contadores.Add(contador);
                    }
                    lectorDatos.Close();
                    comando.Dispose();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return contadores = null;
                }
                finally
                {
                    if (conexion != null)
                    {
                        conexion.Close();
                    }
                }
            }
            return contadores;
        }
    }
}
