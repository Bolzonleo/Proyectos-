using Gestion_de_productos.DTOs;
using Gestion_de_productos.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gestion_de_productos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriasController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
        {
            var categorias = await _categoriaService.ObtenerTodosAsync();
            return Ok(categorias);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            try
            {
                var categoria = await _categoriaService.ObtenerPorIdAsync(id);
                return Ok(categoria);
            }
            catch (Exception ex)
            {
                return NotFound(new ResponseDTO { Success = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> Post([FromBody] CrearCategoriaDTO dto)
        {
            try
            {
                var categoria = await _categoriaService.CrearAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = categoria.Id }, categoria);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDTO { Success = false, Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ActualizarCategoriaDTO dto)
        {
            try
            {
                await _categoriaService.ActualizarAsync(id, dto);
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
                await _categoriaService.EliminarAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDTO { Success = false, Message = ex.Message });
            }
        }
    }
}
