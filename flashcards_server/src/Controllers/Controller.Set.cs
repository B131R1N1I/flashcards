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
        public SuccessMessageResponseMessage CreateSet(MinSet minSet)
        {
            var set = CreateSetFromMinSet(minSet);
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

        [HttpGet]
        [Route("allPublicSets")]
        [EnableCors]
        [Produces("application/json")]
        public List<Set.Set> getAllSets()
        {
            return db.GetPublicSets();
        }

        private Set.Set CreateSetFromMinSet(MinSet minSet)
        {
            return new Set.Set(minSet.name, minSet.creator, minSet.owner, minSet.isPublic);
        }
        DatabaseManagement.DatabaseManagement db = flashcards_server.Program.db;

    }
}