using System;

namespace flashcards_server.Set
{
    public partial class Set
    {
        public class NameEventArgs : EventArgs
        {
            public string name { get; set; }
        }
    }
}