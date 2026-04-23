using Gestion_de_productos.DTOs;
using Gestion_de_productos.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gestion_de_productos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnviosController : ControllerBase
    {
        private readonly IEnvioService _envioService;

        public EnviosController(IEnvioService envioService)
        {
            _envioService = envioService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnvioDTO>>> Get() => Ok(await _envioService.ObtenerTodosAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<EnvioDTO>> Get(int id)
        {
            try { return Ok(await _envioService.ObtenerPorIdAsync(id)); }
            catch (Exception ex) { return NotFound(new ResponseDTO { Success = false, Message = ex.Message }); }
        }

        [HttpPost]
        public async Task<ActionResult<EnvioDTO>> Post(CrearEnvioDTO dto)
        {
            try { return Ok(await _envioService.CrearAsync(dto)); }
            catch (Exception ex) { return BadRequest(new ResponseDTO { Success = false, Message = ex.Message }); }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, ActualizarEnvioDTO dto)
        {
            try { await _envioService.ActualizarAsync(id, dto); return NoContent(); }
            catch (Exception ex) { return BadRequest(new ResponseDTO { Success = false, Message = ex.Message }); }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try { await _envioService.EliminarAsync(id); return NoContent(); }
            catch (Exception ex) { return BadRequest(new ResponseDTO { Success = false, Message = ex.Message }); }
        }
    }
}
