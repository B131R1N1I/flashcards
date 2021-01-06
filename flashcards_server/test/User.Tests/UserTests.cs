using System;
using Xunit;
using fs = flashcards_server;


namespace User.Tests
{
    public class UserTests
    {
        [Fact]
        public void CreateProperUser()
        {
            var uLogin = "login";
            var uEmail = "email@email.com";
            var uName = "name";
            var uSurname = "surname";
            var uPassword = "Pasword123";
            var user = new fs.User.User(uLogin, uEmail, uName, uSurname, uPassword);

            Assert.Equal(uLogin, user.username);
            Assert.Equal(uEmail, user.email);
            Assert.Equal(uName, user.name);
            Assert.Equal(uSurname, user.surname);
            Assert.Equal(uPassword, user.password);
            Assert.Null(user.id);

            var secondUserId = 4096;

            user = new fs.User.User(uLogin, uEmail, uName, uSurname, uPassword, secondUserId);

            Assert.True(user.id == secondUserId);
        }


    }
}
