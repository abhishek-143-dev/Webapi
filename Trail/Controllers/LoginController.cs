using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Trail.Data;
using Trail.Response;
using System.Security.Principal;
using Trail.Model;
namespace Trail.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly DBdata _context;
        public  IConfiguration _configuration;

        public LoginController(DBdata context , IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        [HttpPost]
        public ActionResult Login([FromBody] Loging loging)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == loging.Email && x.Password == loging.Password);
            if (user == null)
            {
                return Unauthorized();
            }
            else
            {
                //var claims = new[] {
                //        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                //        new Claim("Id", user.Id.ToString()),
                //        new Claim("UserName", user.Name),
                //        new Claim("Email", user.Email)
                //    };
                //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                //var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                //var token = new JwtSecurityToken(
                //    _configuration["Jwt:Issuer"],
                //    _configuration["Jwt:Audience"],
                //    claims,
                //    expires: DateTime.UtcNow.AddMinutes(10),
                //    signingCredentials: signIn);

                var jwtToken = GenerateToken(user);
                //SessionExtensions.SetString(HttpContext.Session, "access_token", jwtToken);

                Response.Cookies.Append("access_token", jwtToken, new Microsoft.AspNetCore.Http.CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddMinutes(10)
                });
                return Ok(new { token = jwtToken });
            }
           
        }
        [HttpGet]
        [Route("/validate")]
        public ActionResult Check() {

            var token = Request.Cookies["access_token"];
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }
            if (!ValidateToken(token))
            {
                Response.Cookies.Delete("access_token");
                return Unauthorized();
            }
            return Ok("Your are Logedin");
        }
        [HttpGet]
        [Route("/logout")]
        public ActionResult Logout()
        {
            Response.Cookies.Delete("access_token");
            return Ok("Logout");
        }
        private static string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Yh2k7QSu4l8CZg5p6X3Pna9L0Miy4D3Bvt0JVr87UcOj69Kqw5R2Nmf4FWs03Hdx"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var secToken = new JwtSecurityToken(
                signingCredentials: credentials,
                issuer: "JWTAuthenticationServer",
                audience: "JWTServicePostmanClient",
                 claims : new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, "JWTServiceAccessToken"),
                        new Claim("Id", user.Id.ToString()),
                        new Claim("UserName", user.Name),
                        new Claim("Email", user.Email)
                    },
        expires: DateTime.UtcNow.AddMinutes(10));

            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(secToken);
        }

        private static bool ValidateToken(string authToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();

            SecurityToken validatedToken;
            IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
            return true;
        }

        private static TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                RequireExpirationTime = true,  // Because there is no issuer in the generated token
                ValidIssuer = "JWTAuthenticationServer",
                ValidAudience = "JWTServicePostmanClient",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Yh2k7QSu4l8CZg5p6X3Pna9L0Miy4D3Bvt0JVr87UcOj69Kqw5R2Nmf4FWs03Hdx")) ,// The same key as the one that generate the token
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ClockSkew = System.TimeSpan.Zero,
            };
        }

    }
}
