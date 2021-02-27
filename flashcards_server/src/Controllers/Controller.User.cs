using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using System.Net.Http;
using System.Net;

namespace flashcards_server.API.Controllers
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
                db.AddUserToDatabase(u);
                System.Console.WriteLine($">> added {u.username}");
                var mess = new SuccessMessageResponseMessage(true);
                return new SuccessMessageResponseMessage(true);
            }
            catch (Exception e)
            {
                if (e is FormatException || e is Npgsql.NpgsqlException)
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
                var user = db.GetUserById(updateRequest.id);
                var to = updateRequest.to;
                var what = updateRequest.what;
                switch (what.ToLower())
                {
                    case "email":
                        if (!db.IsValidEmail(to))
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
                return CreatePublicUserResponseMessage(db.GetUserById(id));
            }
            catch (Npgsql.NpgsqlException)
            {
                try
                {
                    return CreatePublicUserResponseMessage(db.GetUserByUsername(username));
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
        public HttpResponseMessage isEmailUsed(string email)
        {
            if (db.IsValidEmail(email))
                return new IsAleradyUsedResponseMessage() { isAlreadyUsed = !db.IsUserEmailUnique(email) };
            else
                return new SuccessMessageResponseMessage(false,
                                                         $"{email} isn't correct email format.",
                                                         HttpStatusCode.BadRequest);
        }

        [HttpGet]
        [Route("isUsernameAlreadyUsed")]
        [EnableCors]
        [Produces("application/json")]
        public HttpResponseMessage isUsernameUsed(string username)
        {
            return new IsAleradyUsedResponseMessage() { isAlreadyUsed = !db.IsUserUsernameUnique(username) };
        }

        private PublicUserResponseMessage CreatePublicUserResponseMessage(User.User u)
        {
            return new PublicUserResponseMessage { id = u.id, username = u.username };
        }

        DatabaseManagement.DatabaseManagement db = flashcards_server.Program.db;
    }
}