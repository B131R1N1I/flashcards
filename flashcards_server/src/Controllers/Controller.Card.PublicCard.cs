namespace flashcards_server.Controllers
{
    public class PublicCard
    {
        public uint? id { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public string image { get; set; }
        public uint inSet { get; set; }

        public PublicCard(uint? id, string question, string answer, string image, uint inSet)
        {
            this.id = id;
            this.question = question;
            this.answer = answer;
            this.image = image;
            this.inSet = inSet;
        }
    }
}