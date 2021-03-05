using System.Net;
using System.Net.Http;

namespace flashcards_server.Controllers
{
    public class SuccessMessageResponseMessage : HttpResponseMessage
    {
        public bool successed { get; set; }
        public string reason { get; set; }

        public SuccessMessageResponseMessage(bool successed, string reason = "", HttpStatusCode code = HttpStatusCode.OK)
        {
            this.successed = successed;
            this.reason = reason;
            this.StatusCode = code;
        }
    }
}