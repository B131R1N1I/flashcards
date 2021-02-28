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
                var card = CreateCardFromMinCard(c);
                db.AddCardToDatabase(card);
                return new SuccessMessageResponseMessage(true);
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
                var card = db.GetCardByID(updateRequest.id);
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
                        card.image = updateRequest.image;
                        break;
                    default:
                        return new SuccessMessageResponseMessage(false, $"{what} isn't a proper value");

                }
                return new SuccessMessageResponseMessage(true);
            }
            catch (Exception e)
            {
                return new SuccessMessageResponseMessage(false, e.Message, HttpStatusCode.BadRequest);
            }
        }

        Card.Card CreateCardFromMinCard(MinCard minCard)
        {
            return new Card.Card(minCard.answer, minCard.question, minCard.image, minCard.inSet);
        }

        DatabaseManagement.DatabaseManagement db = flashcards_server.Program.db;
    }
}