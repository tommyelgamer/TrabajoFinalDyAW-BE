using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TrabajoFinalDyAW.DTOs;
using TrabajoFinalDyAW.Entities;
using TrabajoFinalDyAW.Models;
using TrabajoFinalDyAW.Presenters;
using TrabajoFinalDyAW.Utils;

namespace TrabajoFinalDyAW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthnController : ControllerBase
    {
        private readonly TrabajoFinalContext _context;
        private readonly IConfiguration _config;

        public AuthnController(IConfiguration config, TrabajoFinalContext context)
        {
            _context = context;
            _config = config;
        }

        private async Task<bool> ValidateUserCreds(string username, string password)
        {
            var user = await (from u in _context.User
                              where u.UserUsername == username
                              select u).ToListAsync();

            if (user.Count <= 0) return false;

            if (HashUtil.ValidateSHA256Hash(password, user[0].UserPassword)) return true;

            return false;
        }

        /// <summary>
        /// Obtener token utilizando el usuario y contraseña
        /// </summary>
        /// <remarks>Obtener token utilizando el usuario y contraseña</remarks>
        /// <response code="200">Token</response>
        /// <response code="401">No autorizado</response>
        /// <response code="500">Error interno del servidor</response>
        [Route("login/basic")]
        [HttpPost]
        [ProducesResponseType(typeof(AuthenticatedPresenter), 200)]
        [ProducesResponseType(typeof(UnauthorizedResult), 401)]
        public async Task<IActionResult> AuthenticateBasic([FromBody] BasicCredsDto body)
        {
            if (await ValidateUserCreds(body.Username, body.Password))
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var users = await (from u in _context.User.Include(u => u.Userpermisssionclaim)
                                   where u.UserUsername == body.Username
                                   select u).ToListAsync();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Sid, users[0].UserId.ToString()),
                    new Claim(ClaimTypes.Name, users[0].UserUsername),
                };

                foreach (var permission in users[0].Userpermisssionclaim)
                {
                    claims.Add(new Claim("permission", permission.PermissionclaimName));
                }

                var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
                  _config["Jwt:Issuer"],
                  claims,
                  expires: DateTime.Now.AddMinutes(Double.Parse(_config["Jwt:ExpireMinutes"])),
                  signingCredentials: credentials);

                var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

                var Reftoken = new JwtSecurityToken(_config["Jwt:Issuer"],
                  _config["Jwt:Issuer"],
                  claims,
                  expires: DateTime.Now.AddDays(Double.Parse(_config["Jwt:RefreshTokenValidityInDays"])),
                  signingCredentials: credentials);

                var refresh_token = new JwtSecurityTokenHandler().WriteToken(Reftoken);

                _context.Refreshtoken.Add(new Refreshtoken
                {
                    RefreshtokenId = Guid.NewGuid(),
                    RefreshtokenValue = refresh_token,
                    RefreshtokenExpire = DateTime.Now.AddDays(Double.Parse(_config["Jwt:RefreshTokenValidityInDays"])),
                    UserId = users[0].UserId,
                });

                await _context.SaveChangesAsync();

                return Ok(new AuthenticatedPresenter {
                    JwtToken = token,
                    RefreshToken = refresh_token
                });
            }

            return Unauthorized();
        }

        /// <summary>
        /// Obtener token utilizando el token de refresco
        /// </summary>
        /// <remarks>Obtener token utilizando el token de refresco</remarks>
        /// <response code="200">Token renovado</response>
        /// <response code="401">No autorizado</response>
        /// <response code="500">Error interno del servidor</response>
        [Route("login/refresh")]
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(AuthenticatedPresenter), 200)]
        [ProducesResponseType(typeof(UnauthorizedResult), 401)]
        public async Task<IActionResult> AuthenticateRefresh()
        {
            var refresh_token = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            var userId = JWTUtils.GetUserIdFromToken(refresh_token);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var dbRefreshToken = await (from refresh in _context.Refreshtoken
                                        where refresh.UserId == userId
                                        where refresh.RefreshtokenExpire >= DateTime.Now
                                        select refresh).ToListAsync();

            if (dbRefreshToken.Count <= 0 || dbRefreshToken.Find(r => r.RefreshtokenValue == refresh_token) == null) return Unauthorized();

            var users = await (from u in _context.User.Include(u => u.Userpermisssionclaim)
                                where u.UserId == userId
                               select u).ToListAsync();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, users[0].UserId.ToString()),
                new Claim(ClaimTypes.Name, users[0].UserUsername),
            };

            foreach (var permission in users[0].Userpermisssionclaim)
            {
                claims.Add(new Claim("permission", permission.PermissionclaimName));
            }

            var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(Double.Parse(_config["Jwt:ExpireMinutes"])),
                signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

            return Ok(new AuthenticatedPresenter
            {
                JwtToken = token,
                RefreshToken = refresh_token
            });
        }
    }
}
