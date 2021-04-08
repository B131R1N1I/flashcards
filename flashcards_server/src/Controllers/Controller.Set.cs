using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace flashcards_server.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("fc/set")]
    public class SetController : ControllerBase
    {
        [HttpPost]
        [Route("create")]
        [EnableCors]
        [Consumes("application/json")]
        [Produces("application/json")]
        public IActionResult CreateSet(SetToCreate setToCreate)
        {
            try
            {
                using (var context = new flashcardsContext())
                {
                    var id = LoggedInId();
                    var set = new Set.Set(setToCreate.name, id, id, setToCreate.isPublic);
                    context.sets.Add(set);
                    context.SaveChanges();
                }

                System.Console.WriteLine($">>> ADDED SET {setToCreate.name}");
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("allPublicSets")]
        [EnableCors]
        [Produces("application/json")]
        public List<Set.Set> GetAllSets()
        {
            using var context = new flashcardsContext();
            return context.sets.Where(s => s.isPublic).ToList();
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
        public List<Set.Set> GetSetsAvailableForUser()
        {
            var id = LoggedInId();
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
        public IActionResult TransferOwnership(ChangeOwnership change)
        {
            try
            {
                using var context = new flashcardsContext();
                
                var set = context.sets.First(s => s.id == change.setId);
                if (set.ownerId != LoggedInId())
                    return Unauthorized("Access denied for this set");
                
                var user = context.users.First(u => u.Id == change.userId);

                set.ownerId = user.Id;
                context.SaveChanges();
                
                return Ok();
            }
            catch (ArgumentNullException e)
            {
                return BadRequest(e.Message);
            }
        }
        
        private int LoggedInId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? 
                             throw new InvalidOperationException(
                                 $"Cannot validate - there's no user with id {ClaimTypes.NameIdentifier}"));
        }
        private Set.Set CreateSetFromMinSet(MinSet minSet)
        {
            return new Set.Set(minSet.name, minSet.creator, minSet.owner, minSet.isPublic);
        }

    }
}