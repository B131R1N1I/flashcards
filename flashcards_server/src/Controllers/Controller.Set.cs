using System;
using System.Collections.Generic;
using System.Linq;
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
                using (var context = new flashcardsContext())
                {
                    context.sets.Add(set);
                    context.SaveChanges();
                }

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
            using (var context = new flashcardsContext())
                return context.sets.Where(s => s.isPublic == true).ToList();
        }

        [HttpGet]
        [Route("getSetById")]
        [EnableCors]
        [Produces("application/json")]
        public Set.Set GetSetById(uint id)
        {
            using var context = new flashcardsContext();
                return context.sets.First(s => s.id == id);
        }

        [HttpGet]
        [Route("getSetsForMe")]
        [EnableCors]
        [Produces("application/json")]
        public List<Set.Set> GetSetsAvailableForUser(uint id)
        {
            using var context = new flashcardsContext();
            return context.sets.Where(s => s.ownerId == id || s.creatorId == id).ToList();
        }

        [HttpGet]
        [Route("getPublicSetsByNameLike")]
        [EnableCors]
        [Produces("application/json")]
        public List<Set.Set> GetPublicSetsByNameLike(string name)
        {
            using (var context = new flashcardsContext())
            {
                return context.sets.Where(s => s.name.Contains(name)).ToList();
            }
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
                using var context = new flashcardsContext();
                
                var u = context.users.First(u => u.Id == change.userId); 
                // db.GetUserById(change.userId);
                var s = context.sets.First(s => s.id == change.setId);
                // db.GetSetById(change.setId);
                s.ownerId = u.Id;
                // db.TransferOwnership(s, u);
                return new SuccessMessageResponseMessage(true);
            }
            catch (ArgumentNullException e)
            {
                return new SuccessMessageResponseMessage(false, e.Message, HttpStatusCode.BadRequest);
            }
        }


        private Set.Set CreateSetFromMinSet(MinSet minSet)
        {
            return new Set.Set(minSet.name, minSet.creator, minSet.owner, minSet.isPublic);
        }

    }
}