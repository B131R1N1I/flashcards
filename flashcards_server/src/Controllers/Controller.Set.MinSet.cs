namespace flashcards_server.Controllers
{
    public class MinSet
    {
        public string name { get; set; }
        public uint creator { get; set; }
        public uint owner { get; set; }
        public bool isPublic { get; set; }
    }
}