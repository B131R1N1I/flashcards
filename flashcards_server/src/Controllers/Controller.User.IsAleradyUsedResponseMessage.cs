using System;
using System.Net.Http;

namespace flashcards_server.API.Controllers
{
    public class IsAleradyUsedResponseMessage : HttpResponseMessage
    {
        public bool isAlreadyUsed { get; set; }
    }
}