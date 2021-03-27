using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace flashcards_server.User
{
    public partial class User
    {
        public User()
        {
            sets = new HashSet<Set.Set>();
        }

        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string password { get; set; }
        public bool active { get; set; }

        public virtual ICollection<Set.Set> sets { get; set; }
        
        
        [JsonConstructor]
        public User(string username, string email, string name, string surname, string password, int id)
        {
            this.id = id;
            this.username = username;
            this.email = email;
            this.name = name;
            this.surname = surname;
            this.password = password;
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
