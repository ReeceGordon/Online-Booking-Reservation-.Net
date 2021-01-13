using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace Lotus.Server
{
    public class MySqlSetup
    {
        private static string server;
        private static string database;
        private static string user;
        private static string password;
        private static string port;
        private static string connectionString;
        private static string sslM;

        private static MySqlConnection connection;
        //function to be used to make the connection split as repeated alot also makes it easier to change servers
        public static MySqlConnection Connect()
        {
            server = "blacklotusdns.duckdns.org";
            database = "blacklotusdb";
            user = "BlackLotus";
            password = "BlackLotus2020";
            port = "3306";
            sslM = "none";

            connectionString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}; SslMode={5}", server, port, user, password, database, sslM);
            connection = new MySqlConnection(connectionString);
            return connection;
        }

    }
}
