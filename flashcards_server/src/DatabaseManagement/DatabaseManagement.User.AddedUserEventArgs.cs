using System;


namespace flashcards_server.DatabaseManagement
{
    public partial class DatabaseManagement
    {
        public class AddedUserEventArgs : EventArgs
        {
            public User.User user { get; set; }
        }
    }
}