using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace flashcards_server.Controllers
{
    public class SuccessMessageResponseMessage : HttpResponseMessage, IActionResult
    {
        public bool successed { get; set; }
        public string reason { get; set; }

        public SuccessMessageResponseMessage(bool _successed, string _reason = "", HttpStatusCode _code = HttpStatusCode.OK)
        {
            successed = _successed;
            reason = _reason;
            this.StatusCode = _code;
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            var json = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(this));
            Stream stream = new MemoryStream(json);
            response.Body = stream;
            return Task.CompletedTask;
        }
    }
}