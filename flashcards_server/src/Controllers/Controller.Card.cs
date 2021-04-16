using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.IO;

namespace flashcards_server.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("fc/card")]
    public class CardController : ControllerBase
    {
        private readonly string rootPath = Startup.rootPath;
        private readonly string imagePath;

        private readonly string[] imageExtensions = { ".gif", ".jpg", "jpeg", ".png" };

        public CardController(IConfiguration configuration)
        {
            imagePath = rootPath + "/../data/images/cards/";
        }

        [HttpPost]
        [Route("create")]
        [EnableCors]
        // [Consumes("application/json")]
        // [Produces("application/json")]
        public IActionResult CreateCard([FromForm] MinCard c)
        {
            var id = LoggedInId();
            try
            {
                using var context = new flashcardsContext();
                if (!context.sets.Any(s => s.id == c.inSet))
                    return BadRequest($"There is no set with id {c.inSet}");
                if (!context.sets.Any(s => s.id == c.inSet && (s.creatorId == id
                                                               || s.ownerId == id)))
                    return BadRequest("Access denied");
                var card = CreateCardFromMinCard(c, LoggedInId());
                context.cards.Add(card);

                // SaveImage(c.image, card.id);
                context.SaveChanges();

                card.picture = SaveImage(c.image, card.id);
                context.cards.Update(card);
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
        // [Consumes("application/json")]
        // [Produces("application/json")]
        public IActionResult UpdateCard([FromForm] UpdateRequest updateRequest)
        {
            try
            {
                Console.WriteLine(updateRequest.id);

                using var context = new flashcardsContext();

                var card = context.cards.First(c => c.id == updateRequest.id
                                                    && c.ownerId == LoggedInId());

                //db.GetCardById(updateRequest.id);
                Console.WriteLine("check after card");
                var what = updateRequest.what;
                var to = updateRequest.to;
                switch (what.ToLower())
                {
                    case "question":
                        card.question = to;
                        break;
                    case "answer":
                        card.answer = to;
                        break;
                    case "image":
                        card.picture = SaveImage(updateRequest.image, card.id);
                        break;
                    default:
                        return BadRequest($"{what} isn't a proper value");
                }
                context.Update(card);
                context.SaveChanges();

                return Ok();
            }
            catch (InvalidOperationException)
            {
                return Unauthorized("Access denied");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // [HttpPut]
        // [Route("updateImage")]
        // [EnableCors]
        // [Consumes("image/png")]
        // [Produces("application/json")]
        // public IActionResult updateImage()
        // {

        // }

        [HttpGet]
        [Route("getCardById")]
        [EnableCors]
        [Produces("application/json")]
        public PublicCard GetCardById(uint id)
        {
            try
            {
                using var context = new flashcardsContext();
                // var tempCard = context.cards.First(c => c.id == id && (c.ownerId == LoggedInId() || c.isPublic)); // A
                var tempCard =
                    (
                        from c in context.cards
                        where c.id == id && (c.ownerId == LoggedInId() || c.isPublic)
                        select c
                    ).First();
                return CreatePublicCardFromCard(tempCard);
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("getCardsBySet")]
        [EnableCors]
        // [Produces("application/json")]
        public IEnumerable<PublicCard> GetCardsBySetId(uint id)
        {
            try
            {
                using var context = new flashcardsContext();
                if (!context.sets.Any(s => s.id == id && (s.ownerId == LoggedInId() || s.isPublic)))
                    return null;
                var tempCard = context.cards.Where(c => c.inSet == id && (c.ownerId == LoggedInId()
                                                                          || c.isPublic)).ToList();
                return CreatePublicCardFromCard(tempCard);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        [Route("updateImage")]
        [EnableCors]
        public IActionResult UploadImage([FromForm] UploadImageViewModel model)
        {
            var cardId = model.cardId;
            var file = model.file;

            // checks extension
            if (!imageExtensions.Any(x => file.FileName.EndsWith(x)))
                return UnprocessableEntity();

            using var context = new flashcardsContext();

            // returns card if exists and is public or the user is owner
            // if doesn't exist returns null
            var card = (from c in context.cards
                        where c.id == model.cardId
                              && (c.ownerId == LoggedInId() || c.isPublic)
                        select c).SingleOrDefault();

            // 404 if card not found
            if (card is null)
                return NotFound();

            // user cannot edit card if is not owner
            if (card.ownerId != LoggedInId())
                return Forbid();

            var name = SaveImage(file, cardId);
            if (name is null)
                return BadRequest();
            card.picture = name;
            context.cards.Update(card);
            context.SaveChanges();
            return Ok();

        }

        [HttpGet]
        [Route("getImageById")]
        [EnableCors]
        public IActionResult GetImageById(uint id)
        {
            using var context = new flashcardsContext();
            var imageFileName = (from c in context.cards
                                 where c.id == id && (c.ownerId == LoggedInId() || c.isPublic)
                                 select c.picture).FirstOrDefault();
            if (imageFileName is null)
                return NotFound();
            var path = Path.Combine(imagePath, imageFileName);
            System.Console.WriteLine(path);
            // var converter = new ImageConverter();
            if (path.EndsWith(".png", true, null))
                return PhysicalFile(path, "image/png");
            else if (path.EndsWith(".jpg", true, null) || path.EndsWith(".jpeg", true, null))
                return PhysicalFile(path, "image/jpeg");
            else if (path.EndsWith(".gif", true, null))
                return PhysicalFile(path, "image/gif");
            return null;
            // return File((byte[])converter.ConvertTo(context.cards.First(c => c.id == id && (c.ownerId == LoggedInId() || c.isPublic)).picture, typeof(byte[])), "image/gif");
        }

        private string SaveImage(IFormFile file, int imageId)
        {
            var uniqueFileName = "";
            if (file == null) return null;
            uniqueFileName = Path.Combine($"{imageId}.png");
            var fullPath = Path.Combine(imagePath + uniqueFileName);
            System.Console.WriteLine(fullPath);
            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            return uniqueFileName;

        }

        private int LoggedInId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                             throw new InvalidOperationException(
                                 $"Cannot validate - there's no user with id {ClaimTypes.NameIdentifier}"));
        }

        private static Card.Card CreateCardFromMinCard(MinCard minCard, int ownerId)
            => new Card.Card(minCard.answer,
                             minCard.question,
                             minCard.inSet,
                             ownerId,
                             minCard.isPublic);

        private IEnumerable<PublicCard> CreatePublicCardFromCard(IEnumerable<Card.Card> listOfCards)
            => listOfCards.Select(CreatePublicCardFromCard);

        private PublicCard CreatePublicCardFromCard(Card.Card card)
            => new PublicCard(card.id,
                              card.question,
                              card.answer,
                              card.picture is not null,
                              card.inSet);
    }
}