using MySqlConnector;
namespace Common.Shared
{
    public class ConnectionStrings
    {
        public string Cnn { get; set; }
        public MySqlConnection Database => new MySqlConnection(Cnn);

    }
}
