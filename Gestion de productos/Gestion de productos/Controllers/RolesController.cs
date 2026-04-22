using Gestion_de_productos.DTOs;
using Gestion_de_productos.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gestion_de_productos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRolService _rolService;

        public RolesController(IRolService rolService)
        {
            _rolService = rolService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RolDTO>>> Get()
        {
            var roles = await _rolService.ObtenerTodosAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RolDTO>> Get(int id)
        {
            try
            {
                var rol = await _rolService.ObtenerPorIdAsync(id);
                return Ok(rol);
            }
            catch (Exception ex)
            {
                return NotFound(new ResponseDTO { Success = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<RolDTO>> Post([FromBody] CrearRolDTO dto)
        {
            try
            {
                var rol = await _rolService.CrearAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = rol.Id }, rol);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDTO { Success = false, Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] CrearRolDTO dto)
        {
            try
            {
                await _rolService.ActualizarAsync(id, dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDTO { Success = false, Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _rolService.EliminarAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDTO { Success = false, Message = ex.Message });
            }
        }
    }
}
