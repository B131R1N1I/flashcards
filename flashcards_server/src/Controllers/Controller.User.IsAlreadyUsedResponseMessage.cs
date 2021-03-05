using System.Net.Http;

namespace flashcards_server.Controllers
{
    public class IsAlreadyUsedResponseMessage : HttpResponseMessage
    {
        public bool isAlreadyUsed { get; set; }
    }
}