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
    public class LoginController : ControllerBase
    {
        // [HttpGet]
        // [Route("login/{login}/{password}")]
        // public string Get(string login, string password)
        // {
        //     try
        //     {
        //         var u = db.GetUserByUsername(login);
        //         var correct = db.PasswordMatch(u, password);
        //         return $"Login: {login}\n" +
        //                $"Password: {password}\n" +
        //                $"Correct: {correct}";
        //     }
        //     catch (Npgsql.NpgsqlException e)
        //     {
        //         return e.Message;
        //     }


        // }

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
                System.Console.WriteLine(" >>>>> " + JsonSerializer.Serialize(new SuccessMessageResponseMessage { successed = true }));
                var mess = new SuccessMessageResponseMessage { successed = true };
                return new SuccessMessageResponseMessage { successed = true };
            }
            catch (Npgsql.NpgsqlException e)
            {
                return new SuccessMessageResponseMessage { successed = false, reason = e.Message };
            }
            catch (FormatException e)
            {
                return new SuccessMessageResponseMessage { successed = false, reason = e.Message };
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
                switch (updateRequest.what.ToLower())
                {
                    case "email":
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
                        return new SuccessMessageResponseMessage() { successed = false };

                }
                return new SuccessMessageResponseMessage() { successed = true };
            }
            catch (Npgsql.NpgsqlException e)
            {
                return new SuccessMessageResponseMessage() { successed = false, reason = e.Message };
            }
            catch (FormatException e)
            {
                return new SuccessMessageResponseMessage() { successed = false, reason = e.Message };
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
        [Route("isEmailUnique")]
        [EnableCors]
        [Produces("application/json")]
        public HttpResponseMessage isEmailUnique(string email)
        {
            if (db.IsValidEmail(email))
                return new IsAleradyUsedResponseMessage() { isAlreadyUsed = !db.IsUserEmailUnique(email) };
            else
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        private PublicUserResponseMessage CreatePublicUserResponseMessage(User.User u)
        {
            return new PublicUserResponseMessage { id = u.id, username = u.username };
        }

        DatabaseManagement.DatabaseManagement db = flashcards_server.Program.db;
    }
}