using System;
using Npgsql;


namespace flashcards_server.DatabaseManagement
{
    public partial class DatabaseManagement
    {
        public readonly NpgsqlConnection conn;

        public DatabaseManagement(string server, string user, string password /*temporary*/, string database)
        {
            conn = new NpgsqlConnection($"Server={server};User Id={user}; Password={password};Database={database}");
            User.User.UserCreatedEventHandler += AddUserEvents;
        }

        public void OpenConnection()
        {
            conn.Open();
        }

        public void CloseConnection()
        {
            conn.Close();
        }

        void AddUserEvents(object obj, EventArgs e)
        {
            ((User.User)obj).NameChangedEventHandler += this.UpdateUserName;
            ((User.User)obj).EmailChangedEventHandler += this.UpdateUserEmail;
            ((User.User)obj).SurnameChangedEventHandler += this.UpdateUserSurname;
            ((User.User)obj).PasswordChangedEventHandler += this.UpdateUserPassword;
        }

    }
}
