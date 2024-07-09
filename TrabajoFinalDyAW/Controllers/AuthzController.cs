using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabajoFinalDyAW.Models;
using TrabajoFinalDyAW.Presenters;

namespace TrabajoFinalDyAW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthzController : ControllerBase
    {
        private readonly TrabajoFinalContext _context;
        private readonly IMapper _mapper;

        public AuthzController(TrabajoFinalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtener permisos disponibles en el sistema
        /// </summary>
        /// <remarks>Obtener permisos disponibles en el sistema</remarks>
        /// <response code="200">Listado de permisos</response>
        /// <response code="401">No autorizado</response>
        /// <response code="500">Error interno del servidor</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserPresenter>), 200)]
        public async Task<ActionResult<IEnumerable<UserPresenter>>> GetAllUsers()
        {
            var permissions = await (from user in _context.Permissionclaim
                               select user).ToListAsync();

            return Ok(
                permissions.Select(p => p.PermissionclaimName)
            );
        }
    }
}
