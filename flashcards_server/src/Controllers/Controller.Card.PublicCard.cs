using Microsoft.AspNetCore.Http;

namespace flashcards_server.Controllers
{
    public class PublicCard
    {
        public int id { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public bool hasImage { get; set; }
        public int inSet { get; set; }

        public PublicCard(int _id, string _question, string _answer, bool _image, int _inSet)
        {
            id = _id;
            question = _question;
            answer = _answer;
            hasImage = _image;
            inSet = _inSet;
        }
    }
}