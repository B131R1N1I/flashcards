using System.Drawing;

namespace flashcards_server.Controllers
{
    public class MinCard
    {
        public string question { get; set; }
        public string answer { get; set; }
        public byte[] image { get; set; }
        public int inSet { get; set; }
        public bool isPublic { get; set; }
    }
}