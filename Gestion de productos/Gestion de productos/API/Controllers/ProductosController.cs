using Gestion_de_productos.DTOs;
using Gestion_de_productos.Services.Interfaces;
using Gestion_de_productos.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Gestion_de_productos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductosController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoDTO>>> Get([FromQuery] int? categoriaId, [FromQuery] string? nombre)
        {
            if (categoriaId.HasValue)
            {
                var productosPorCategoria = await _productoService.ObtenerPorCategoriaAsync(categoriaId.Value);
                return Ok(productosPorCategoria);
            }

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                var productosFiltrados = await _productoService.BuscarPorNombreAsync(nombre);
                return Ok(productosFiltrados);
            }

            var productos = await _productoService.ObtenerTodosAsync();
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDTO>> Get(int id)
        {
            try
            {
                var producto = await _productoService.ObtenerPorIdAsync(id);
                return Ok(producto);
            }
            catch (Exception ex)
            {
                return NotFound(new ResponseDTO { Success = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProductoDTO>> Post([FromBody] CrearProductoDTO dto)
        {
            try
            {
                var nuevoProducto = await _productoService.CrearAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = nuevoProducto.Id }, nuevoProducto);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDTO { Success = false, Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ActualizarProductoDTO dto)
        {
            try
            {
                await _productoService.ActualizarAsync(id, dto);
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
                await _productoService.EliminarAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDTO { Success = false, Message = ex.Message });
            }
        }
    }
}
