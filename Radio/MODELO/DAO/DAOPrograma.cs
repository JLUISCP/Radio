using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Radio.MODELO.DB;
using Radio.MODELO.POCO;

namespace Radio.MODELO.DAO
{
    class DAOPrograma
    {  
        public static List<Programa> getProgramas()
        {
            List<Programa> programas = new List<Programa>();

            SqlConnection conn = null;
            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    SqlDataReader sqlDataReader;
                    String query = "SELECT PRO_ID, PRO_FECHABLOQUEADO, PRO_FECHAINICIO, PRO_NOMBRE, PRO_ESTATUS, PRO_LOGO, mus_programas.RAD_ID, RAD_NOMBRE " +
                                   "FROM mus_programas left join mus_estacionesradios on mus_programas.RAD_ID = mus_estacionesradios.RAD_ID; ";
                    
                    command = new SqlCommand(query, conn);
                    sqlDataReader = command.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        Programa programa = new Programa();
                        
                        programa.IdPrograma = (!sqlDataReader.IsDBNull(0)) ? sqlDataReader.GetInt32(0) : 0;
                        programa.FechaBloqueo = (!sqlDataReader.IsDBNull(1)) ? sqlDataReader.GetDateTime(1).ToString("dd-MM-yyyy") : DateTime.MinValue.ToString("dd-MM-yyyy");
                        programa.FechaInicio = (!sqlDataReader.IsDBNull(2)) ? sqlDataReader.GetDateTime(2).ToString("dd-MM-yyyy") : DateTime.MinValue.ToString("dd-MM-yyyy");
                        programa.NombrePrograma = (!sqlDataReader.IsDBNull(3)) ? sqlDataReader.GetString(3) : "";
                        programa.EstadoPrograma = (!sqlDataReader.IsDBNull(4)) ? sqlDataReader.GetInt32(4) : 0;
                        programa.Imagen = (byte[])sqlDataReader.GetValue(5);
                        programa.IdRadio = (!sqlDataReader.IsDBNull(6)) ? sqlDataReader.GetInt32(6) : 0;
                        programa.NombreRadio = (!sqlDataReader.IsDBNull(7)) ? sqlDataReader.GetString(7) : "";

                        if (programa.EstadoPrograma == 1)
                        {
                            programa.EstadoProgramaNombre = "Activo";
                        }

                        programa.Locutor = DAOLocutor.obtenerLocutorPrograma(programa.IdPrograma);
                        programa.DiasOperacion = DAODiasOperacion.obtenerDiasOperacionPrograma(programa.IdPrograma);


                        programas.Add(programa);
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
            return programas;
        }
        

