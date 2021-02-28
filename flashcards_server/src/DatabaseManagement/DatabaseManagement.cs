using System;
using Npgsql;


namespace flashcards_server.DatabaseManagement
{
    public partial class DatabaseManagement
    {
        public readonly NpgsqlConnection conn;

        public DatabaseManagement(string server, string user, string password /*temporary*/, string database)
        {
            conn = new NpgsqlConnection($"Server={server};User Id={user}; Password={password};Database={database}");
            User.User.UserCreatedEventHandler += AddUserEvents;
            Card.Card.CardCreatedEventHandler += AddCardEvents;
        }

        public void OpenConnection()
        {
            conn.Open();
        }

        public void CloseConnection()
        {
            conn.Close();
        }

        void AddUserEvents(object obj, EventArgs e)
        {
            var user = (User.User)obj;
            user.NameChangedEventHandler += this.UpdateUserName;
            user.EmailChangedEventHandler += this.UpdateUserEmail;
            user.SurnameChangedEventHandler += this.UpdateUserSurname;
            user.PasswordChangedEventHandler += this.UpdateUserPassword;
        }

        void AddCardEvents(object obj, EventArgs e)
        {
            var card = (Card.Card)obj;
            card.AnswerChangedEventHandler += this.UpdateCardAnswer;
            card.QuestionChangedEventHandler += this.UpdateCardQuestion;
            card.ImageChangedEventHandler += this.UpdateCardPicture;
        }

    }
}
