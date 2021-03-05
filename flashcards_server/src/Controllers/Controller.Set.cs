using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace flashcards_server.Controllers
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
                _db.AddSetToDatabase(set);
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
            return _db.GetPublicSets();
        }

        [HttpGet]
        [Route("getSetById")]
        [EnableCors]
        [Produces("application/json")]
        public Set.Set GetSetById(uint id)
        {
            try
            {
                return _db.GetSetById(id);
            }
            catch (Npgsql.NpgsqlException)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("getSetsForMe")]
        [EnableCors]
        [Produces("application/json")]
        public List<Set.Set> GetSetsAvailableForUser(uint id)
        {
            return _db.GetSetsByCreatorOrOwner(_db.GetUserById(id));
        }

        [HttpGet]
        [Route("getPublicSetsByNameLike")]
        [EnableCors]
        [Produces("application/json")]
        public List<Set.Set> GetPublicSetsByNameLike(string name)
        {
            return _db.GetPublicSetsByNameLike(name);
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
                var u = _db.GetUserById(change.userId);
                var s = _db.GetSetById(change.setId);
                _db.TransferOwnership(s, u);
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

        private readonly DatabaseManagement.DatabaseManagement _db = flashcards_server.Program.db;
    }
}