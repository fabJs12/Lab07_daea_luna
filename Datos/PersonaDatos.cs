using System.Data;
using Entidades;
using Microsoft.Data.SqlClient;

namespace Datos
{
    public class PersonaDatos
    {
        public List<Persona> Listar()
        {
            return LeerPersonas("usp_Persona_Listar");
        }

        public List<Persona> ListarTodos()
        {
            return LeerPersonas("usp_Persona_ListarTodos");
        }

        private List<Persona> LeerPersonas(string procedimiento)
        {
            List<Persona> personas = new List<Persona>();

            using (SqlConnection conexion = Conexion.ObtenerConexion())
            using (SqlCommand comando = new SqlCommand(procedimiento, conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                conexion.Open();

                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Persona persona = new Persona
                        {
                            IdPersona = reader.GetInt32(reader.GetOrdinal("idPersona")),
                            Nombres = reader.GetString(reader.GetOrdinal("nombres")),
                            Apellidos = reader.GetString(reader.GetOrdinal("apellidos")),
                            Dni = reader.GetString(reader.GetOrdinal("dni")),
                            Estado = reader.GetBoolean(reader.GetOrdinal("estado"))
                        };
                        personas.Add(persona);
                    }
                }
            }

            return personas;
        }

        public bool Insertar(Persona persona)
        {
            using (SqlConnection conexion = Conexion.ObtenerConexion())
            using (SqlCommand comando = new SqlCommand("usp_Persona_Insertar", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                AgregarDatosPersona(comando, persona);
                conexion.Open();
                return comando.ExecuteNonQuery() != 0;
            }
        }

        public bool Actualizar(Persona persona)
        {
            using (SqlConnection conexion = Conexion.ObtenerConexion())
            using (SqlCommand comando = new SqlCommand("usp_Persona_Actualizar", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@idPersona", SqlDbType.Int).Value = persona.IdPersona;
                AgregarDatosPersona(comando, persona);
                conexion.Open();
                return comando.ExecuteNonQuery() != 0;
            }
        }

        public bool Eliminar(int idPersona)
        {
            using (SqlConnection conexion = Conexion.ObtenerConexion())
            using (SqlCommand comando = new SqlCommand("usp_Persona_Eliminar", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@idPersona", SqlDbType.Int).Value = idPersona;
                conexion.Open();
                return comando.ExecuteNonQuery() != 0;
            }
        }

        private static void AgregarDatosPersona(SqlCommand comando, Persona persona)
        {
            comando.Parameters.Add("@nombres", SqlDbType.VarChar, 80).Value = persona.Nombres;
            comando.Parameters.Add("@apellidos", SqlDbType.VarChar, 80).Value = persona.Apellidos;
            comando.Parameters.Add("@dni", SqlDbType.VarChar, 8).Value = persona.Dni;
        }
    }
}
