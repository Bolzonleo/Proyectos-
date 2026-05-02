using Gestion_de_productos.DTOs;
using Gestion_de_productos.Services.Interfaces;
using Gestion_de_productos.Shared.DTOs;
using Gestion_de_productos.Shared.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Gestion_de_productos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidosController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PedidoDTO>>> Get() => Ok(await _pedidoService.ObtenerTodosAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoDTO>> Get(int id)
        {
            try { return Ok(await _pedidoService.ObtenerPorIdAsync(id)); }
            catch (Exception ex) { return NotFound(new ResponseDTO { Success = false, Message = ex.Message }); }
        }

        [HttpPost]
        public async Task<ActionResult<PedidoDTO>> Post(CrearPedidoDTO dto)
        {
            try
            {
                var pedido = await _pedidoService.CrearAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = pedido.Id }, pedido);
            }
            catch (Exception ex) { return BadRequest(new ResponseDTO { Success = false, Message = ex.Message }); }
        }

        [HttpPost("desde-carrito/{usuarioId}")]
        public async Task<ActionResult<PedidoDTO>> PostDesdeCarrito(int usuarioId)
        {
            try
            {
                var pedido = await _pedidoService.CrearDesdeCarritoAsync(usuarioId);
                return CreatedAtAction(nameof(Get), new { id = pedido.Id }, pedido);
            }
            catch (Exception ex) { return BadRequest(new ResponseDTO { Success = false, Message = ex.Message }); }
        }

        [HttpPatch("{id}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromQuery] EstadoPedido estado)
        {
            try
            {
                await _pedidoService.CambiarEstadoAsync(id, estado);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try { await _pedidoService.EliminarAsync(id); return NoContent(); }
            catch (Exception ex) { return BadRequest(new ResponseDTO { Success = false, Message = ex.Message }); }
        }
    }
}
