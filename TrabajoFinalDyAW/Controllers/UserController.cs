using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TrabajoFinalDyAW.DTOs;
using TrabajoFinalDyAW.Models;
using TrabajoFinalDyAW.Presenters;
using TrabajoFinalDyAW.Responses;
using TrabajoFinalDyAW.Utils;

namespace TrabajoFinalDyAW.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TrabajoFinalContext _context;
        private readonly IMapper _mapper;

        public UserController(TrabajoFinalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtener usuarios del sistema
        /// </summary>
        /// <remarks>Obtener todos los usuarios creados en el sistema. Requiere el claim de GET_USER</remarks>
        /// <response code="200">Listado de usuarios</response>
        /// <response code="401">No autorizado</response>
        /// <response code="403">No tenes permisos para realizar esta accion</response>
        /// <response code="500">Error interno del servidor</response>
        [Authorize(Policy = "GET_USER")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserPresenter>), 200)]
        public async Task<ActionResult<IEnumerable<UserPresenter>>> GetAllUsers()
        {
            var users = await (from user in _context.User.Include(u => u.Userpermisssionclaim)
                                select user).ToListAsync();

            return Ok(
                users.Select(
                    u => _mapper.Map<UserPresenter>(
                        _mapper.Map<Entities.User>(u)
                    )
                )
            );
        }

        /// <summary>
        /// Obtener un usuario por su ID
        /// </summary>
        /// <remarks>Obtener un usuario por su Id. Requiere el claim de GET_USER</remarks>
        /// <response code="200">Usuario</response>
        /// <response code="400">ID no valida</response>
        /// <response code="401">No autorizado</response>
        /// <response code="404">No se encontro el usuario</response>
        /// <response code="403">No tenes permisos para realizar esta accion</response>
        /// <response code="500">Error interno del servidor</response>
        [Authorize(Policy = "GET_USER")]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserPresenter), 200)]
        [ProducesResponseType(typeof(BadRequestResponse), 400)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        public async Task<ActionResult<UserPresenter>> GetUserById(string id)
        {
            if (!Guid.TryParse(id, out var userId))
            {
                return BadRequest(new BadRequestResponse {
                    error = "El ID no es un GUID"
                });
            }

            var users = await (from user in _context.User.Include(u => u.Userpermisssionclaim)
                               where user.UserId == userId
                               select user).ToListAsync();

            if (users.Count == 0) return NotFound();

            return Ok(
                _mapper.Map<UserPresenter>(
                    _mapper.Map<Entities.User>(users[0])
                )
            );
        }

        /// <summary>
        /// Crear un usuario
        /// </summary>
        /// <remarks>Crear un usuario. Requiere el claim de CREATE_USER</remarks>
        /// <response code="201">Usuario</response>
        /// <response code="400">ID no valida</response>
        /// <response code="401">No autorizado</response>
        /// <response code="403">No tenes permisos para realizar esta accion</response>
        /// <response code="409">Un usuario con esos datos ya existe</response>
        /// <response code="500">Error interno del servidor</response>
        [Authorize(Policy = "CREATE_USER")]
        [HttpPost]
        [ProducesResponseType(typeof(Created<UserPresenter>), 201)]
        [ProducesResponseType(typeof(BadRequestResponse), 400)]
        [ProducesResponseType(typeof(ConflictResult), 409)]
        public async Task<ActionResult<UserPresenter>> CreateUser(CreateUserDto body)
        {
            var existingUser = await (from u in _context.User
                                      where u.UserUsername == body.Username
                                      select u).ToListAsync();

            if (existingUser.Count != 0) return Conflict();

            var user = _mapper.Map<Entities.User>(body);
            user.Id = Guid.NewGuid();
            user.Password = HashUtil.GenerateSHA256Hash(user.Password);
            var model = _mapper.Map<Models.User>(user);

            _context.Add(model);

            await _context.SaveChangesAsync();

            return Created(
                $"/api/User/{user.Id}",
                _mapper.Map<UserPresenter>(
                    _mapper.Map<Entities.User>(model)
                )
            );
        }

        /// <summary>
        /// Actualizar un usuario
        /// </summary>
        /// <remarks>Actualizar un usuario. Requiere el claim de UPDATE_USER</remarks>
        /// <response code="200">Usuario</response>
        /// <response code="400">Datos de la solicitud no validos</response>
        /// <response code="401">No autorizado</response>
        /// <response code="403">No tenes permisos para realizar esta accion</response>
        /// <response code="404">No se encontro el usuario</response>
        /// <response code="500">Error interno del servidor</response>
        [Authorize(Policy = "UPDATE_USER")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Created<UserPresenter>), 201)]
        [ProducesResponseType(typeof(BadRequestResponse), 400)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        public async Task<ActionResult<UserPresenter>> UpdateUser([FromRoute] string id, [FromBody] UpdateUserDto body)
        {
            if (!Guid.TryParse(id, out var userId)) return BadRequest("User id is not a Guid");

            var existingUser = await (from u in _context.User.Include(u => u.Userpermisssionclaim)
                                      where u.UserId == Guid.Parse(id)
                                      select u).ToListAsync();

            if (existingUser.Count <= 0) return NotFound();

            if (body.Username != null)
            {
                existingUser[0].UserUsername = body.Username;
            }

            if (body.Password != null)
            {
                existingUser[0].UserPassword = HashUtil.GenerateSHA256Hash(body.Password);
            }

            if (body.Permissions != null)
            {
                var bodyPermissions = body.Permissions.ToList();
                var userPermissions = existingUser[0].Userpermisssionclaim;
                foreach (var permission in userPermissions)
                {
                    var p = bodyPermissions.Find(p => p == permission.PermissionclaimName);
                    if (p == null)
                    {
                        _context.Userpermisssionclaim.Remove(permission);
                    } else
                    {
                        bodyPermissions.Remove(p);
                    }
                }

                foreach (var permission in bodyPermissions)
                {
                    existingUser[0].Userpermisssionclaim.Add(new Userpermisssionclaim { UserpermissionclaimId = Guid.NewGuid(), PermissionclaimName = permission, UserId = existingUser[0].UserId });
                }
            }

            await _context.SaveChangesAsync();

            return Ok(
                _mapper.Map<UserPresenter>(
                    _mapper.Map<Entities.User>(existingUser[0])
                )
            );
        }

        /// <summary>
        /// Eliminar un usuario
        /// </summary>
        /// <remarks>Eliminar un usuario. Requiere el claim de DELETE_USER</remarks>
        /// <response code="200">Vacia</response>
        /// <response code="400">Datos de la solicitud no validos</response>
        /// <response code="401">No autorizado</response>
        /// <response code="403">No tenes permisos para realizar esta accion</response>
        /// <response code="404">No se encontro el usuario</response>
        /// <response code="500">Error interno del servidor</response>
        [Authorize(Policy = "DELETE_USER")]
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(BadRequestResponse), 400)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        public async Task<ActionResult> DeleteUser([FromRoute] string id)
        {
            if (!Guid.TryParse(id, out var userId)) return BadRequest("User id is not a Guid");

            var existingUser = await (from u in _context.User.Include(u => u.Userpermisssionclaim)
                                      where u.UserId == Guid.Parse(id)
                                      select u).ToListAsync();

            if (existingUser.Count <= 0) return NotFound();

            _context.User.Remove(existingUser[0]);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
