using System;
using System.Collections.Generic;

#nullable disable

namespace flashcards_server
{
    public partial class CardStatus
    {
        public int cardId { get; set; }
        public int userId { get; set; }
        public DateTime lastReview { get; set; }
        public DateTime nextReview { get; set; }
        public bool active { get; set; }
        public bool difficult { get; set; }

        public virtual Card.Card card { get; set; }
        public virtual User.User user { get; set; }
    }
}
