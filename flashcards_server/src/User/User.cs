using System;
using System.Text.Json.Serialization;


namespace flashcards_server.User
{
    public partial class User
    {
        public static event EventHandler UserCreatedEventHandler;
        public event EventHandler<UsernameEventArgs> UsernameChangedEventHandler;
        public event EventHandler<EmailEventArgs> EmailChangedEventHandler;
        public event EventHandler<NameEventArgs> NameChangedEventHandler;
        public event EventHandler<SurnameEventArgs> SurnameChangedEventHandler;
        public event EventHandler<PasswordEventArgs> PasswordChangedEventHandler;

        protected virtual void OnUserCreated(User user)
        {
            if (UserCreatedEventHandler != null)
                UserCreatedEventHandler(this, new EventArgs());
        }

        protected virtual void OnUsernameChanged(string username)
        {
            if (UsernameChangedEventHandler != null)
                UsernameChangedEventHandler(this, new UsernameEventArgs { username = username });
        }

        protected virtual void OnPasswordChanged(string password)
        {
            if (PasswordChangedEventHandler != null)
                PasswordChangedEventHandler(this, new PasswordEventArgs { password = password });
        }

        protected virtual void OnEmailChanged(string email)
        {
            if (EmailChangedEventHandler != null)
                EmailChangedEventHandler(this, new EmailEventArgs { email = email });
        }

        protected void OnNameChanged(string name)
        {
            if (NameChangedEventHandler != null)
                NameChangedEventHandler(this, new NameEventArgs { name = name });
        }

        protected void OnSurnameChanged(string surname)
        {
            if (SurnameChangedEventHandler != null)
                SurnameChangedEventHandler(this, new SurnameEventArgs { surname = surname });
        }

        // private uint _id;
        public uint? id { get; }

        private string _username;
        public string username
        {
            get => _username;
            set
            {
                try
                {
                    OnUsernameChanged(value);
                    _username = value;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Couldn't change username to: " + value);
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }
        private string _email;

        public string email
        {
            get => _email;
            set
            {
                try
                {
                    OnEmailChanged(value);
                    _email = value;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Cannot set email to: " + value);
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }

        private string _name;

        public string name
        {
            get => _name;
            set
            {
                try
                {
                    OnNameChanged(value);
                    _name = value;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Cannot set name to: " + value);
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }

        private string _surname;

        public string surname
        {
            get => _surname;
            set
            {
                try
                {
                    OnSurnameChanged(value);
                    _surname = value;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Cannot set name to: " + surname);
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }

        private string _password; // idk yet if it should be a string

        public string password
        {
            get => _password;
            set
            {
                try
                {
                    OnPasswordChanged(value);
                    _password = value;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Cannot set passowrd ");
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }
        [JsonConstructor]
        public User(string username, string email, string name, string surname, string password, uint? id = null)
        {
            this.id = id;
            this._username = username;
            this._email = email;
            this._name = name;
            this._surname = surname;
            this._password = password;
            OnUserCreated(this);
        }

        public User(string username, string email, string name, string surname, string password, int id) : this(username, email, name, surname, password, (uint?)id) { }

        public void RegisterUser(DatabaseManagement.DatabaseManagement database)
        {
            if (database.conn.Database == String.Empty)
                throw new Npgsql.NpgsqlException("CONNECTION IS NOT OPEN");
            try
            {
                database.AddUserToDatabase(this);
            }
            catch (ArgumentException)
            {
                throw;
            }
        }

        static public User GetUser(/* args */)
        {
            throw new NotImplementedException("Connection with database has not been created yet");
        }

        public override string ToString()
        {
            return $"User [{id}: ({username}, {email}, {name}, {surname})]";
        }

        public override bool Equals(object obj)
        {
            return (obj is User && ((User)obj).name == this.name && ((User)obj).email == this.email);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
