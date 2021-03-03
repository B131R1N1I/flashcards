using System;


namespace flashcards_server.DatabaseManagement
{

    public class PasswordValidation
    {
        public bool lengthMin8 { get; set; }
        public bool lengthMax32 { get; set; }
        public bool lowerCaseLetter { get; set; }
        public bool upperCaseLetter { get; set; }
        public bool number { get; set; }

        public bool isCorrect()
        {
            return (this.lengthMax32 && this.lengthMin8 &&
            this.lowerCaseLetter && this.number &&
            this.upperCaseLetter);
        }
    }
}