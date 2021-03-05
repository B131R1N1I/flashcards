using System.Net.Http;

namespace flashcards_server.Controllers
{
    public class PublicUserResponseMessage : HttpResponseMessage
    {
        public uint? id { get; set; } = null;
        public string username { get; set; } = "";
    }
}