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
    public class MerchandiseController : ControllerBase
    {
        private readonly TrabajoFinalContext _context;
        private readonly IMapper _mapper;

        public MerchandiseController(TrabajoFinalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtener mercancias del sistema
        /// </summary>
        /// <remarks>Obtener todos los mercancias creados en el sistema. Requiere el claim de GET_MERCHANDISE</remarks>
        /// <response code="200">Listado de mercancias</response>
        /// <response code="401">No autorizado</response>
        /// <response code="403">No tenes permisos para realizar esta accion</response>
        /// <response code="500">Error interno del servidor</response>
        [Authorize(Policy = "GET_MERCHANDISE")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MerchandisePresenter>), 200)]
        public async Task<ActionResult<IEnumerable<MerchandisePresenter>>> GetAllMerchandises()
        {
            var merchandises = await (from merchandise in _context.Merchandise
                                      select merchandise).ToListAsync();

            return Ok(
                merchandises.Select(
                    m => _mapper.Map<MerchandisePresenter>(
                        _mapper.Map<Entities.Merchandise>(m)
                    )
                )
            );
        }

        /// <summary>
        /// Obtener una Mercancia por su ID
        /// </summary>
        /// <remarks>Obtener un Mercancia por su Id. Requiere el claim de GET_MERCHANDISE</remarks>
        /// <response code="200">Listado de mercancias</response>
        /// <response code="400">ID no valido</response>
        /// <response code="401">No autorizado</response>
        /// <response code="403">No tenes permisos para realizar esta accion</response>
        /// <response code="404">Mercancia no encontrada</response>
        /// <response code="500">Error interno del servidor</response>
        [Authorize(Policy = "GET_MERCHANDISE")]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MerchandisePresenter), 200)]
        [ProducesResponseType(typeof(BadRequestResponse), 400)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        public async Task<ActionResult<MerchandisePresenter>> GetMerchandiseById(string id)
        {
            if (!Guid.TryParse(id, out var merchandiseId))
            {
                return BadRequest(new BadRequestResponse
                {
                    error = "ID no es un GUID"
                });
            }

            var merchandise = await (from merchandise in _context.Merchandise
                                      where merchandise.MerchandiseId == merchandiseId
                                      select merchandise).ToListAsync();

            if (merchandise.Count == 0) return NotFound();

            return Ok(
                _mapper.Map<MerchandisePresenter>(
                    _mapper.Map<Entities.Merchandise>(merchandise[0])
                )
            );
        }

        /// <summary>
        /// Crear una mercancia
        /// </summary>
        /// <remarks>Crear una mercancia. Requiere el claim de CREATE_MERCHANDISE</remarks>
        /// <response code="201">Mercancia creada</response>
        /// <response code="400">Mercancia no valida</response>
        /// <response code="401">No autorizado</response>
        /// <response code="403">No tenes permisos para realizar esta accion</response>
        /// <response code="500">Error interno del servidor</response>
        [Authorize(Policy = "CREATE_MERCHANDISE")]
        [HttpPost]
        [ProducesResponseType(typeof(Created<MerchandisePresenter>), 201)]
        [ProducesResponseType(typeof(BadRequestResponse), 400)]
        [ProducesResponseType(typeof(ConflictResult), 409)]
        public async Task<ActionResult<MerchandisePresenter>> CreateMerchandise(CreateMerchandiseDto body)
        {
            var existingMerchandise = await (from m in _context.Merchandise
                                             where m.MerchandiseBarcode == body.MerchandiseBarcode
                                             select m).ToListAsync();

            if (existingMerchandise.Count > 0) return Conflict();

            var merchandise = _mapper.Map<Entities.Merchandise>(body);
            merchandise.MerchandiseId = Guid.NewGuid();
            
            var model = _mapper.Map<Models.Merchandise>(merchandise);
            _context.Merchandise.Add(merchandise);
            _context.Add(model);

            await _context.SaveChangesAsync();

            return Created(
                $"/merchandises/{merchandise.MerchandiseId}",
                _mapper.Map<MerchandisePresenter>(
                    _mapper.Map<Entities.Merchandise>(merchandise)
                )
            );
        }

        /// <summary>
        /// Actualizar stock
        /// </summary>
        /// <remarks>Actualiza stock. Requiere el claim de UPDATE_MERCHANDISE</remarks>
        /// <response code="200">Mercancia actualizada</response>
        /// <response code="400">Mercancia no valida</response>
        /// <response code="401">No autorizado</response>
        /// <response code="403">No tenes permisos para realizar esta accion</response>
        /// <response code="404">Mercancia no encontrada</response>
        /// <response code="500">Error interno del servidor</response>
        [Authorize(Policy = "UPDATE_MERCHANDISE")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CreateUserDto<MerchandisePresenter>), 200)]
        [ProducesResponseType(typeof(BadRequestResponse), 400)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        public async Task<ActionResult<MerchandisePresenter>> UpdateMerchandise(string id, UpdateMerchandiseDto body)
        {
            if (!Guid.TryParse(id, out var merchandiseId))
            {
                return BadRequest(new BadRequestResponse
                {
                    error = "ID no es un GUID"
                });
            }

            var existingMerchandise = await (from m in _context.Merchandise
                                             where m.MerchandiseId == merchandiseId
                                             select m).ToListAsync();

            if (existingMerchandise.Count == 0) return NotFoundResult();

            _mapper.Map(body, existingMerchandise[0]);

            var model = _mapper.Map<Models.Merchandise>(existingMerchandise[0]);

            _context.Merchandise.Update(existingMerchandise[0]);
            _context.Update(model);

            await _context.SaveChangesAsync();

            return Ok(
                _mapper.Map<MerchandisePresenter>(
                    _mapper.Map<Entities.Merchandise>(existingMerchandise[0])
                )
            );
        }

        /// <summary>
        /// Eliminar una mercancia
        /// </summary>
        /// <remarks>Elimina una mercancia. Requiere el claim de DELETE_MERCHANDISE</remarks>
        /// <response code="200">Mercancia eliminada</response>
        /// <response code="400">Mercancia no valida</response>
        /// <response code="401">No autorizado</response>
        /// <response code="403">No tenes permisos para realizar esta accion</response>
        /// <response code="404">Mercancia no encontrada</response>
        /// <response code="500">Error interno del servidor</response>
        [Authorize(Policy = "DELETE_MERCHANDISE")]
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(BadRequestResponse), 400)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        public async Task<ActionResult> DeleteMerchandise(string id)
        {
            if (!Guid.TryParse(id, out var merchandiseId))
            {
                return BadRequest(new BadRequestResponse
                {
                    error = "ID no es un GUID"
                });
            }

            var existingMerchandise = await (from m in _context.Merchandise
                                             where m.MerchandiseId == merchandiseId
                                             select m).ToListAsync();

            if (existingMerchandise.Count == 0) return NotFoundResult();

            _context.Merchandise.Remove(existingMerchandise[0]);
            
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
