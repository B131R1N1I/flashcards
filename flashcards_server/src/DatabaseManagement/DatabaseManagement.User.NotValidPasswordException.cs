using System;

namespace flashcards_server.DatabaseManagement
{
    public class NotValidPasswordException : Exception
    {
        public PasswordValidation passwordValidation { get; set; }

        public NotValidPasswordException(string message, PasswordValidation passwordValidation) : base(message)
        {
            this.passwordValidation = passwordValidation;
        }
    }
}