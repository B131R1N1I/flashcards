using System;
using System.Drawing;
using System.Net.Http;

namespace flashcards_server.API.Controllers
{
    public class PublicCard : HttpResponseMessage
    {
        public uint? id { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public string image { get; set; }
        public uint inSet { get; set; }

        public PublicCard(uint? _id, string _question, string _answer, string _image, uint _inSet)
        {
            id = _id;
            question = _question;
            answer = _answer;
            image = _image;
            inSet = _inSet;
        }
    }
}