using System;
using Npgsql;


namespace flashcards_server.DatabaseManagement
{
    public partial class DatabaseManagement
    {
        private NpgsqlConnection conn;

        public DatabaseManagement(string server, string user, string password /*temporary*/, string database)
        {
            conn = new NpgsqlConnection($"Server={server};User Id={user}; Password={password};Database={database}");
        }

        public void OpenConnection()
        {
            conn.Open();
        }

        public void CloseConnection()
        {
            conn.Close();
        }

    }
}
