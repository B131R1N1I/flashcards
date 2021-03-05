using System.Net.Http;

namespace flashcards_server.Controllers
{
    public class IsAleradyUsedResponseMessage : HttpResponseMessage
    {
        public bool isAlreadyUsed { get; set; }
    }
}