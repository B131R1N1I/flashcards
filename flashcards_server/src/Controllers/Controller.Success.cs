using System.Net;
using System.Net.Http;

namespace flashcards_server.Controllers
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