using Gestion_de_productos.Services.Interfaces;
using Gestion_de_productos.Shared.DTOs;
using Gestion_de_productos.Shared.Entities;

namespace Gestion_de_productos.Services
{
    public class CarritoService : ICarritoService
    {
        private readonly ICarritoRepository _carritoRepo;
        private readonly ICarritoItemRepository _itemRepo;
        private readonly IProductoRepository _productoRepo;
        private readonly IUsuarioRepository _usuarioRepo;

        public CarritoService(
            ICarritoRepository carritoRepo,
            ICarritoItemRepository itemRepo,
            IProductoRepository productoRepo,
            IUsuarioRepository usuarioRepo)
        {
            _carritoRepo = carritoRepo;
            _itemRepo = itemRepo;
            _productoRepo = productoRepo;
            _usuarioRepo = usuarioRepo;
        }

        // ==========================
        // GET CARRITO
        // ==========================
        public async Task<CarritoDTO> ObtenerPorUsuarioIdAsync(int usuarioId)
        {
            var carrito = await _carritoRepo.ObtenerConItemsAsync(usuarioId);

            if (carrito == null)
            {
                carrito = await CrearInterno(usuarioId);
            }

            return Mapear(carrito);
        }

        // ==========================
        // CREAR
        // ==========================
        public async Task<CarritoDTO> CrearParaUsuarioAsync(int usuarioId)
        {
            var carrito = await CrearInterno(usuarioId);
            return Mapear(carrito);
        }

        private async Task<Carrito> CrearInterno(int usuarioId)
        {
            var usuarioExiste = await _usuarioRepo.ExisteAsync(usuarioId);
            if (!usuarioExiste)
                throw new Exception("El usuario no existe");

            var existente = await _carritoRepo.ObtenerPorUsuarioIdAsync(usuarioId);
            if (existente != null)
                return existente;

            var carrito = new Carrito { UsuarioId = usuarioId };

            await _carritoRepo.CrearAsync(carrito);

            return carrito;
        }

        // ==========================
        // AGREGAR ITEM
        // ==========================
        public async Task<CarritoDTO> AgregarItemAsync(int usuarioId, AgregarAlCarritoDTO dto)
        {
            if (dto.Cantidad <= 0)
                throw new Exception("La cantidad debe ser mayor a 0");

            var producto = await _productoRepo.ObtenerPorIdAsync(dto.ProductoId);
            if (producto == null)
                throw new Exception("El producto no existe");

            var carrito = await _carritoRepo.ObtenerConItemsAsync(usuarioId)
                         ?? await CrearInterno(usuarioId);

            var item = carrito.Items.FirstOrDefault(i => i.ProductoId == dto.ProductoId);

            if (item == null)
            {
                item = new CarritoItem
                {
                    CarritoId = carrito.Id,
                    ProductoId = dto.ProductoId,
                    Cantidad = dto.Cantidad
                };

                await _itemRepo.AgregarAsync(item);
            }
            else
            {
                item.Cantidad += dto.Cantidad;
                _itemRepo.Actualizar(item);
            }

            await _carritoRepo.GuardarCambiosAsync();

            return await ObtenerPorUsuarioIdAsync(usuarioId);
        }

        // ==========================
        // ACTUALIZAR CANTIDAD
        // ==========================
        public async Task<CarritoDTO> ActualizarCantidadAsync(int usuarioId, int productoId, int cantidad)
        {
            if (cantidad <= 0)
                throw new Exception("La cantidad debe ser mayor a 0");

            var carrito = await _carritoRepo.ObtenerConItemsAsync(usuarioId);

            if (carrito == null)
                throw new Exception("No existe carrito para el usuario");

            var item = carrito.Items.FirstOrDefault(i => i.ProductoId == productoId);

            if (item == null)
                throw new Exception("El producto no existe en el carrito");

            item.Cantidad = cantidad;

            _itemRepo.Actualizar(item);

            await _carritoRepo.GuardarCambiosAsync();

            return await ObtenerPorUsuarioIdAsync(usuarioId);
        }

        // ==========================
        // QUITAR ITEM
        // ==========================
        public async Task<bool> QuitarItemAsync(int usuarioId, int productoId)
        {
            var carrito = await _carritoRepo.ObtenerConItemsAsync(usuarioId);

            if (carrito == null)
                throw new Exception("No existe carrito para el usuario");

            var item = carrito.Items.FirstOrDefault(i => i.ProductoId == productoId);

            if (item == null)
                throw new Exception("El producto no existe en el carrito");

            _itemRepo.Eliminar(item);

            await _carritoRepo.GuardarCambiosAsync();

            return true;
        }

        // ==========================
        // VACIAR
        // ==========================
        public async Task<bool> VaciarAsync(int usuarioId)
        {
            var carrito = await _carritoRepo.ObtenerConItemsAsync(usuarioId);

            if (carrito == null)
                throw new Exception("No existe carrito para el usuario");

            _itemRepo.EliminarRango(carrito.Items);

            await _carritoRepo.GuardarCambiosAsync();

            return true;
        }

        // ==========================
        // MAPPER
        // ==========================
        private CarritoDTO Mapear(Carrito carrito)
        {
            return new CarritoDTO
            {
                Id = carrito.Id,
                UsuarioId = carrito.UsuarioId,
                Items = carrito.Items.Select(i => new CarritoItemDTO
                {
                    Id = i.Id,
                    ProductoId = i.ProductoId,
                    Cantidad = i.Cantidad,
                    ProductoNombre = i.Producto?.Nombre ?? string.Empty,
                    PrecioUnitario = i.Producto?.Precio ?? 0
                }).ToList()
            };
        }
    }
}