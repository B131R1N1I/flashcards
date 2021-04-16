using System.Drawing;
using Microsoft.AspNetCore.Http;

namespace flashcards_server.Controllers
{
    public class UpdateRequest
    {
        public uint id { get; set; }
        public string what { get; set; }
        public string to { get; set; }
        public IFormFile image { get; set; }
    }
}