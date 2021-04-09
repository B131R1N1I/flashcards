using System;
using System.Drawing;

namespace flashcards_server.Card
{
    public partial class Card
    {
        public int id { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public byte[] picture { get; set; }
        public int inSet { get; set; }
        public int ownerId { get; set; }
        public bool isPublic { get; set; }

        public virtual Set.Set inSetNavigation { get; set; }

        public Card(string answer, string question, byte[] picture, int inSet, int ownerId, bool isPublic)
        {
            this.answer = answer;
            this.question = question;
            this.picture = picture;
            this.inSet = inSet;
            this.ownerId = ownerId;
            this.isPublic = isPublic;

        }
    }
}