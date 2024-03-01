using Microsoft.Data.SqlClient;

namespace Polizia.Models
{
    public class DB
    {
        public static string connectionString = "server=DESKTOP-SRMJK0U\\SQLEXPRESS; Initial Catalog=PoliziaMunicipale; Integrated Security=true; TrustServerCertificate=true";
        public static SqlConnection conn = new SqlConnection(connectionString);
    }
}
