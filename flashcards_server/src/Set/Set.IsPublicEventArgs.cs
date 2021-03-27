using System;


namespace flashcards_server.Set
{
    public sealed partial class Set
    {
        public class IsPublicEventArgs : EventArgs
        {
            public bool isPublic { get; set; }
        }
    }
}
