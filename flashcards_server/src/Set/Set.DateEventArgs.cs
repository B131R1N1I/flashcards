using System;


namespace flashcards_server.Set
{
    public sealed partial class Set
    {
        public class DateEventArgs : EventArgs
        {
            public DateTime date { get; set; }
        }
    }
}
