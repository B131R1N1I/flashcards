using System;


namespace flashcards_server.User
{
    public partial class User
    {
        public class SurnameEventArgs : EventArgs
        {
            public string surname { get; set; }
        }
    }
}

