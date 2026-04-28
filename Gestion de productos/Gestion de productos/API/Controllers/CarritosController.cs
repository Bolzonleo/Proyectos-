using Gestion_de_productos.DTOs;
using Gestion_de_productos.Services.Interfaces;
using Gestion_de_productos.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Gestion_de_productos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarritosController : ControllerBase
    {
        private readonly ICarritoService _carritoService;

        public CarritosController(ICarritoService carritoService)
        {
            _carritoService = carritoService;
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<CarritoDTO>> GetPorUsuario(int usuarioId)
        {
            try { return Ok(await _carritoService.ObtenerPorUsuarioIdAsync(usuarioId)); }
            catch (Exception ex) { return BadRequest(new ResponseDTO { Success = false, Message = ex.Message }); }
        }

        [HttpPost("usuario/{usuarioId}")]
        public async Task<ActionResult<CarritoDTO>> CrearParaUsuario(int usuarioId)
        {
            try { return Ok(await _carritoService.CrearParaUsuarioAsync(usuarioId)); }
            catch (Exception ex) { return BadRequest(new ResponseDTO { Success = false, Message = ex.Message }); }
        }

        [HttpPost("usuario/{usuarioId}/items")]
        public async Task<ActionResult<CarritoDTO>> AgregarItem(int usuarioId, AgregarAlCarritoDTO dto)
        {
            try { return Ok(await _carritoService.AgregarItemAsync(usuarioId, dto)); }
            catch (Exception ex) { return BadRequest(new ResponseDTO { Success = false, Message = ex.Message }); }
        }

        [HttpPut("usuario/{usuarioId}/items/{productoId}")]
        public async Task<ActionResult<CarritoDTO>> ActualizarCantidad(int usuarioId, int productoId, [FromQuery] int cantidad)
        {
            try { return Ok(await _carritoService.ActualizarCantidadAsync(usuarioId, productoId, cantidad)); }
            catch (Exception ex) { return BadRequest(new ResponseDTO { Success = false, Message = ex.Message }); }
        }

        [HttpDelete("usuario/{usuarioId}/items/{productoId}")]
        public async Task<ActionResult> QuitarItem(int usuarioId, int productoId)
        {
            try { await _carritoService.QuitarItemAsync(usuarioId, productoId); return NoContent(); }
            catch (Exception ex) { return BadRequest(new ResponseDTO { Success = false, Message = ex.Message }); }
        }

        [HttpDelete("usuario/{usuarioId}/items")]
        public async Task<ActionResult> Vaciar(int usuarioId)
        {
            try { await _carritoService.VaciarAsync(usuarioId); return NoContent(); }
            catch (Exception ex) { return BadRequest(new ResponseDTO { Success = false, Message = ex.Message }); }
        }
    }
}
