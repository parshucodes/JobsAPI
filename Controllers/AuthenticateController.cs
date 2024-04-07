using JobsAPI.Helpers;
using JobsAPI.Models;
using JobsAPI.WebModels.AuthenticationWebModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace JobsAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;
        private readonly CommonHelper _commonHelper;

        public AuthenticateController(DatabaseContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _commonHelper = new CommonHelper(_configuration);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<AuthResponseWebModel>> LoginUser(AuthLoginWebModel userAuth)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(i => i.Username == userAuth.Username);
                if (user == null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound, new AuthResponseWebModel
                    {
                        JwtToken = null,
                        Message = "Username or password is incorrect."
                    });
                }

                string encryptedPassword = _commonHelper.Encrypt(userAuth.Password);

                if (user.Password == encryptedPassword)
                {
                    var issuer = _configuration.GetValue<string>("JWT:Issuer");
                    var audience = _configuration.GetValue<string>("JWT:Audience");
                    var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWT:Key"));

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[]
                        {
                            new Claim("Id", user.UserId.ToString()),
                            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                            new Claim(JwtRegisteredClaimNames.Email, user.Username),
                            new Claim(JwtRegisteredClaimNames.Jti,
                            Guid.NewGuid().ToString())
                         }),
                        Expires = DateTime.UtcNow.AddMinutes(30),
                        Issuer = issuer,
                        Audience = audience,
                        SigningCredentials = new SigningCredentials
                        (new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha512Signature)
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var jwtToken = tokenHandler.WriteToken(token);
                    var stringToken = tokenHandler.WriteToken(token);

                    return Ok(new AuthResponseWebModel
                    {
                        JwtToken = stringToken,
                        Message = "Authentication Successful."
                    });
                }

                return StatusCode((int)HttpStatusCode.NotFound, new AuthResponseWebModel
                {
                    JwtToken = null,
                    Message = "Username or password is incorrect."
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
