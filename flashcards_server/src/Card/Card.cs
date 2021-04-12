using System;
using System.Drawing;
using System.Text.Json.Serialization;

namespace flashcards_server.Card
{
    public partial class Card
    {
        public int id { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        [JsonIgnore]
        public string picture { get; set; }
        public int inSet { get; set; }
        public int ownerId { get; set; }
        public bool isPublic { get; set; }

        public virtual Set.Set inSetNavigation { get; set; }

        public Card(string answer, string question, int inSet, int ownerId, bool isPublic)
        {
            this.answer = answer;
            this.question = question;
            // this.picture = picture;
            this.inSet = inSet;
            this.ownerId = ownerId;
            this.isPublic = isPublic;

        }
    }
}