using System;


namespace flashcards_server.User
{
    public partial class User
    {
        public class NameEventArgs : EventArgs
        {
            public string name { get; set; }
        }
    }
}
