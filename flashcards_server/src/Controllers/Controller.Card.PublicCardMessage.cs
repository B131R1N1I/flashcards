using System.Net.Http;

namespace flashcards_server.Controllers
{
    public class PublicCardMessage : HttpResponseMessage
    {
        public PublicCard card { get; set; }
    }
}