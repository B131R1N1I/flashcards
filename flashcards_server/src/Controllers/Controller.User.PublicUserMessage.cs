using System;
using System.Net.Http;

namespace flashcards_server.API.Controllers
{
    public class PublicUserResponseMessage : HttpResponseMessage
    {
        public PublicUser user { get; set; }
    }
}