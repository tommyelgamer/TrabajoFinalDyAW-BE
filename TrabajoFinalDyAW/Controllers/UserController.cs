﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TrabajoFinalDyAW.DTOs;
using TrabajoFinalDyAW.Models;
using TrabajoFinalDyAW.Presenters;
using TrabajoFinalDyAW.Responses;

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
        /// <remarks>Obtener todos los usuarios creados en el sistema. Requiere el claim de GET_USER </remarks>
        /// <response code="200">Listado de usuarios</response>
        /// <response code="401">No autorizado</response>
        /// <response code="403">No tenes permisos para realizar esta accion</response>
        /// <response code="500">Error interno del servidor</response>
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
        /// <remarks>Obtener un usuario por su I. Requiere el claim de GET_USER</remarks>
        /// <response code="200">Usuario</response>
        /// <response code="400">ID no valida</response>
        /// <response code="401">No autorizado</response>
        /// <response code="404">No se encontro el usuario</response>
        /// <response code="403">No tenes permisos para realizar esta accion</response>
        /// <response code="500">Error interno del servidor</response>
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
        /// <response code="404">No se encontro el usuario</response>
        /// <response code="409">Un usuario con esos datos ya existe</response>
        /// <response code="500">Error interno del servidor</response>
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
    }
}
