using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;

namespace flashcards_server.API.Controllers
{

    [ApiController]
    [Route("fc")]
    public class LoginController : ControllerBase
    {
        [HttpGet]
        [Route("login/{login}/{password}")]
        public string Get(string login, string password)
        {
            // db.OpenConnection();
            try
            {
                var u = db.GetUserByUsername(login);
                var correct = db.PasswordMatch(u, password);
                // db.CloseConnection();
                return $"Login: {login}\n" +
                       $"Password: {password}\n" +
                       $"Correct: {correct}";
            }
            catch (Npgsql.NpgsqlException e)
            {
                // db.CloseConnection();
                return e.Message;
            }


        }

        [HttpPost]
        [Route("register/")]
        [EnableCors]
        [Consumes("application/json")]
        // [Produces("application/json")]
        public SuccessMessage Register([FromBody] User.User u)
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

        DatabaseManagement.DatabaseManagement db = flashcards_server.Program.db;
    }
}