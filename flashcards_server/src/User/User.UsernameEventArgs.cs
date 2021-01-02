using System;


namespace flashcards_server.User
{
    public partial class User
    {
        public class UsernameEventArgs : EventArgs
        {
            public string username { get; set; }
        }

    }
}