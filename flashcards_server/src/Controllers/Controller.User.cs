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
        public SuccessMessage Register(User.User u)
        {
            try
            {
                db.AddUserToDatabase(u);
                System.Console.WriteLine($">> added {u.username}");
                System.Console.WriteLine(" >>>>> " + JsonSerializer.Serialize(new SuccessMessage { successed = true }));
                var mess = new SuccessMessage { successed = true };
                return new SuccessMessage { successed = true };
            }
            catch (Npgsql.NpgsqlException)
            {
                return new SuccessMessage { successed = false };
            }
        }

        [HttpPut]
        [Route("update/")]
        [EnableCors]
        [Consumes("application/json")]
        [Produces("application/json")]
        public SuccessMessage UpdateUserData(UpdateRequest updateRequest)
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
                        return new SuccessMessage() { successed = false };

                }
                return new SuccessMessage() { successed = true };
            }
            catch (Npgsql.NpgsqlException)
            {
                return new SuccessMessage() { successed = false };
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
                return CreatePublicUser(db.GetUserById(id));
            }
            catch (Npgsql.NpgsqlException)
            {
                try
                {
                    return CreatePublicUser(db.GetUserByUsername(username));
                }
                catch (Npgsql.NpgsqlException)
                {
                    return new HttpResponseMessage(HttpStatusCode.NoContent);
                }
            }
        }

        private PublicUser CreatePublicUser(User.User u)
        {
            return new PublicUser { id = u.id, username = u.username };
        }

        DatabaseManagement.DatabaseManagement db = flashcards_server.Program.db;
    }
}