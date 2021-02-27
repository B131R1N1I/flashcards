using System;
using System.Net.Http;
using System.Net;

namespace flashcards_server.API.Controllers
{
    public class SuccessMessageResponseMessage : HttpResponseMessage
    {
        public bool successed { get; set; }
        public string reason { get; set; }

        public SuccessMessageResponseMessage(bool _successed, string _reason = "", HttpStatusCode _code = HttpStatusCode.OK)
        {
            successed = _successed;
            reason = _reason;
            this.StatusCode = _code;
        }
    }
}