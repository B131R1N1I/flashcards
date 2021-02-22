using System;
using System.Net.Http;

namespace flashcards_server.API.Controllers
{
    public class SuccessMessageResponseMessage : HttpResponseMessage
    {
        public bool successed { get; set; }
        public string reason { get; set; }
    }
}