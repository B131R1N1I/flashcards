using System;
using Npgsql;


namespace flashcards_server.DatabaseManagement
{
    public partial class DatabaseManagement
    {
        public event EventHandler<AddedUserEventArgs> UserAddedEventHandler;
        // it will be read from json file
        // private NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;User Id=flashcards_app; Password=fc_app;Database=flashcards");

        protected virtual void OnUserAdded(User.User user)
        {
            if (UserAddedEventHandler != null)
                UserAddedEventHandler(this, new AddedUserEventArgs { user = user });
        }


        public void AddUserToDatabase(User.User user)
        {

            using (var cmd = new NpgsqlCommand($"INSERT INTO users (username, email, name, surname, password) VALUES ('{user.username}', '{user.email}', '{user.name}', '{user.surname}', '{user.password}')", conn))
            {
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (PostgresException e)
                {
                    // some code
                    // it won't be printing out the error message,
                    // it will handle it
                    System.Console.WriteLine("-> Cannot add user: " + user.username);
                    System.Console.WriteLine("-> " + e.MessageText);
                }
            }
        }

        public void UpdateUserEmail(User.User user, string newEmail)
        {
            using (var cmd = new NpgsqlCommand($"UPDATE users SET email = '{newEmail}' WHERE id={user.id}", conn))
            {
                // some code (try..catch) etc.
                cmd.ExecuteNonQuery();
                System.Console.WriteLine("updated email");
            }
        }

        public void UpdateUserName(User.User user, string newName)
        {
            using (var cmd = new NpgsqlCommand($"UPDATE users SET name = '{newName}' WHERE id={user.id}", conn))
            {
                // some code (try..catch) etc.
                cmd.ExecuteNonQuery();
                System.Console.WriteLine("updated name");
            }
        }

        public void UpdateUserSurname(User.User user, string newSurname)
        {
            using (var cmd = new NpgsqlCommand($"UPDATE users SET surname = '{newSurname}' WHERE id={user.id}", conn))
            {
                // some code (try..catch) etc.
                cmd.ExecuteNonQuery();
                System.Console.WriteLine("updated surname");
            }
        }

        public bool IsUsernameUnique(string username)
        {
            using (var cmd = new NpgsqlCommand($"SELECT COUNT(*) FROM users WHERE users.username = '{username}'", conn))
            {
                var output = (Int64)cmd.ExecuteScalar();
                return output == 0;
            }
        }

        public bool IsEmailUnique(string email)
        {
            using (var cmd = new NpgsqlCommand($"SELECT COUNT(*) FROM users WHERE users.email = '{email}'", conn))
            {
                var output = (Int64)cmd.ExecuteScalar();
                return output == 0;
            }
        }

        public User.User GetUser(int id)
        {
            using (var cmd = new NpgsqlCommand($"SELECT * FROM users WHERE id = {id} LIMIT 1;", conn))
            {
                User.User user;
                using (var output = cmd.ExecuteReader())
                {
                    output.Read();
                    user = new User.User(output[1].ToString(), output[2].ToString(),
                                             output[3].ToString(), output[4].ToString(),
                                             output[5].ToString(), (Int32)output[0]);
                }
                return user;
            }
        }

        public User.User GetUser(string username)
        {
            using (var cmd = new NpgsqlCommand($"SELECT * FROM users WHERE username = '{username}' LIMIT 1;", conn))
            {
                User.User user;
                using (var output = cmd.ExecuteReader())
                {
                    output.Read();
                    user = new User.User(output[1].ToString(), output[2].ToString(),
                                             output[3].ToString(), output[4].ToString(),
                                             output[5].ToString(), (Int32)output[0]);
                }
                return user;
            }
        }
    }
}