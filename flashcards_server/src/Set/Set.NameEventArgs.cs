using System;

namespace flashcards_server.Set
{
    public sealed partial class Set
    {
        public class NameEventArgs : EventArgs
        {
            public string name { get; set; }
        }
    }
}