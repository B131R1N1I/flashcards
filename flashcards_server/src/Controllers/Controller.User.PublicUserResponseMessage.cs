using System.Net.Http;

namespace flashcards_server.Controllers
{
    public class PublicUserResponseMessage : HttpResponseMessage
    {
        public PublicUser user { get; set; } 
    }
}