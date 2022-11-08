using HeroesAPIApp.DTOs;
using HeroesAPIApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SuperHeroWebAPI.Tools;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HeroesAPIApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        public static User curr_User = new User();
        public string status = "99";
        public static string logmsg = "";
        public readonly IConfiguration _config;
        public static string token_now = "";


        public AuthController(IConfiguration configuration)
        {
            this._config = configuration;
        }

         

        [HttpPost("register")]
        public async Task<ActionResult<User>> RegisterUser(UserDto request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            curr_User.Username = request.UserName;
            curr_User.PasswordHash = passwordHash;
            curr_User.PasswordSalt = passwordSalt;
            status = "01";
            logmsg += "\n Logged as:" + curr_User.Username + " with passHash : " + passwordHash.ToString() +" register : "+status;
            return Ok(curr_User);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginUser(UserDto request)
        {
            if (curr_User.Username != request.UserName)
            {
                status = "00";   
                logmsg +=" Credentials for this user was not found " + request.UserName + "Login : "+status;
                Log.WriteLine(logmsg);
                return BadRequest("User was not found");
            }

            if (!VerifyPasswordHash(request.Password , curr_User.PasswordHash , curr_User.PasswordSalt))
            {
                //do something
                status = "04";
                logmsg += " Password missmatch " + request.UserName + "Login : " + status;
                Log.WriteLine(logmsg);
                return BadRequest("Login password typed doesn't match");
            }


            string token = CreateToken(curr_User);
            token_now = token;
            status = "02";
            logmsg += " Success , Login :" + status;
            Log.WriteLine(logmsg);
            return Ok(token);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name , user.Username),
                new Claim(ClaimTypes.Role , "Visitor")
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var cred = new SigningCredentials(key , SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        private void CreatePasswordHash(string password , out byte[] passwordHash , out byte[] passwordSalt )
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }


        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            //decrypt the password 
            //test
            using (var hmac = new HMACSHA512(passwordSalt))
            {

                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }

        }


        [HttpGet("Token"), Authorize]
        public string GetToken()
        {
            return "bearer " + token_now;
        }


    }

 
}