        public static int setReporte(String programa, int estadoPrograma, byte[] imagen, int idRadio, int idLocutorUno, int idLocutorDos,  Boolean[] diaActivo,
                                     int[] idDia, int[] numCanciones, int[] numDia, TimeSpan[] horaInicio, TimeSpan[] horaFinal, Boolean[] estadoDia, 
                                     int[] idPatron, int[] clavePrograma, List<Cancion>[] listaCaciones)
        {
            String fechaActual = DateTime.Now.ToString("yyyy-MM-dd");
            String fechaVacia = "2000-01-01";
            int resultadoQuery = 0;
            int idRegistrado = -1;
            SqlConnection conn = null;
                
            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    String query = "INSERT INTO mus_programas (RAD_ID, PRO_ESTATUS, PRO_NOMBRE, PRO_FECHABLOQUEADO, PRO_FECHAINICIO, PRO_LOGO) VALUES (@RAD_ID, @PRO_ESTATUS ,@PRO_NOMBRE ,@PRO_FECHABLOQUEADO, @PRO_FECHAINICIO, @PRO_LOGO)";
                        
                    command = new SqlCommand(query, conn);
                    command.Parameters.Add(new SqlParameter("@PRO_FECHABLOQUEADO", fechaVacia));
                    command.Parameters.Add(new SqlParameter("@PRO_FECHAINICIO", fechaActual));
                    command.Parameters.Add(new SqlParameter("@PRO_NOMBRE", programa));
                    command.Parameters.Add(new SqlParameter("@PRO_ESTATUS", estadoPrograma));
                    command.Parameters.Add(new SqlParameter("@PRO_LOGO", imagen));
                    command.Parameters.Add(new SqlParameter("@RAD_ID", idRadio));

                    resultadoQuery = command.ExecuteNonQuery();
                    command.Dispose();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR" + e.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

            if (resultadoQuery == 1)
            {
                idRegistrado = ultimoIdRegistrado() - 1;

                if (idLocutorUno != 0)
                {
                    DAOLocutor.modificarProgramaPerteneciente(idLocutorUno, idRegistrado);
                }
                if (idLocutorUno != 0)
                {
                    DAOLocutor.modificarProgramaPerteneciente(idLocutorDos, idRegistrado);
                }
            }

            if (resultadoQuery == 1)
            {
                conn = null;
                try
                {
                    conn = ConexionBD.getConnection();
                    if (conn != null)
                    {
                        SqlCommand command;
                        String query = "INSERT INTO mus_horarios (HOR_NUMCANCIONES, HOR_DIASSEMANA, HOR_HORAFIN, HOR_HORAINICIO, HOR_ESTATUS, PAT_ID ,PRO_ID) " +
                                       "VALUES (@HOR_NUMCANCIONES, @HOR_DIASSEMANA, @HOR_HORAFIN, @HOR_HORAINICIO, @HOR_ESTATUS, @PAT_ID, @PRO_ID)";

                        for (int i = 0; i < 7; i++)
                        {
                            command = new SqlCommand(query, conn);

                            command.Parameters.Add(new SqlParameter("@HOR_NUMCANCIONES", numCanciones[i]));
                            command.Parameters.Add(new SqlParameter("@HOR_DIASSEMANA", numDia[i]));
                            command.Parameters.Add(new SqlParameter("@HOR_HORAFIN", horaFinal[i]));
                            command.Parameters.Add(new SqlParameter("@HOR_HORAINICIO", horaInicio[i]));
                            command.Parameters.Add(new SqlParameter("@HOR_ESTATUS", estadoDia[i]));

                            if (idPatron[i] != -1)
                            {
                                command.Parameters.Add(new SqlParameter("@PAT_ID", idPatron[i]));
                            }
                            else
                            {
                                command.Parameters.Add(new SqlParameter("@PAT_ID", SqlInt32.Null));
                            }
                            command.Parameters.Add(new SqlParameter("@PRO_ID", idRegistrado));

                            resultadoQuery = command.ExecuteNonQuery();
                            command.Dispose();
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR" + e.Message);
                }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }
            }
         
            List<DiasOperacion> diasOperacionRegistrados = new List<DiasOperacion>();
            diasOperacionRegistrados = DAODiasOperacion.obtenerDiasOperacionPrograma(idRegistrado);

            if (resultadoQuery == 1)
            {
                conn = null;
                try
                {
                    conn = ConexionBD.getConnection();
                    if (conn != null)
                    {
                        SqlCommand command;
                        String query = "INSERT INTO mus_programapatron (HOR_ID, PAT_ID) VALUES (@HOR_ID, @PAT_ID)";

                        foreach (DiasOperacion dia in diasOperacionRegistrados)
                        {
                            int posicionPatron = dia.NumeroDia - 1;

                            if (idPatron[posicionPatron] > -1) 
                            {
                                command = new SqlCommand(query, conn);
                                command.Parameters.Add(new SqlParameter("@HOR_ID", dia.IdDia));
                                command.Parameters.Add(new SqlParameter("@PAT_ID", idPatron[posicionPatron]));
                                //command.Parameters.Add(new SqlParameter("PROPAT_CANCIONACTUAL", ));

                                resultadoQuery = command.ExecuteNonQuery();
                                command.Dispose();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR" + e.Message);
                }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }
            }

            if (resultadoQuery == 1)
            {
                conn = null;
                try
                {
                    conn = ConexionBD.getConnection();
                    if (conn != null)
                    {
                        SqlCommand command;
                        String query = "INSERT INTO mus_musicadiapatron (CAN_ID, PROPAT_ID) VALUES (@CAN_ID, @PROPAT_ID)";

                        foreach (DiasOperacion dia in diasOperacionRegistrados)
                        {
                            if (dia.IdPatron != -1)
                            {
                                ProgramaPatron programaPatron = new ProgramaPatron();
                                programaPatron = DAOProgramaPatron.getProgramaPatron(dia.IdDia);

                                int posicionListaMusica = dia.NumeroDia - 1;

                                if (listaCaciones[posicionListaMusica] != null) { 
                                    foreach (Cancion cancion in listaCaciones[dia.NumeroDia-1])
                                    {
                                        command = new SqlCommand(query, conn);
                                        command.Parameters.Add(new SqlParameter("@PROPAT_ID", programaPatron.IdprogramaPatron));
                                        command.Parameters.Add(new SqlParameter("@CAN_ID", cancion.CancionID));

                                        resultadoQuery = command.ExecuteNonQuery();
                                        command.Dispose();
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR" + e.Message);
                }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }
            }
            return resultadoQuery; 
        }

        public static int ultimoIdRegistrado()
        {
            SqlConnection conn = null;
            SqlDataReader numero = null;
            int valor = 0;

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    String query = "SELECT MAX(PRO_ID) FROM mus_programas; ";
                    command = new SqlCommand(query, conn);
                    numero = command.ExecuteReader();
                    if (numero.Read())
                    {
                        valor = (!numero.IsDBNull(0)) ? numero.GetInt32(0) : 0;
                    }
                    numero.Close();
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
            return valor + 1;
        }

        public static int eliminarPrograma(int idPrograma)
        {
            SqlConnection conn = null;
            int respuestaConsulta = 0;
            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;

                    String query = String.Format("DELETE FROM mus_programas WHERE PRO_ID = '{0}';", idPrograma);

                    command = new SqlCommand(query, conn);
                    respuestaConsulta = command.ExecuteNonQuery();
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
            return respuestaConsulta;
        }

        internal static int updateReporte(int idProgramaRadio, String programaText, int estado, byte[] imagenGuardar, int idRadioObtenido, int idLocutorUno, int idLocutorDos, bool[] diaActivo, 
                                          int[] idDia, int[] numCaciones, int[] numeroDia, TimeSpan[] horaInicio, TimeSpan[] horaFinal, bool[] estadoDia, int[] idPatron, int[] claveProgramaDia,
                                           List<Cancion>[] listaCaciones)
        {
            String fechaBloqueo;
            
            if (estado == 0) 
            {
                fechaBloqueo = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                fechaBloqueo = "2000-01-01";
            }

            int resultadoQuery = 0;

            SqlConnection conn = null;

            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    String query = "UPDATE mus_programas SET PRO_FECHABLOQUEADO = @PRO_FECHABLOQUEADO, PRO_NOMBRE = @PRO_NOMBRE, " +
                                   "PRO_ESTATUS = @PRO_ESTATUS, PRO_LOGO = @PRO_LOGO, RAD_ID = @RAD_ID WHERE PRO_ID = @PRO_ID;";
                    command = new SqlCommand(query, conn);

                    command.Parameters.Add(new SqlParameter("@PRO_FECHABLOQUEADO", fechaBloqueo));
                    command.Parameters.Add(new SqlParameter("@PRO_NOMBRE", programaText));
                    command.Parameters.Add(new SqlParameter("@PRO_ESTATUS", estado));
                    command.Parameters.Add(new SqlParameter("@PRO_LOGO", imagenGuardar));
                    command.Parameters.Add(new SqlParameter("@RAD_ID", idRadioObtenido));
                    command.Parameters.Add(new SqlParameter("@PRO_ID", idProgramaRadio));

                    resultadoQuery = command.ExecuteNonQuery();
                    command.Dispose();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR" + e.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            
            if (idLocutorUno != 0)
            {
                DAOLocutor.modificarProgramaPerteneciente(idLocutorUno, idProgramaRadio);
            }
            if (idLocutorUno != 0)
            {
                DAOLocutor.modificarProgramaPerteneciente(idLocutorDos, idProgramaRadio);
            }
                      
            conn = null;
            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    String query = "UPDATE mus_horarios SET HOR_NUMCANCIONES = @HOR_NUMCANCIONES, HOR_HORAFIN = @HOR_HORAFIN, HOR_HORAINICIO = @HOR_HORAINICIO, HOR_ESTATUS = @HOR_ESTATUS, PAT_ID = @PAT_ID WHERE HOR_ID = @HOR_ID;";

                    for (int i = 0; i < 7; i++)
                    {
                        command = new SqlCommand(query, conn);

                        command.Parameters.Add(new SqlParameter("@HOR_NUMCANCIONES", numCaciones[i]));
                        command.Parameters.Add(new SqlParameter("@HOR_HORAFIN", horaFinal[i]));
                        command.Parameters.Add(new SqlParameter("@HOR_HORAINICIO", horaInicio[i]));
                        command.Parameters.Add(new SqlParameter("@HOR_ESTATUS", estadoDia[i]));
                            
                        if(idPatron[i] != -1)
                        {
                            command.Parameters.Add(new SqlParameter("@PAT_ID", idPatron[i]));
                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@PAT_ID", SqlInt32.Null));
                        }
                        command.Parameters.Add(new SqlParameter("@HOR_ID", idDia[i]));

                        resultadoQuery = command.ExecuteNonQuery();
                        command.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR" + e.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

            List<DiasOperacion> diasOperacionRegistrados = new List<DiasOperacion>();
            diasOperacionRegistrados = DAODiasOperacion.obtenerDiasOperacionPrograma(idProgramaRadio);


            conn = null;
            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    String query = "INSERT INTO mus_programapatron (HOR_ID, PAT_ID) VALUES (@HOR_ID, @PAT_ID)";

                    foreach (DiasOperacion dia in diasOperacionRegistrados)
                    {
                        int eliminacionDia = DAOProgramaPatron.deleteProgramaPatron(dia.IdDia);

                        if (eliminacionDia > 0)
                        {
                            int posicionPatron = dia.NumeroDia - 1;

                            if (idPatron[posicionPatron] > -1)
                            {
                                command = new SqlCommand(query, conn);
                                command.Parameters.Add(new SqlParameter("@HOR_ID", dia.IdDia));
                                command.Parameters.Add(new SqlParameter("@PAT_ID", idPatron[posicionPatron]));
                                //command.Parameters.Add(new SqlParameter("PROPAT_CANCIONACTUAL", ));

                                resultadoQuery = command.ExecuteNonQuery();
                                command.Dispose();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR" + e.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

            diasOperacionRegistrados = DAODiasOperacion.obtenerDiasOperacionPrograma(idProgramaRadio);

            if (resultadoQuery == 1)
            {
                conn = null;
                try
                {
                    conn = ConexionBD.getConnection();
                    if (conn != null)
                    {
                        SqlCommand command;
                        String query = "INSERT INTO mus_musicadiapatron (CAN_ID, PROPAT_ID) VALUES (@CAN_ID, @PROPAT_ID)";

                        foreach (DiasOperacion dia in diasOperacionRegistrados)
                        {
                            if (dia.IdPatron != -1)
                            {
                                ProgramaPatron programaPatron = new ProgramaPatron();
                                programaPatron = DAOProgramaPatron.getProgramaPatron(dia.IdDia);

                                int posicionListaMusica = dia.NumeroDia - 1;

                                if (listaCaciones[posicionListaMusica] != null)
                                {
                                    foreach (Cancion cancion in listaCaciones[posicionListaMusica])
                                    {
                                        command = new SqlCommand(query, conn);
                                        command.Parameters.Add(new SqlParameter("@PROPAT_ID", programaPatron.IdprogramaPatron));
                                        command.Parameters.Add(new SqlParameter("@CAN_ID", cancion.CancionID));

                                        resultadoQuery = command.ExecuteNonQuery();
                                        command.Dispose();
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR" + e.Message);
                }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }
            }

            return resultadoQuery;
        }
    }
}
