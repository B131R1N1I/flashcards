using System;

namespace flashcards_server.Controllers
{
    public class PublicUser
    {
        public long id { get; set; }
        public string username { get; set; }

        public PublicUser(long id, string username)
        {
            this.id = id;
            this.username = username;
        }
        
        public PublicUser(User.User u) : this(u.Id, u.UserName) {}
    }
}