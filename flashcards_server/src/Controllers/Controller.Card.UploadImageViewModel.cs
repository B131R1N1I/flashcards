using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace flashcards_server.Controllers
{
    public class UploadImageViewModel
    {
        [Display(Name = "File")]
        public IFormFile file { get; set; }
        public int cardId { get; set; }
    }
}