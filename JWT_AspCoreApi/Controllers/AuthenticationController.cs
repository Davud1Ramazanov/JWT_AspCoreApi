using JWT_AspCoreApi.Data;
using JWT_AspCoreApi.Model;
using JWT_AspCoreApi.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWT_AspCoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly GadgetstoreDbContext _dbContext;

        public AuthenticationController(GadgetstoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("registration")]
        public IActionResult Registration(User user)
        {
            var item = _dbContext.Users.FirstOrDefault(x => x.Login.Equals(user.Login) && x.Password.Equals(user.Password));

            if (item == null)
            {
                _dbContext.Add(new User { Login = user.Login, Password = user.Password });
                _dbContext.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(User user)
        {
            var item = _dbContext.Users.FirstOrDefault(x => x.Login.Equals(user.Login) && x.Password.Equals(user.Password));
            if (item != null)
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSetting["JWT:Secret"]));
                var signinCredeentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var jtOptions = new JwtSecurityToken(
                    issuer: ConfigurationManager.AppSetting["JWT:ValidIssuer"],
                    audience: ConfigurationManager.AppSetting["JWT:ValidAudience"],
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: signinCredeentials);
                var tokenStr = new JwtSecurityTokenHandler().WriteToken(jtOptions);
                return Ok(new JWTTokenResponse() { Token = tokenStr });
            }
            else { return BadRequest(); }
            return Unauthorized();
        }
    }
}

