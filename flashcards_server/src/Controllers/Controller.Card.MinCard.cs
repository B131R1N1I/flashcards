using System.Drawing;

namespace flashcards_server.Controllers
{
    public class MinCard
    {
        public string question { get; set; }
        public string answer { get; set; }
        public Bitmap image { get; set; }
        public uint inSet { get; set; }
    }
}