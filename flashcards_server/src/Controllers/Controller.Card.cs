using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace flashcards_server.Controllers
{
    [ApiController]
    [Route("fc/card")]
    public class CardController : ControllerBase
    {
        [HttpPost]
        [Route("create")]
        [EnableCors]
        [Consumes("application/json")]
        [Produces("application/json")]
        public HttpResponseMessage CreateCard(MinCard c)
        {
            try
            {
                using (var context = new flashcardsContext())
                {
                    if (!context.sets.Any(s => s.id == c.inSet))
                        throw new ArgumentException($"There is no set with id {c.inSet}");
                        context.cards.Add(CreateCardFromMinCard(c));
                    context.SaveChanges();

                    return new SuccessMessageResponseMessage(true);
                }
            }
            catch (Exception e)
            {
                return new SuccessMessageResponseMessage(false, e.Message, HttpStatusCode.BadRequest);
            }
        }

        [HttpPut]
        [Route("update")]
        [EnableCors]
        [Consumes("application/json")]
        [Produces("application/json")]
        public HttpResponseMessage UpdateCard(UpdateRequest updateRequest)
        {
            try
            {
                System.Console.WriteLine(updateRequest.id);
                using (var context = new flashcardsContext())
                {
                    var card = context.cards.First(c => c.id == updateRequest.id);

                    //db.GetCardById(updateRequest.id);
                    System.Console.WriteLine("check after card");
                    var what = updateRequest.what;
                    var to = updateRequest.to;
                    System.Console.WriteLine(card);
                    System.Console.WriteLine(what);
                    System.Console.WriteLine(to);
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
                            return new SuccessMessageResponseMessage(false, $"{what} isn't a proper value");
                    }

                    return new SuccessMessageResponseMessage(true);
                }
            }
            catch (Exception e)
            {
                return new SuccessMessageResponseMessage(false, e.Message, HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        [Route("getCardById")]
        [EnableCors]
        [Produces("application/json")]
        public HttpResponseMessage GetCardById(uint id)
        {
            try
            {
                using (var context = new flashcardsContext())
                {
                    var tempCard = context.cards.First(c => c.id == id);

                    //db.GetCardById(id);
                    var card = CreatePublicCardFromCard(tempCard);
                    return new PublicCardMessage() {card = card};
                }
            }
            catch (ArgumentNullException e)
            {
                
                return new SuccessMessageResponseMessage(false, e.Message, HttpStatusCode.NoContent);
            }
        }

        [HttpGet]
        [Route("getCardsBySet")]
        [EnableCors]
        [Consumes("application/json")]
        [Produces("application/json")]
        public List<PublicCard> GetCardsBySetId(uint id)
        {
            try
            {
                using (var context = new flashcardsContext())
                {
                    var temp_card = context.cards.Where(c => c.inSet == id).ToList();
                    var cards = CreatePublicCardFromCard(temp_card);
                    return cards;
                }
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
            using (var context = new flashcardsContext())
            {
                var converter = new System.Drawing.ImageConverter();
                return File((byte[]) converter.ConvertTo(context.cards.First(c => c.id == id).picture, typeof(byte[])), "image/gif");
            }
        }

        Card.Card CreateCardFromMinCard(MinCard minCard)
        {
            return new Card.Card(minCard.answer, minCard.question, minCard.image, minCard.inSet);
        }

        List<PublicCard> CreatePublicCardFromCard(List<Card.Card> listOfCards)
        {
            var l = new List<PublicCard>();
            foreach (var i in listOfCards)
                l.Add(CreatePublicCardFromCard(i));
            return l;
        }

        PublicCard CreatePublicCardFromCard(Card.Card card)
        {
            var href = (card.picture is not null ? $"getImageById?id={card.id}" : null);
            return new PublicCard(card.id, card.question, card.answer, href, card.inSet);
        }
    }
}