using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RecipeNest_Core.Dtos.Login;
using RecipeNest_Core.Models.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RecipeNest_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {


        private readonly RecipeNestDbContext _context;

        public AuthenticationController(RecipeNestDbContext context)
        {
            _context = context;
        }

        [NonAction]
        public string GetRandomString(int length = 20)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            byte[] data = new byte[length];

            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(data);
            }

            StringBuilder result = new StringBuilder(length);
            foreach (byte b in data)
            {
                result.Append(chars[b % chars.Length]);
            }

            return result.ToString();
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> LoginToGetApiKey(CreateLoginDTO dto)
        {
            try
            {
                var user = await _context.Logins.FirstOrDefaultAsync(x => x.Username == dto.Username && x.Password == dto.Password);
                if (user == null)
                {
                    return Unauthorized("Username or Password is incorrect");
                }

                // Generate a unique API key
                string apikey = GetRandomString();
                user.ApiKey = apikey;

                _context.Update(user);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Login successful", apikey });
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }



        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> GenerateJWTToken(LoginRequestDTO dto)
        {
            var user = await _context.Logins.FirstOrDefaultAsync
                  (x => x.Username == dto.Username && x.Password == dto.Password);
            if (user == null)
            {
                return Unauthorized("Username or Password is uncorrect");
            }
            else
            {
                var profile = await _context.Users.FirstOrDefaultAsync
                    (x => x.Email == dto.Username);
                //Generate Token
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.UTF8.GetBytes("RecipeNestAPISecrectJWTJoken2024Web10FoodRecipesAPI");
                var tokenDescriptior = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("Username",profile.UserId.ToString()),
                        new Claim("UserType",profile.IsAdmin.ToString())
                    }),
                    Expires = DateTime.Now.AddHours(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey)
                    , SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptior);
                return Ok(tokenHandler.WriteToken(token));
            }
        }
    }
}
