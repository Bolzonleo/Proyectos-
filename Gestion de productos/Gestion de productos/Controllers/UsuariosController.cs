using Gestion_de_productos.DTOs;
using Gestion_de_productos.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gestion_de_productos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDTO>>> Get() => Ok(await _usuarioService.ObtenerTodosAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDTO>> Get(int id)
        {
            try { return Ok(await _usuarioService.ObtenerPorIdAsync(id)); }
            catch (Exception ex) { return NotFound(new ResponseDTO { Success = false, Message = ex.Message }); }
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioDTO>> Post(CrearUsuarioDTO dto)
        {
            try
            {
                var usuario = await _usuarioService.CrearAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = usuario.Id }, usuario);
            }
            catch (Exception ex) { return BadRequest(new ResponseDTO { Success = false, Message = ex.Message }); }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, ActualizarUsuarioDTO dto)
        {
            try { await _usuarioService.ActualizarAsync(id, dto); return NoContent(); }
            catch (Exception ex) { return BadRequest(new ResponseDTO { Success = false, Message = ex.Message }); }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try { await _usuarioService.EliminarAsync(id); return NoContent(); }
            catch (Exception ex) { return BadRequest(new ResponseDTO { Success = false, Message = ex.Message }); }
        }
    }
}
