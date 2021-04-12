using System.Text.Json.Serialization;

namespace flashcards_server.Controllers
{
    public class UserToRegister
    {
        public string username { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        
        [JsonConstructor]
        public UserToRegister(string username, string name, string surname, string email, string password)
        {
            this.username = username;
            this.name = name;
            this.surname = surname;
            this.email = email;
            this.password = password;
        }
    }
}