using System;
using System.Drawing;
using System.Net.Http;

namespace flashcards_server.API.Controllers
{
    public class PublicCardMessage : HttpResponseMessage
    {
        public PublicCard card { get; set; }
    }
}