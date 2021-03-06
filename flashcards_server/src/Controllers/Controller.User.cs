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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [AllowAnonymous]
        [Consumes("application/json")]
        [Produces("application/json")]
        public IActionResult Register(UserToRegister user)
        {
            try
            {
                var u = new User.User(user.username, user.email, user.name, user.surname);
                using var context = new flashcardsContext();
                if (!IsValidEmail(u.Email))
                    throw new FormatException("Email format is not valid.");
                if (context.users.Any(us => us.UserName == u.UserName | us.Email == u.Email))
                    if (context.users.Any(us => us.UserName == u.UserName))
                        throw new FormatException("Username is already used");
                    else throw new FormatException("Email is already used");
                u.ChangePassword(user.password);
                context.users.Add(u);
                context.SaveChanges();
                return Ok(u);
            }
            catch (FormatException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("login/")]
        [AllowAnonymous]
        [EnableCors]
        [Consumes("application/json")]
        public IActionResult Login(LoginData loginData)
        {
            using var context = new flashcardsContext();
            IActionResult response = Unauthorized();
            try
            {
                var user = context.users.First(u => u.UserName == loginData.login);
                if (!user.ValidatePassword(loginData.password))
                    return response;
                var token = GenerateJsonWebToken(user);
                response = Ok(new { token });
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
        [Consumes("application/json")]
        [Produces("application/json")]
        public IActionResult UpdateUserData(UpdateRequest updateRequest)
        {
            if (updateRequest.id != LoggedInId())
                return Unauthorized("Access denied");

            using var context = new flashcardsContext();
            var user = context.users.First(u => u.Id == updateRequest.id);

            var to = updateRequest.to;
            var what = updateRequest.what;
            switch (what.ToLower())
            {
                case "email":
                    if (!IsValidEmail(to))
                        return BadRequest($"'{to}' isn't correct email format.");
                    user.Email = to;
                    break;
                case "name":
                    user.name = to;
                    break;
                case "surname":
                    user.surname = to;
                    break;
                case "password":
                    user.ChangePassword(to);
                    break;
                default:
                    return BadRequest($"'{what}' is not valid property");
            }

            context.Update(user);
            context.SaveChanges();

            return Ok("Successfully changed");

        }

        [HttpGet]
        [Route("getUsers/")]
        [EnableCors]
        [Produces("application/json")]
        public IEnumerable<PublicUser> GetPublicUsers()
        {
            using var context = new flashcardsContext();
            return context.users.Cast<PublicUser>().ToList();
        }

        [HttpGet]
        [Route("getMe/")]
        [EnableCors]
        [Produces("application/json")]
        public User.User GetMe()
        {
            using var context = new flashcardsContext();
            return context.users.First(u => u.Id == LoggedInId());
        }

        [HttpGet]
        [Route("getuser/")]
        [EnableCors]
        [Produces("application/json")]
        public PublicUser GetUserPublic(uint id, string username)
        {
            try
            {
                using var context = new flashcardsContext();
                return new PublicUser(context.users.First(u => u.Id == id));
            }
            catch (Exception)
            {
                try
                {
                    using var context = new flashcardsContext();
                    return new PublicUser(context.users.First(u => u.UserName == username));
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        [HttpGet]
        [Route("isEmailAlreadyUsed")]
        [AllowAnonymous]
        [EnableCors]
        [Produces("application/json")]
        public IActionResult IsEmailUsed(string email)
        {
            if (!IsValidEmail(email)) return BadRequest($"'{email}' isn't correct email format.");
            using var context = new flashcardsContext();
            return Ok(context.users.Any(u => u.Email == email));

        }

        [HttpGet]
        [Route("isUsernameAlreadyUsed")]
        [AllowAnonymous]
        [EnableCors]
        [Produces("application/json")]
        public IActionResult IsUsernameUsed(string username)
        {
            using var context = new flashcardsContext();
            return Ok(context.users.Any(u => u.UserName == username));
        }

        private PublicUserResponseMessage CreatePublicUserResponseMessage(User.User u)
        {
            return new PublicUserResponseMessage { user = new PublicUser(u.Id, u.UserName) };
        }

        private string GenerateJsonWebToken(User.User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            // var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var myIssuer = _configuration["Jwt:Issuer"];
            var myAudience = _configuration["Jwt:Issuer"];

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
                }, out var validatedToken);
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
            if (securityToken == null) throw new NotImplementedException("token is null");
            var userId = int.Parse(securityToken.Claims.First(claim => claim.Type == claimType).Value);
            if (securityToken.ValidTo < DateTime.UtcNow)
                throw new NotImplementedException("expired");
            return context.users.First(u => u.Id == userId);

        }

        private int LoggedInId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                             throw new InvalidOperationException(
                                 $"Cannot validate - there's no user with id {ClaimTypes.NameIdentifier}"));
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

    }
}