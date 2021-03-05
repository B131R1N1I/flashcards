using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
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
                _db.AddUserToDatabase(u);
                System.Console.WriteLine($">> added {u.username}"); 
                return new SuccessMessageResponseMessage(true);
            }
            catch (Exception e)
            {
                if (e is FormatException || e is Npgsql.NpgsqlException ||
                    e is DatabaseManagement.NotValidPasswordException)
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
                var user = _db.GetUserById(updateRequest.id);
                var to = updateRequest.to;
                var what = updateRequest.what;
                switch (what.ToLower())
                {
                    case "email":
                        if (!_db.IsValidEmail(to))
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
        [Route("getuser/")]
        [EnableCors]
        [Produces("application/json")]
        public HttpResponseMessage GetUserPublic(uint id, string username)
        {

            try
            {
                return CreatePublicUserResponseMessage(_db.GetUserById(id));
            }
            catch (Npgsql.NpgsqlException)
            {
                try
                {
                    return CreatePublicUserResponseMessage(_db.GetUserByUsername(username));
                }
                catch (Npgsql.NpgsqlException)
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
            if (_db.IsValidEmail(email))
                return new IsAlreadyUsedResponseMessage() { isAlreadyUsed = !_db.IsUserEmailUnique(email) };
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
            return new IsAlreadyUsedResponseMessage() { isAlreadyUsed = !_db.IsUserUsernameUnique(username) };
        }

        private PublicUserResponseMessage CreatePublicUserResponseMessage(User.User u)
        {
            return new PublicUserResponseMessage() { user = new PublicUser { username = u.username, id = u.id } };
        }

        private readonly DatabaseManagement.DatabaseManagement _db = flashcards_server.Program.db;
    }
}