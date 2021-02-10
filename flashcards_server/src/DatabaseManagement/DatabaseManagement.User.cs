using System;
using Npgsql;


namespace flashcards_server.DatabaseManagement
{
    public partial class DatabaseManagement
    {
        public void AddUserToDatabase(User.User user)
        {
            if (!IsUserUsernameUnique(user.username))
                throw new NpgsqlException($"username {user.username} is already used");
            if (!IsUserEmailUnique(user.email))
                throw new NpgsqlException($"email {user.email} is already used");
            using (var cmd = new NpgsqlCommand($"INSERT INTO users (username, email, name, surname, password) VALUES ('{user.username}', '{user.email}', '{user.name}', '{user.surname}', md5('{user.password}'));", conn))
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

        protected void UpdateUserEmail(User.User user, string newEmail)
        {
            using (var cmd = new NpgsqlCommand($"UPDATE users SET email = '{newEmail}' WHERE id={user.id}", conn))
            {
                // some code (try..catch) etc.
                cmd.ExecuteNonQuery();
                System.Console.WriteLine("updated email");
            }
        }

        public void UpdateUserEmail(Object source, User.User.EmailEventArgs args)
        {
            UpdateUserEmail((User.User)source, args.email);
        }

        protected void UpdateUserName(User.User user, string newName)
        {
            using (var cmd = new NpgsqlCommand($"UPDATE users SET name = '{newName}' WHERE id={user.id}", conn))
            {
                // some code (try..catch) etc.
                cmd.ExecuteNonQuery();
                System.Console.WriteLine("updated name");
            }
        }

        public void UpdateUserName(Object source, User.User.NameEventArgs args)
        {
            UpdateUserName((User.User)source, args.name);
        }

        protected void UpdateUserSurname(User.User user, string newSurname)
        {
            using (var cmd = new NpgsqlCommand($"UPDATE users SET surname = '{newSurname}' WHERE id={user.id}", conn))
            {
                // some code (try..catch) etc.
                cmd.ExecuteNonQuery();
                System.Console.WriteLine("updated surname");
            }
        }

        public void UpdateUserSurname(Object source, User.User.SurnameEventArgs args)
        {
            UpdateUserSurname((User.User)source, args.surname);
        }

        protected void UpdateUserPassword(User.User user, string newPassword)
        {
            using (var cmd = new NpgsqlCommand($"UPDATE users SET password = md5('{newPassword}') WHERE id={user.id}", conn))
            {
                // some code (try..catch) etc.
                cmd.ExecuteNonQuery();
                System.Console.WriteLine("Updated password");
            }
        }

        public void UpdateUserPassword(Object source, User.User.PasswordEventArgs args)
        {
            UpdateUserPassword((User.User)source, args.password);
        }

        public bool IsUserUsernameUnique(string username)
        {
            using (var cmd = new NpgsqlCommand($"SELECT COUNT(*) FROM users WHERE LOWER(users.username) = LOWER('{username}')", conn))
            {
                var output = (Int64)cmd.ExecuteScalar();
                return output == 0;
            }
        }

        public bool IsUserEmailUnique(string email)
        {
            using (var cmd = new NpgsqlCommand($"SELECT COUNT(*) FROM users WHERE LOWER(users.email) = LOWER('{email}')", conn))
            {
                var output = (Int64)cmd.ExecuteScalar();
                return output == 0;
            }
        }

        public User.User GetUserById(int id)
        {
            using (var cmd = new NpgsqlCommand($"SELECT * FROM users WHERE id = {id} LIMIT 1;", conn))
            {
                return _GetUserByCmd(cmd);
            }
        }

        public User.User GetUserByUsername(string username)
        {
            using (var cmd = new NpgsqlCommand($"SELECT * FROM users WHERE username = '{username}' LIMIT 1;", conn))
            {
                return _GetUserByCmd(cmd);
            }
        }

        public User.User GetUserByEmail(string email)
        {
            using (var cmd = new NpgsqlCommand($"SELECT * FROM users WHERE email = '{email}' LIMIT 1;", conn))
            {

                return _GetUserByCmd(cmd);
            }
        }

        public User.User _GetUserByCmd(NpgsqlCommand cmd)
        {
            using (var output = cmd.ExecuteReader())
            {
                if (!output.HasRows)
                    throw new NpgsqlException($"No User found");
                output.Read();
                return new User.User(output.GetString(1), output.GetString(2),
                                     output.GetString(3), output.GetString(4),
                                     output.GetString(5), output.GetInt32(0));
            }
        }

        public bool PasswordMatch(User.User user, string password)
        {
            using (var cmd = new NpgsqlCommand($"SELECT md5('{password}') = password FROM users WHERE id = {user.id};", conn))
            {
                using (var output = cmd.ExecuteReader())
                {
                    output.Read();
                    if (!output.HasRows)
                        throw new NpgsqlException("No user found to match password");
                    return (bool)output[0];
                }
            }
        }

    }
}