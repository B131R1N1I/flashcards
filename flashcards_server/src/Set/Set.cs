using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace flashcards_server.Set
{
    public partial class Set
    {
        public static event EventHandler SetCreatedEventHandler;

        public event EventHandler<NameEventArgs> NameChangedEventHandler;

        public event EventHandler<UserEventArgs> OwnerChangedEventHandler;

        public event EventHandler<DateEventArgs> LastModificationDateChanged;

        public event EventHandler<IsPublicEventArgs> IsPublicChanged;

        protected virtual void OnSetCreated(Set set)
        {
            if (SetCreatedEventHandler != null)
                SetCreatedEventHandler(set, new EventArgs());
        }

        protected virtual void OnNameChanged(string name)
        {
            if (NameChangedEventHandler != null)
                NameChangedEventHandler(this, new NameEventArgs { name = name });
        }

        protected virtual void OnOwnerChanged(uint user)
        {
            if (OwnerChangedEventHandler != null)
                OwnerChangedEventHandler(this, new UserEventArgs { user = user });
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

        public uint? id { get; private set; }

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

        private uint _creator;

        public uint creator
        {
            get => _creator;
            private set
            {
                try
                {
                    _creator = value;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Cannot set creator to user with id: " + value);
                    Console.WriteLine(e.Message);
                }
            }
        }

        private uint _owner;

        public uint owner
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
                    Console.WriteLine("Cannot change owner to user with id: " + value);
                    Console.WriteLine(e.Message);
                }
            }
        }

        private DateTime _createdDate;

        public DateTime createdDate
        {
            get => _createdDate;
            private set
            {
                try
                {
                    _createdDate = value;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Cannot set created date to: " + value.ToString());
                    Console.WriteLine(e.Message);
                }
            }

        }

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

        public Set(string name, uint creator, uint owner, DateTime createdDate, DateTime lastModification, bool isPublic, uint? id = null)
        {
            this.id = id;
            this.name = name;
            this.creator = creator;
            this.owner = owner;
            this.createdDate = createdDate;
            this.lastModificationDate = lastModification;
            this.isPublic = isPublic;
            OnSetCreated(this);
        }

        public Set(string name, uint creator, uint owner, DateTime createdDate, DateTime lastModification, bool isPublic, int id) :
        this(name, creator, owner, createdDate, lastModification, isPublic, (uint?)id)
        { }

        public Set(string name, uint creator, uint owner, bool isPublic) :
        this(name, creator, owner, createdDate: DateTime.Now, lastModification: DateTime.Now, isPublic)
        { }

        public override string ToString()
        {
            return $"Set [{id}: ({name}, {creator}, {owner}, {createdDate}, {lastModificationDate}, {isPublic})]";
        }

        public override bool Equals(object obj)
        {
            return (obj is Set && (((Set)obj).name == this.name && ((Set)obj).id == ((Set)this).id));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
