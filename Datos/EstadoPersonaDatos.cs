using System.Data;
using Entidades;
using Microsoft.Data.SqlClient;

namespace Datos
{
    public class EstadoPersonaDatos
    {
        public List<EstadoPersona> Listar()
        {
            List<EstadoPersona> estados = new List<EstadoPersona>();

            using (SqlConnection conexion = Conexion.ObtenerConexion())
            using (SqlCommand comando = new SqlCommand("usp_EstadoPersona_Listar", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                conexion.Open();

                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        estados.Add(new EstadoPersona
                        {
                            Estado = reader.GetBoolean(reader.GetOrdinal("estado")),
                            Descripcion = reader.GetString(reader.GetOrdinal("descripcion"))
                        });
                    }
                }
            }

            return estados;
        }
    }
}
