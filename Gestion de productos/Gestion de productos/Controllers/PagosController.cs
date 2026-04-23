using Gestion_de_productos.DTOs;
using Gestion_de_productos.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gestion_de_productos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagosController : ControllerBase
    {
        private readonly IPagoService _pagoService;

        public PagosController(IPagoService pagoService)
        {
            _pagoService = pagoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PagoDTO>>> Get() => Ok(await _pagoService.ObtenerTodosAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<PagoDTO>> Get(int id)
        {
            try { return Ok(await _pagoService.ObtenerPorIdAsync(id)); }
            catch (Exception ex) { return NotFound(new ResponseDTO { Success = false, Message = ex.Message }); }
        }

        [HttpPost]
        public async Task<ActionResult<PagoDTO>> Post(CrearPagoDTO dto)
        {
            try { return Ok(await _pagoService.CrearAsync(dto)); }
            catch (Exception ex) { return BadRequest(new ResponseDTO { Success = false, Message = ex.Message }); }
        }

        [HttpPatch("{id}/estado")]
        public async Task<ActionResult> PatchEstado(int id, ActualizarPagoDTO dto)
        {
            try { await _pagoService.ActualizarEstadoAsync(id, dto); return NoContent(); }
            catch (Exception ex) { return BadRequest(new ResponseDTO { Success = false, Message = ex.Message }); }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try { await _pagoService.EliminarAsync(id); return NoContent(); }
            catch (Exception ex) { return BadRequest(new ResponseDTO { Success = false, Message = ex.Message }); }
        }
    }
}
