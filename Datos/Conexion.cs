using Microsoft.Data.SqlClient;

namespace Datos
{
    public static class Conexion
    {
        private const string CadenaConexion =
            @"Server=localhost\SQLEXPRESS;Database=Lab07DB;Trusted_Connection=True;TrustServerCertificate=True;Connect Timeout=5;";

        public static SqlConnection ObtenerConexion()
        {
            return new SqlConnection(CadenaConexion);
        }
    }
}

