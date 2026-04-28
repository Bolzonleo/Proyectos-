using Gestion_de_productos.DTOs;
using Gestion_de_productos.Services.Interfaces;
using Gestion_de_productos.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Gestion_de_productos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoDetallesController : ControllerBase
    {
        private readonly IPedidoDetalleService _pedidoDetalleService;

        public PedidoDetallesController(IPedidoDetalleService pedidoDetalleService)
        {
            _pedidoDetalleService = pedidoDetalleService;
        }

        [HttpGet("pedido/{pedidoId}")]
        public async Task<ActionResult<IEnumerable<PedidoDetalleDTO>>> GetPorPedido(int pedidoId)
            => Ok(await _pedidoDetalleService.ObtenerPorPedidoIdAsync(pedidoId));

        [HttpPost("pedido/{pedidoId}")]
        public async Task<ActionResult<PedidoDetalleDTO>> Post(int pedidoId, CrearPedidoDetalleDTO dto)
        {
            try
            {
                var detalle = await _pedidoDetalleService.AgregarDetalleAsync(pedidoId, dto);
                return Ok(detalle);
            }
            catch (Exception ex) { return BadRequest(new ResponseDTO { Success = false, Message = ex.Message }); }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try { await _pedidoDetalleService.EliminarDetalleAsync(id); return NoContent(); }
            catch (Exception ex) { return BadRequest(new ResponseDTO { Success = false, Message = ex.Message }); }
        }
    }
}
