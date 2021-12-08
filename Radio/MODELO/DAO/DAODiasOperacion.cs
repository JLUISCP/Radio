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
    class DAODiasOperacion
    {
        public static List<DiasOperacion> obtenerDiasOperacionPrograma(int idProgra)
        {
            List<DiasOperacion> diasOperaciones = new List<DiasOperacion>();
            SqlConnection conn = null;

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    SqlDataReader dataReader;

                    String query = String.Format("SELECT * FROM mus_horarios WHERE PRO_ID = '{0}';", idProgra);

                    command = new SqlCommand(query, conn);
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        DiasOperacion diasOperacion = new DiasOperacion();

                        diasOperacion.IdDia = (!dataReader.IsDBNull(0)) ? dataReader.GetInt32(0) : 0;
                        diasOperacion.NumCanciones = (!dataReader.IsDBNull(1)) ? dataReader.GetInt32(1) : 0;
                        diasOperacion.NumeroDia = (!dataReader.IsDBNull(2)) ? dataReader.GetInt32(2) : 0;
                        diasOperacion.HoraFinal = (!dataReader.IsDBNull(3)) ? dataReader.GetTimeSpan(3) : TimeSpan.MinValue;
                        diasOperacion.HoraInicio = (!dataReader.IsDBNull(4)) ? dataReader.GetTimeSpan(4) : TimeSpan.MinValue;
                        diasOperacion.EstadoDia = (!dataReader.IsDBNull(5)) ? dataReader.GetInt32(5) : 0;
                        diasOperacion.IdPatron = (!dataReader.IsDBNull(6)) ? dataReader.GetInt32(6) : 0;
                        diasOperacion.IdProgramaDia = (!dataReader.IsDBNull(7)) ? dataReader.GetInt32(7) : 0;

                        diasOperacion.ProgramaPatron = DAOProgramaPatron.getProgramaPatron(diasOperacion.IdDia);

                        diasOperacion.CancionesProgramaPatron = DAOCanciones.obtenerCancionDiaPatron(diasOperacion.ProgramaPatron.IdprogramaPatron);

                        diasOperaciones.Add(diasOperacion);

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
            return diasOperaciones;
        }


        public static List<DiasOperacion> obtenerHorasDia(int dia, int idPrograma)
        {
            List<DiasOperacion> horasDia = new List<DiasOperacion>();

            SqlConnection conn = null;

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    SqlDataReader dataReader;

                    String query = String.Format("SELECT HOR_ID, HOR_HORAINICIO, HOR_HORAFIN, HOR_ESTATUS FROM  mus_horarios WHERE HOR_DIASSEMANA = '{0}' AND PRO_ID != '{1}';", dia, idPrograma);

                    command = new SqlCommand(query, conn);
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        DiasOperacion diasOperacion = new DiasOperacion();

                        diasOperacion.IdDia = (!dataReader.IsDBNull(0)) ? dataReader.GetInt32(0) : 0;
                        diasOperacion.HoraInicio = (!dataReader.IsDBNull(1)) ? dataReader.GetTimeSpan(1) : TimeSpan.MinValue;
                        diasOperacion.HoraFinal = (!dataReader.IsDBNull(2)) ? dataReader.GetTimeSpan(2) : TimeSpan.MinValue;
                        diasOperacion.EstadoDia = (!dataReader.IsDBNull(3)) ? dataReader.GetInt32(3) : 0;

                        horasDia.Add(diasOperacion);
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

            return horasDia;
        }
    }
}
