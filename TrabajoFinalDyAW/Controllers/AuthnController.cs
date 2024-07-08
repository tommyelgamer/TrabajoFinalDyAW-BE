using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabajoFinalDyAW.DTOs;
using TrabajoFinalDyAW.Presenters;

namespace TrabajoFinalDyAW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthnController : ControllerBase
    {
        private DbContext _context;

        public AuthnController(DbContext context)
        {
            _context = context;
        }

        private bool ValidateUserCreds(string username, string password)
        {


            return false;
        }

        [Route("login/basic")]
        [HttpPost]
        public async Task<IActionResult> AuthenticateBasic([FromBody] BasicCredsDto body)
        {
            var response = Unauthorized();
            return response;
        }
    }
}
