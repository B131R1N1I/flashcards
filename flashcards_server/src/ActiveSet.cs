using System;
using System.Collections.Generic;

#nullable disable

namespace flashcards_server
{
    public partial class ActiveSet
    {
        public int userId { get; set; }
        public int setId { get; set; }

        public virtual Set.Set set { get; set; }
        public virtual User.User user { get; set; }
    }
}
