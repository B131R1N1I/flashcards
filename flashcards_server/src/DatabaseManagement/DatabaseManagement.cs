using System;
using Npgsql;


namespace flashcards_server.DatabaseManagement
{
    public partial class DatabaseManagement
    {
        public readonly NpgsqlConnection Conn;

        public DatabaseManagement(string server, string user, string password /*temporary*/, string database)
        {
            Conn = new NpgsqlConnection($"Server={server};User Id={user}; Password={password};Database={database}");
            User.User.UserCreatedEventHandler += AddUserEvents;
            Card.Card.CardCreatedEventHandler += AddCardEvents;
        }

        public void OpenConnection()
        {
            Conn.Open();
        }

        public void CloseConnection()
        {
            Conn.Close();
        }

        void AddUserEvents(object obj, EventArgs e)
        {
            var user = obj as User.User;
            user.NameChangedEventHandler += this.UpdateUserName;
            user.EmailChangedEventHandler += this.UpdateUserEmail;
            user.SurnameChangedEventHandler += this.UpdateUserSurname;
            user.PasswordChangedEventHandler += this.UpdateUserPassword;
        }

        void AddCardEvents(object obj, EventArgs e)
        {
            var card = obj as Card.Card;
            card.AnswerChangedEventHandler += this.UpdateCardAnswer;
            card.QuestionChangedEventHandler += this.UpdateCardQuestion;
            card.ImageChangedEventHandler += this.UpdateCardPicture;
        }

    }
}
