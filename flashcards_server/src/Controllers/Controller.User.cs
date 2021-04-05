using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace flashcards_server.Controllers
{
    [ApiController]
    [Route("fc/user")]
    public class UserController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        [HttpPost]
        [Route("register/")]
        [EnableCors]
        [Consumes("application/json")]
        [Produces("application/json")]
        public SuccessMessageResponseMessage Register(User.User u)
        {
            try
            {
                using var context = new flashcardsContext();
                if (!IsValidEmail(u.Email))
                    throw new FormatException("Email format is not valid.");
                if (context.users.Any(user => user.UserName == u.UserName | user.Email == u.Email))
                    if (context.users.Any(user => user.UserName == u.UserName))
                        throw new FormatException("Username is already used");
                    else throw new FormatException("Email is already used");
                context.users.Add(u);
                context.SaveChanges();
                Console.WriteLine($">> added {u.UserName}");
                return new SuccessMessageResponseMessage(true);
            }
            catch (FormatException e)
            {
                return new SuccessMessageResponseMessage(false, e.Message);
                throw;
            }
        }

        [HttpPost]
        [Route("login/")]
        [EnableCors]
        [Consumes("application/json")]
        public IActionResult Login(LoginData loginData)
        {
            
            using var context = new flashcardsContext();
            IActionResult response = Unauthorized();
            try
            {
                var user = context.users.First(u => u.UserName == loginData.login && u.password == loginData.password);
                var token = GenerateJSONWebToken(user);
                response = Ok(new {token});
                Console.WriteLine(user);
                
                return response;
            }
            catch (InvalidOperationException)
            {
                return response;
            }

        }
        
        [HttpPut]
        [Route("update/")]
        [EnableCors]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public HttpResponseMessage UpdateUserData([FromBody]UpdateRequest updateRequest)
        {
            if (updateRequest.id != loggedInId())
                 return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            try
            {
                using var context = new flashcardsContext();
                var user = context.users.First(u => u.Id == updateRequest.id);
                
                var to = updateRequest.to;
                var what = updateRequest.what;
                switch (what.ToLower())
                {
                    case "email":
                        if (IsValidEmail(to))
                            throw new FormatException($"{to} isn't correct email format.");
                        user.Email = to;
                        break;
                    case "name":
                        user.name = to;
                        Console.WriteLine("Name!!!!!!!!!");
                        break;
                    case "surname":
                        user.surname = to;
                        break;
                    case "password":
                        user.password = to;
                        break;
                    default:
                        return new SuccessMessageResponseMessage(false,
                            $"'{what}' is not valid property",
                            HttpStatusCode.BadRequest);
                }

                Console.WriteLine(user);
                context.Update(user);
                context.SaveChanges();

                return new SuccessMessageResponseMessage(true);
            }
            catch (FormatException e)
            {
                return new SuccessMessageResponseMessage(false,
                                                             e.Message,
                                                             HttpStatusCode.BadRequest);
                throw;
            }
        }

        [HttpGet]
        [Route("getUsers/")]
        [EnableCors]
        [Produces("application/json")]
        public List<PublicUser> GetPublicUsers()
        {
            using (var context = new flashcardsContext())
                return context.users.Cast<PublicUser>().ToList();
        }

        [HttpGet]
        [Route("getuser/")]
        [EnableCors]
        [Produces("application/json")]
        public HttpResponseMessage GetUserPublic(uint id, string username)
        {

            try
            {
                using var context = new flashcardsContext();
                return CreatePublicUserResponseMessage(context.users.First(u => u.Id == id));
            }
            catch (Exception)
            {
                try
                {
                    using var context = new flashcardsContext();
                    return CreatePublicUserResponseMessage(context.users.First(u => u.UserName == username));
                }
                catch (Exception)
                {
                    return new HttpResponseMessage(HttpStatusCode.NoContent);
                }
            }
        }

        [HttpGet]
        [Route("isEmailAlreadyUsed")]
        [EnableCors]
        [Produces("application/json")]
        public HttpResponseMessage IsEmailUsed(string email)
        {
            if (IsValidEmail(email))
            {
                using var context = new flashcardsContext();
                return new IsAleradyUsedResponseMessage { isAlreadyUsed = context.users.Any(u => u.Email == email)};
            }

            return new SuccessMessageResponseMessage(false,
                $"{email} isn't correct email format.",
                HttpStatusCode.BadRequest);
        }

        [HttpGet]
        [Route("isUsernameAlreadyUsed")]
        [EnableCors]
        [Produces("application/json")]
        public HttpResponseMessage IsUsernameUsed(string username)
        {
            // return new IsAleradyUsedResponseMessage() { isAlreadyUsed = !db.IsUserUsernameUnique(username) };
            using var context = new flashcardsContext();
                return new IsAleradyUsedResponseMessage { isAlreadyUsed = context.users.Any(u => u.UserName == username)};
        }

        private PublicUserResponseMessage CreatePublicUserResponseMessage(User.User u)
        {
            return new PublicUserResponseMessage { user = new PublicUser(u.Id, u.UserName) };
        }

        private string GenerateJSONWebToken(User.User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            // var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var myIssuer = "https://localhost:5001";
            var myAudience = "https://localhost:5001";

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                }),
                Expires = DateTime.Now.AddHours(1),
                Issuer = myIssuer,
                Audience = myAudience,
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);


            // var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Issuer"], null, expires: DateTime.Now.AddMinutes(5), signingCredentials: credentials);
            // Console.WriteLine("================");
            // Console.WriteLine(token);
            // Console.WriteLine("================");
            // return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
        private bool ValidateCurrentToken(string token)
        {
            
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var myIssuer = _configuration["Jwt:Issuer"];
            var myAudience = _configuration["Jwt:Issuer"];

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = myIssuer,
                    ValidAudience = myAudience,
                    IssuerSigningKey = securityKey
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }


        private User.User GetClaim(string token, string claimType)
        {
            if (!ValidateCurrentToken(token))
                throw new NotImplementedException("bad token");
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            
            using var context = new flashcardsContext();
            var userId = Int32.Parse(securityToken.Claims.First(claim => claim.Type == claimType).Value);
            if (securityToken.ValidTo < DateTime.UtcNow)
                throw new NotImplementedException("expired");
            return context.users.First(u => u.Id == userId);
        }

        private int loggedInId()
        {
            return Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }

        static bool IsValidEmail(string email)
        {
            try {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch {
                return false;
            }
        }

    }
}