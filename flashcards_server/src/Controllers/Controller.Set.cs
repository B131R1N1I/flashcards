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
    [Route("fc/set")]
    public class SetController : ControllerBase
    {
        [HttpPost]
        [Route("create")]
        [EnableCors]
        [Consumes("application/json")]
        [Produces("application/json")]
        public SuccessMessageResponseMessage CreateSet(TempMinSet minSet)
        {
            var set = CreateSetFromTempMinSet(minSet);
            System.Console.WriteLine(" wqereqwrewrqrewqrewqierwqkoerwiierwoioewiowerienlnk");
            try
            {
                db.AddSetToDatabase(set);
                System.Console.WriteLine($">>> ADDED SET {set.name}");
                return new SuccessMessageResponseMessage() { successed = true };
            }
            catch (Npgsql.NpgsqlException e)
            {
                return new SuccessMessageResponseMessage() { successed = false, reason = e.Message, StatusCode = HttpStatusCode.BadRequest };
            }
        }

        private Set.Set CreateSetFromTempMinSet(TempMinSet minSet)
        {
            return new Set.Set(minSet.name, minSet.creator, minSet.owner, minSet.isPublic);
        }
        DatabaseManagement.DatabaseManagement db = flashcards_server.Program.db;

    }
}