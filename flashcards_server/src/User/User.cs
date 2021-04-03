using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using flashcards_server.Controllers;
using Microsoft.AspNetCore.Identity;


namespace flashcards_server.User
{
    public partial class User : IdentityUser<int>
    {
        public User()
        {
            sets = new HashSet<Set.Set>();
        }

        public override int Id { get; set; }
        public override string UserName { get; set; }
        public override string Email { get; set; }
        public override bool EmailConfirmed { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string password { get; set; }
        public bool active { get; set; }
        [NotMapped] public override string PasswordHash { get; set; }

        [NotMapped] public override int AccessFailedCount { get; set; }
        [NotMapped] public override string NormalizedUserName { get; set; }
        [NotMapped] public override string NormalizedEmail { get; set; }
        [NotMapped] public override string SecurityStamp { get; set; }
        [NotMapped] public override string ConcurrencyStamp { get; set; }
        [NotMapped] public override string PhoneNumber { get; set; }
        [NotMapped] public override bool PhoneNumberConfirmed { get; set; }
        [NotMapped] public override bool TwoFactorEnabled { get; set; }
        [NotMapped] public override bool LockoutEnabled { get; set; }
        [NotMapped] public override DateTimeOffset? LockoutEnd { get; set; }
        

        public virtual ICollection<Set.Set> sets { get; set; }
        
        
        [JsonConstructor]
        public User(string username, string email, string name, string surname, string password, int id)
        {
            this.Id = id;
            this.UserName = username;
            this.Email = email;
            this.name = name;
            this.surname = surname;
            this.password = password;
        }
        
        public override string ToString()
        {
            return $"User [{Id}: ({UserName}, {Email}, {name}, {surname})]";
        }

        public override bool Equals(object obj)
        {
            return (obj is User && ((User)obj).name == this.name && ((User)obj).Email == this.Email);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static implicit operator PublicUser(User user)
        {
            return new PublicUser(user.Id, user.UserName);
        }
        
    }
    
    
}
