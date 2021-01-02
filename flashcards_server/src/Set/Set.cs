using System;

namespace flashcards_server.Set
{
    public partial class Set
    {
        public event EventHandler<NameEventArgs> NameChangedEventHandler;
        public event EventHandler<OwnerEventArgs> OwnerChangedEventHandler;

        public event EventHandler<DateEventArgs> LastModificationDateChanged;

        public event EventHandler<IsPublicEventArgs> IsPublicChanged;

        protected virtual void OnNameChanged(string name)
        {
            if (NameChangedEventHandler != null)
                NameChangedEventHandler(this, new NameEventArgs { name = name });
        }

        protected virtual void OnOwnerChanged(User.User user)
        {
            if (OwnerChangedEventHandler != null)
                OwnerChangedEventHandler(this, new OwnerEventArgs { user = user });
        }

        protected void OnLastModificationDateChanged(DateTime date)
        {
            if (LastModificationDateChanged != null)
                LastModificationDateChanged(this, new DateEventArgs { date = date });
        }
        protected void OnIsPublicChanged(bool isPublic)
        {
            if (IsPublicChanged != null)
                IsPublicChanged(this, new IsPublicEventArgs { isPublic = isPublic });
        }

        public uint id;

        private string _name;

        public string name
        {
            get => _name;
            set
            {
                try
                {
                    OnNameChanged(value);
                    _name = value;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Cannot change set name to: " + value);
                    Console.WriteLine(e.Message);
                }
            }
        }

        public readonly User.User creator;

        private User.User _owner;

        public User.User owner
        {
            get => _owner;
            set
            {
                try
                {
                    OnOwnerChanged(value);
                    _owner = value;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Cannot change owner to user named: " + value.name);
                    Console.WriteLine(e.Message);
                }
            }
        }

        public readonly DateTime createdDate;

        private DateTime _lastModificationDate;

        public DateTime lastModificationDate
        {
            get => _lastModificationDate;
            set
            {
                try
                {
                    OnLastModificationDateChanged(value);
                    _lastModificationDate = value;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Cannot change last modification date to: " + value.ToString());
                    Console.WriteLine(e.Message);
                }
            }

        }

        private bool _isPublic;

        public bool isPublic
        {
            get => _isPublic;
            set
            {
                try
                {
                    OnIsPublicChanged(value);
                    _isPublic = value;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Cannot change last modification date to: " + value.ToString());
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
