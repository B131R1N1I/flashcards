using System;
using System.Text.Json.Serialization;
using System.Drawing;

namespace flashcards_server.API.Controllers
{
    public class UpdateRequest
    {
        public uint id { get; set; }
        public string what { get; set; }
        public string to { get; set; }
        public Bitmap image { get; set; }

        // [JsonConstructor]
        // public UpdateRequest(uint id, string what, string to)
        // {
        //     this.id = id;
        //     this.what = what;
        //     this.to = to;
        // }
    }
}