using System;

namespace flashcards_server.DatabaseManagement
{
    public class NotValidPasswordException : Exception
    {
        public PasswordValidation passwordValidation { get; set; }

        public NotValidPasswordException(string _message, PasswordValidation _passwordValidation) : base(_message)
        {
            passwordValidation = _passwordValidation;
        }
    }
}