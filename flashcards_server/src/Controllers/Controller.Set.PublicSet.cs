using System;

namespace flashcards_server.API.Controllers
{
    public class TempMinSet
    {
        public string name { get; set; }
        public uint creator { get; set; }
        public uint owner { get; set; }
        public bool isPublic { get; set; }
    }
}