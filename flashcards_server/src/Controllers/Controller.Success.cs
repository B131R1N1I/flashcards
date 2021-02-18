using System;
using System.Net.Http;

namespace flashcards_server.API.Controllers
{
    public class SuccessMessage : HttpResponseMessage
    {
        public bool successed { get; set; }
    }
}