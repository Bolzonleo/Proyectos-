using Gestion_de_productos.DTOs;
using Gestion_de_productos.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gestion_de_productos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagenesProductoController : ControllerBase
    {
        private readonly IImagenProductoService _imagenProductoService;

        public ImagenesProductoController(IImagenProductoService imagenProductoService)
        {
            _imagenProductoService = imagenProductoService;
        }

        [HttpGet("producto/{productoId}")]
        public async Task<ActionResult<IEnumerable<ImagenProductoDTO>>> GetPorProducto(int productoId)
            => Ok(await _imagenProductoService.ObtenerPorProductoIdAsync(productoId));

        [HttpGet("{id}")]
        public async Task<ActionResult<ImagenProductoDTO>> Get(int id)
        {
            try { return Ok(await _imagenProductoService.ObtenerPorIdAsync(id)); }
            catch (Exception ex) { return NotFound(new ResponseDTO { Success = false, Message = ex.Message }); }
        }

        [HttpPost]
        public async Task<ActionResult<ImagenProductoDTO>> Post(CrearImagenProductoDTO dto)
        {
            try { return Ok(await _imagenProductoService.CrearAsync(dto)); }
            catch (Exception ex) { return BadRequest(new ResponseDTO { Success = false, Message = ex.Message }); }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try { await _imagenProductoService.EliminarAsync(id); return NoContent(); }
            catch (Exception ex) { return BadRequest(new ResponseDTO { Success = false, Message = ex.Message }); }
        }
    }
}
