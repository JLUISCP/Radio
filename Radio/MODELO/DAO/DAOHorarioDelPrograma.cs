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
    class DAOHorarioDelPrograma
    {
        public static List<HorarioDelPrograma> consultarHorarios(int idPrograma)
        {
            List<HorarioDelPrograma> horarios = new List<HorarioDelPrograma>();
            SqlConnection conexion = ConexionBD.getConnection();
            if (conexion != null)
            {
                try
                {
                    String consulta = "SELECT h.HOR_ID, h.HOR_NUMCANCIONES, h.HOR_DIASSEMANA, h.HOR_HORAINICIO, h.HOR_HORAFIN, p.PAD_NOMBRE FROM mus_horarios h " +
                        "LEFT JOIN mus_patrones p ON h.PAT_ID = p.PAT_ID " +
                        "WHERE PRO_ID = @IdPrograma AND HOR_ESTATUS = 1;";
                    SqlCommand comando;
                    SqlDataReader lectorDatos;
                    comando = new SqlCommand(consulta, conexion);
                    comando.Parameters.AddWithValue("@IdPrograma", idPrograma);
                    lectorDatos = comando.ExecuteReader();
                    while (lectorDatos.Read())
                    {
                        HorarioDelPrograma horario = new HorarioDelPrograma();
                        horario.IdHorario = (!lectorDatos.IsDBNull(0)) ? lectorDatos.GetInt32(0) : 0;
                        horario.NumeroCanciones = (!lectorDatos.IsDBNull(1)) ? lectorDatos.GetInt32(1) : 0;
                        if (!lectorDatos.IsDBNull(2))
                        {
                            int diaCosultada = lectorDatos.GetInt32(2);
                            if (diaCosultada == 1)
                            {
                                horario.DiaDeSemana = "Lunes";
                            }
                            else if (diaCosultada == 2)
                            {
                                horario.DiaDeSemana = "Martes";
                            }
                            else if (diaCosultada == 3)
                            {
                                horario.DiaDeSemana = "Miercóles";
                            }
                            else if (diaCosultada == 4)
                            {
                                horario.DiaDeSemana = "Jueves";
                            }
                            else if (diaCosultada == 5)
                            {
                                horario.DiaDeSemana = "Viernes";
                            }
                            else if (diaCosultada == 6)
                            {
                                horario.DiaDeSemana = "Sábado";
                            }
                            else if (diaCosultada == 7)
                            {
                                horario.DiaDeSemana = "Domingo";
                            }
                            else
                            {
                                horario.DiaDeSemana = "";
                            }
                        }
                        horario.HoraInicio = (!lectorDatos.IsDBNull(3)) ? lectorDatos.GetTimeSpan(3).ToString(@"h\:mm") : "";
                        horario.HoraFin = (!lectorDatos.IsDBNull(4)) ? lectorDatos.GetTimeSpan(4).ToString(@"h\:mm") : "";
                        horario.NombrePatron = (!lectorDatos.IsDBNull(5)) ? lectorDatos.GetString(5) : "";
                        horarios.Add(horario);
                    }
                    lectorDatos.Close();
                    comando.Dispose();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return horarios = null;
                }
                finally
                {
                    if (conexion != null)
                    {
                        conexion.Close();
                    }
                }
            }
            return horarios;
        }
    }
}
