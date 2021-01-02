using System;


namespace flashcards_server.User
{
    public partial class User
    {
        public event EventHandler<UsernameEventArgs> UsernameChangedEventHandler;
        public event EventHandler<EmailEventArgs> EmailChangedEventHandler;
        public event EventHandler<NameEventArgs> NameChangedEventHandler;
        public event EventHandler<SurnameEventArgs> SurnameChangedEventHandler;

        protected virtual void OnUsernameChanged(string username)
        {
            if (UsernameChangedEventHandler != null)
                UsernameChangedEventHandler(this, new UsernameEventArgs { username = username });
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
        public uint id { get; }

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

        readonly protected string password; // idk yet if it should be a string

        public User(string username, string email, string name, string surname, string password)
        {
            // this.ID = id;
            this.username = username;
            this.email = email;
            this.name = name;
            this.surname = surname;
            this.password = password;
        }

        public void RegisterUser(User user)
        {
            throw new NotImplementedException("Cannot register users yet");
        }

        /// <sumary>
        /// GetUser methods serches for user in database and returns if any matches
        /// </sumary> 

        static public User GetUser(/* args */)
        {
            throw new NotImplementedException("Connection with database has not been created yet");
        }
    }
}
