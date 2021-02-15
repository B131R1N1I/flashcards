using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace flashcards_server.API.Controllers
{

    [ApiController]
    [Route("fc")]
    [Route("fc/login")]
    public class LoginController : ControllerBase
    {
        [HttpGet]
        [Route("{login}/{password}")]
        public string Get(string login, string password)
        {
            db.OpenConnection();
            try
            {
                var u = db.GetUserByUsername(login);
                var correct = db.PasswordMatch(u, password);
                db.CloseConnection();
                return $"Login: {login}\n" +
                       $"Password: {password}\n" +
                       $"Correct: {correct}";
            }
            catch (Npgsql.NpgsqlException e)
            {
                db.CloseConnection();
                return e.Message;
            }


        }
        DatabaseManagement.DatabaseManagement db = flashcards_server.Program.db;
    }
}