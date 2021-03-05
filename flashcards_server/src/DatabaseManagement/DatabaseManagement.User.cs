using System;
using Npgsql;
using System.Net.Mail;
using System.Linq;


namespace flashcards_server.DatabaseManagement
{
    public partial class DatabaseManagement
    {
        public void AddUserToDatabase(User.User user)
        {
            try
            {
                if (!IsUserUsernameUnique(user.username))
                    throw new NpgsqlException($"username {user.username} is already used");
                if (!IsUserEmailUnique(user.email))
                    throw new NpgsqlException($"email {user.email} is already used");
                var passwordValidation = ValidatePassword(user.password);
                if (!passwordValidation.IsCorrect())
                    throw new NotValidPasswordException("Password is not correct", passwordValidation);
                using (var cmd = new NpgsqlCommand($"INSERT INTO users (username, email, name, surname, password)" +
                                                   $" VALUES ('{user.username}', '{user.email}', '{user.name}', " +
                                                   $"'{user.surname}', md5('{user.password}'));", Conn))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (PostgresException e)
                    {
                        Console.WriteLine("-> Cannot add user: " + user.username);
                        Console.WriteLine("-> " + e.MessageText);
                    }
                }
            }
            catch (FormatException)
            {
                throw;
            }
        }

        protected void UpdateUserEmail(User.User user, string newEmail)
        {
            using (var cmd = new NpgsqlCommand($"UPDATE users SET email = '{newEmail}' WHERE id={user.id}", Conn))
                cmd.ExecuteNonQuery();
        }

        public void UpdateUserEmail(object source, User.User.EmailEventArgs args)
        {
            UpdateUserEmail((User.User)source, args.email);
        }

        protected void UpdateUserName(User.User user, string newName)
        {
            using (var cmd = new NpgsqlCommand($"UPDATE users SET name = '{newName}' WHERE id={user.id}", Conn))
                cmd.ExecuteNonQuery();
        }

        public void UpdateUserName(object source, User.User.NameEventArgs args)
        {
            UpdateUserName((User.User)source, args.name);
        }

        protected void UpdateUserSurname(User.User user, string newSurname)
        {
            using (var cmd = new NpgsqlCommand($"UPDATE users SET surname = '{newSurname}' WHERE id={user.id}", Conn))
                cmd.ExecuteNonQuery();
        }

        public void UpdateUserSurname(object source, User.User.SurnameEventArgs args)
        {
            UpdateUserSurname((User.User)source, args.surname);
        }

        protected void UpdateUserPassword(User.User user, string newPassword)
        {
            using (var cmd = new NpgsqlCommand($"UPDATE users SET password = md5('{newPassword}') WHERE id={user.id}", Conn))
                cmd.ExecuteNonQuery();
        }

        public void UpdateUserPassword(object source, User.User.PasswordEventArgs args)
        {
            UpdateUserPassword((User.User)source, args.password);
        }

        public bool IsUserUsernameUnique(string username)
        {
            using (var cmd = new NpgsqlCommand($"SELECT COUNT(*) FROM users WHERE LOWER(users.username) = LOWER('{username}')", Conn))
                return (long)cmd.ExecuteScalar() == 0;
        }

        public bool IsUserEmailUnique(string email)
        {
            if (!IsValidEmail(email))
                throw new FormatException($"ERROR: '{email}' isn't a correct mail address");
            using (var cmd = new NpgsqlCommand($"SELECT COUNT(*) FROM users WHERE LOWER(users.email) = LOWER('{email}')", Conn))
                return (long)cmd.ExecuteScalar() == 0;
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                var m = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public PasswordValidation ValidatePassword(string password)
        {
            var passwordValidation = new PasswordValidation();
            passwordValidation.lengthMin8 = IsPasswordValidLengthMin8(password);
            passwordValidation.lengthMax32 = IsPasswordValidLengthMax32(password);
            passwordValidation.lowerCaseLetter = PasswordContainsLowerCaseLetter(password);
            passwordValidation.upperCaseLetter = PasswordContainsUpperCaseLetter(password);
            passwordValidation.number = PasswordContainsADigit(password);
            return passwordValidation;
        }

        private bool IsPasswordValidLengthMin8(string password)
        {
            return password.Length >= 8;
        }

        private bool IsPasswordValidLengthMax32(string password)
        {
            return password.Length <= 32;
        }

        private bool PasswordContainsLowerCaseLetter(string password)
        {
            return password.Any(char.IsLower);
        }

        private bool PasswordContainsUpperCaseLetter(string password)
        {
            return password.Any(char.IsUpper);
        }

        private bool PasswordContainsADigit(string passowrd)
        {
            return passowrd.Any(char.IsDigit);
        }

        public User.User GetUserById(uint id)
        {
            using (var cmd = new NpgsqlCommand($"SELECT * FROM users WHERE id = {id} LIMIT 1;", Conn))
                return _GetUserByCmd(cmd);
        }

        public User.User GetUserByUsername(string username)
        {
            using (var cmd = new NpgsqlCommand($"SELECT * FROM users WHERE username = '{username}' LIMIT 1;", Conn))
                return _GetUserByCmd(cmd);
        }

        public User.User GetUserByEmail(string email)
        {
            using (var cmd = new NpgsqlCommand($"SELECT * FROM users WHERE email = '{email}' LIMIT 1;", Conn))
                return _GetUserByCmd(cmd);
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
            using (var cmd = new NpgsqlCommand($"SELECT md5('{password}') = password FROM users WHERE " +
                                               $"id = {user.id};", Conn))
            {
                using (var output = cmd.ExecuteReader())
                {
                    if (!output.HasRows)
                        throw new NpgsqlException("No user found to match password");
                    output.Read();

                    return (bool)output[0];
                }
            }
        }

    }
}