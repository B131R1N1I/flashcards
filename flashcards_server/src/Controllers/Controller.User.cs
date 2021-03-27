using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace flashcards_server.Controllers
{
    [ApiController]
    [Route("fc/user")]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [Route("register/")]
        [EnableCors]
        [Consumes("application/json")]
        [Produces("application/json")]
        public SuccessMessageResponseMessage Register(User.User u)
        {
            try
            {
                using var context = new flashcardsContext();
                if (!IsValidEmail(u.email))
                    throw new FormatException("Email format is not valid.");
                if (context.users.Any(user => user.username == u.username | user.email == u.email))
                    if (context.users.Any(user => user.username == u.username))
                        throw new FormatException("Username is already used");
                    else throw new FormatException("Email is already used");
                context.users.Add(u);
                context.SaveChanges();
                System.Console.WriteLine($">> added {u.username}");
                return new SuccessMessageResponseMessage(true);
            }
            catch (FormatException e)
            {
                return new SuccessMessageResponseMessage(false, e.Message);
                throw;
            }
        }

        [HttpPut]
        [Route("update/")]
        [EnableCors]
        [Consumes("application/json")]
        [Produces("application/json")]
        public SuccessMessageResponseMessage UpdateUserData(UpdateRequest updateRequest)
        {
            try
            {
                using (var context = new flashcardsContext())
                {
                    var user = context.users.First(u => u.id == updateRequest.id);
                
                var to = updateRequest.to;
                var what = updateRequest.what;
                switch (what.ToLower())
                {
                    case "email":
                        if (IsValidEmail(to))
                            throw new FormatException($"{to} isn't correct email format.");
                        user.email = to;
                        break;
                    case "name":
                        user.name = to;
                        break;
                    case "surname":
                        user.surname = to;
                        break;
                    case "password":
                        user.password = to;
                        break;
                    default:
                        return new SuccessMessageResponseMessage(false,
                            $"'{what}' is not valid property",
                            HttpStatusCode.BadRequest);
                }
                }
                return new SuccessMessageResponseMessage(true);
            }
            catch (Exception e)
            {
                if (e is FormatException || e is Npgsql.NpgsqlException)
                    return new SuccessMessageResponseMessage(false,
                                                             e.Message,
                                                             HttpStatusCode.BadRequest);
                throw;
            }
        }

        [HttpGet]
        [Route("getUsers/")]
        [EnableCors]
        [Produces("application/json")]
        public List<PublicUser> getPublicUsers()
        {
            using (var context = new flashcardsContext())
                return context.users.Cast<PublicUser>().ToList();
        }

        [HttpGet]
        [Route("getuser/")]
        [EnableCors]
        [Produces("application/json")]
        public HttpResponseMessage GetUserPublic(uint id, string username)
        {

            try
            {
                using var context = new flashcardsContext();
                return CreatePublicUserResponseMessage(context.users.First(u => u.id == id));
            }
            catch (Exception)
            {
                try
                {
                    using var context = new flashcardsContext();
                    return CreatePublicUserResponseMessage(context.users.First(u => u.username == username));
                }
                catch (Exception)
                {
                    return new HttpResponseMessage(HttpStatusCode.NoContent);
                }
            }
        }

        [HttpGet]
        [Route("isEmailAlreadyUsed")]
        [EnableCors]
        [Produces("application/json")]
        public HttpResponseMessage IsEmailUsed(string email)
        {
            if (IsValidEmail(email))
            {
                using var context = new flashcardsContext();
                return new IsAleradyUsedResponseMessage() { isAlreadyUsed = context.users.Any(u => u.email == email)};
            }

            else
                return new SuccessMessageResponseMessage(false,
                    $"{email} isn't correct email format.",
                    HttpStatusCode.BadRequest);
        }

        [HttpGet]
        [Route("isUsernameAlreadyUsed")]
        [EnableCors]
        [Produces("application/json")]
        public HttpResponseMessage IsUsernameUsed(string username)
        {
            // return new IsAleradyUsedResponseMessage() { isAlreadyUsed = !db.IsUserUsernameUnique(username) };
            using var context = new flashcardsContext();
                return new IsAleradyUsedResponseMessage() { isAlreadyUsed = context.users.Any(u => u.username == username)};
        }

        private PublicUserResponseMessage CreatePublicUserResponseMessage(User.User u)
        {
            return new PublicUserResponseMessage { user = new PublicUser(u.id, u.username) };
        }

        static bool IsValidEmail(string email)
        {
            try {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch {
                return false;
            }
        }

    }
}