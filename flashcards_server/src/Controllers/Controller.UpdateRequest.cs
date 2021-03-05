using System.Drawing;

namespace flashcards_server.Controllers
{
    public class UpdateRequest
    {
        public uint id { get; set; }
        public string what { get; set; }
        public string to { get; set; }
        public Bitmap image { get; set; }
    }
}