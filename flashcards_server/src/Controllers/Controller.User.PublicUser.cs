using System;

namespace flashcards_server.Controllers
{
    public class PublicUser
    {
        public uint? id { get; set; }
        public string username { get; set; }

        public PublicUser(uint? id, string username)
        {
            this.id = id;
            this.username = username;
        }
    }
}