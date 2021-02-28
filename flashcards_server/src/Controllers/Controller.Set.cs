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
                return new SuccessMessageResponseMessage(true);
            }
            catch (Npgsql.NpgsqlException e)
            {
                return new SuccessMessageResponseMessage(false, e.Message, HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        [Route("allPublicSets")]
        [EnableCors]
        [Produces("application/json")]
        public List<Set.Set> GetAllSets()
        {
            return db.GetPublicSets();
        }

        [HttpGet]
        [Route("getSetById")]
        [EnableCors]
        [Produces("application/json")]
        public Set.Set GetSetById(uint id)
        {
            return db.GetSetById(id);
        }

        [HttpGet]
        [Route("getPublicSetsByNameLike")]
        [EnableCors]
        [Produces("application/json")]
        public List<Set.Set> GetPublicSetsByNameLike(string name)
        {
            return db.GetPublicSetsByNameLike(name);
        }

        [HttpPut]
        [Route("transferOwnership")]
        [EnableCors]
        [Consumes("application/json")]
        [Produces("application/json")]
        public SuccessMessageResponseMessage TransferOwnership(ChangeOwnership change)
        {
            try
            {
                var u = db.GetUserById(change.userId);
                var s = db.GetSetById(change.setId);
                db.TransferOwnership(s, u);
                return new SuccessMessageResponseMessage(true);
            }
            catch (Npgsql.NpgsqlException e)
            {
                return new SuccessMessageResponseMessage(false, e.Message, HttpStatusCode.BadRequest);
            }
        }


        private Set.Set CreateSetFromMinSet(MinSet minSet)
        {
            return new Set.Set(minSet.name, minSet.creator, minSet.owner, minSet.isPublic);
        }

        DatabaseManagement.DatabaseManagement db = flashcards_server.Program.db;
    }
}