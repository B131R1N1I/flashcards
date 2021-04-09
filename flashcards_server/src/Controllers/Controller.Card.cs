using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace flashcards_server.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("fc/card")]
    public class CardController : ControllerBase
    {
        [HttpPost]
        [Route("create")]
        [EnableCors]
        [Consumes("application/json")]
        [Produces("application/json")]
        public IActionResult CreateCard(MinCard c)
        {
            var id = LoggedInId();
            try
            {
                using var context = new flashcardsContext();
                if (!context.sets.Any(s => s.id == c.inSet))
                    return BadRequest($"There is no set with id {c.inSet}");
                if (!context.sets.Any(s => s.id == c.inSet && (s.creatorId == id || s.ownerId == id)))
                    return BadRequest("Access denied");
                context.cards.Add(CreateCardFromMinCard(c, LoggedInId()));
                context.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [Route("update")]
        [EnableCors]
        [Consumes("application/json")]
        [Produces("application/json")]
        public IActionResult UpdateCard(UpdateRequest updateRequest)
        {
            try
            {
                Console.WriteLine(updateRequest.id);

                using var context = new flashcardsContext();

                var card = context.cards.First(c => c.id == updateRequest.id);

                //db.GetCardById(updateRequest.id);
                Console.WriteLine("check after card");
                var what = updateRequest.what;
                var to = updateRequest.to;
                Console.WriteLine(card);
                Console.WriteLine(what);
                Console.WriteLine(to);
                switch (what.ToLower())
                {
                    case "question":
                        card.question = to;
                        break;
                    case "answer":
                        card.answer = to;
                        break;
                    case "image":
                        card.picture = updateRequest.image;
                        break;
                    default:
                        return BadRequest($"{what} isn't a proper value");
                }

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("getCardById")]
        [EnableCors]
        [Produces("application/json")]
        public PublicCard GetCardById(uint id)
        {
            try
            {
                using var context = new flashcardsContext();
                var tempCard = context.cards.First(c => c.id == id);

                return CreatePublicCardFromCard(tempCard);
            }
            catch (ArgumentNullException)
            {

                return null;
            }
        }

        [HttpGet]
        [Route("getCardsBySet")]
        [EnableCors]
        [Consumes("application/json")]
        [Produces("application/json")]
        public IEnumerable<PublicCard> GetCardsBySetId(uint id)
        {
            try
            {
                using var context = new flashcardsContext();
                var tempCard = context.cards.Where(c => c.inSet == id).ToArray();
                return CreatePublicCardFromCard(tempCard);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("getImageById")]
        [EnableCors]
        public IActionResult GetImageById(uint id)
        {
            using var context = new flashcardsContext();
            var converter = new ImageConverter();
            return File((byte[])converter.ConvertTo(context.cards.First(c => c.id == id).picture, typeof(byte[])), "image/gif");
        }

        private int LoggedInId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                             throw new InvalidOperationException(
                                 $"Cannot validate - there's no user with id {ClaimTypes.NameIdentifier}"));
        }

        private static Card.Card CreateCardFromMinCard(MinCard minCard, int ownerId)
        {
            return new Card.Card(minCard.answer, minCard.question, minCard.image, minCard.inSet, ownerId, minCard.isPublic);
        }

        private static IEnumerable<PublicCard> CreatePublicCardFromCard(IEnumerable<Card.Card> listOfCards)
        {
            Console.WriteLine("aa");
            return listOfCards.Select(CreatePublicCardFromCard);
        }

        private static PublicCard CreatePublicCardFromCard(Card.Card card)
        {
            var href = (card.picture is not null ? $"getImageById?id={card.id}" : null);
            return new PublicCard(card.id, card.question, card.answer, href, card.inSet);
        }
    }
}