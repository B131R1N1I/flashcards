using System;


namespace flashcards_server.User
{
    public partial class User
    {
        public class EmailEventArgs : EventArgs
        {
            public string email { get; set; }
        }
    }
}
