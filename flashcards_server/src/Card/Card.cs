using System;


namespace flashcards_server.Card
{
    public partial class Card
    {
        public event EventHandler<QuestionEventArgs> QuestionChangedEventHandler;

        public event EventHandler<AnswerEventArgs> AnswerChangedEventHandler;

        public event EventHandler<ImageEventArgs> ImageChangedEventHandler;

        public event EventHandler<InSetEventArgs> InSetChangedEventHandler;

        protected virtual void OnQuestionChanged(string question)
        {
            if (QuestionChangedEventHandler != null)
                QuestionChangedEventHandler(question, new QuestionEventArgs { question = question });
        }

        protected virtual void OnAnswerChanged(string answer)
        {
            if (AnswerChangedEventHandler != null)
                AnswerChangedEventHandler(question, new AnswerEventArgs { answer = question });
        }

        protected virtual void OnImageChanged(Byte[] image)
        {
            if (ImageChangedEventHandler != null)
                ImageChangedEventHandler(this, new ImageEventArgs { image = image });
        }

        protected virtual void OnInSetChanged(uint inSet)
        {
            if (InSetChangedEventHandler != null)
                InSetChangedEventHandler(this, new InSetEventArgs { inSet = inSet });
        }

        public readonly uint? id;

        private string _question;

        public string question
        {
            get => _question;
            set
            {
                try
                {
                    OnQuestionChanged(value);
                    _question = value;

                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Cannot change card question to: " + value);
                    System.Console.WriteLine(e.Message);
                }
            }
        }

        private string _answer;

        public string answer
        {
            get => _answer;
            set
            {
                try
                {
                    // OnAnswerChanged(value)
                    _answer = value;
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Cannod set card answer to: " + value);
                    System.Console.WriteLine(e.Message);
                }
            }
        }

        private Byte[] _image;

        public Byte[] image
        {
            get => _image;
            set
            {
                try
                {
                    OnImageChanged(value);
                    _image = value;
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Canno set card image!! ;(");
                    System.Console.WriteLine(e.Message);
                }
            }
        }

        private uint _inSet;

        public uint inSet
        {
            get => _inSet;
            set
            {
                try
                {
                    OnInSetChanged(value);
                    _inSet = value;
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Canot change in_set property");
                    System.Console.WriteLine(e.Message);
                }
            }
        }
    }
}