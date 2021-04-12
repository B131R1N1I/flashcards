using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace flashcards_server.Set
{
    public sealed partial class Set
    {
        public Set()
        {
            cards = new HashSet<Card.Card>();
        }

        public int id { get; set; }
        public string name { get; set; }
        public int creatorId { get; set; }
        public int ownerId { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime lastModification { get; set; }
        public bool isPublic { get; set; }

        [JsonIgnore]
        public User.User creator { get; set; }
        [JsonIgnore]
        public ICollection<Card.Card> cards { get; set; }

        public Set(string name, int creator, int owner, DateTime createdDate, DateTime lastModification, bool isPublic, int id )
        {
            this.id = id;
            this.name = name;
            this.creatorId = creator;
            this.ownerId = owner;
            this.createdDate = createdDate;
            this.lastModification = lastModification;
            this.isPublic = isPublic;
        }

        public Set(string name, int creator, int owner, bool isPublic) :
            this(name, creator, owner, DateTime.Now, DateTime.Now, isPublic, 0) {}

        public override string ToString()
        {
            return $"Set [{id}: ({name}, {creatorId}, {ownerId}, {createdDate}, {lastModification}, {isPublic})]";
        }

        public override bool Equals(object obj)
        {
            return (obj is Set && (((Set)obj).name == this.name && ((Set)obj).id == ((Set)this).id));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
