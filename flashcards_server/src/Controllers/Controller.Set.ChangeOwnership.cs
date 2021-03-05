using System;

namespace flashcards_server.API.Controllers
{
    public class ChangeOwnership
    {
        public uint setId { get; set; }
        public uint userId { get; set; }
    }
}