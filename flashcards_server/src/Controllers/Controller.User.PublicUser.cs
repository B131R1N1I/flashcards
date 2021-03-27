using System;

namespace flashcards_server.Controllers
{
    public class PublicUser
    {
        public int id { get; set; }
        public string username { get; set; }

        public PublicUser(int id, string username)
        {
            this.id = id;
            this.username = username;
        }
        
        public PublicUser(User.User u) : this(u.id, u.username) {}
    }
}