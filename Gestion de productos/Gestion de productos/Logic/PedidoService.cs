using Gestion_de_productos.Services.Interfaces;
using Gestion_de_productos.Shared.DTOs;
using Gestion_de_productos.Shared.Entities;
using Gestion_de_productos.Shared.Enums;

namespace Gestion_de_productos.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepo;
        private readonly IPedidoDetalleRepository _detalleRepo;
        private readonly ICarritoRepository _carritoRepo;
        private readonly ICarritoItemRepository _itemRepo;
        private readonly IProductoRepository _productoRepo;

        public PedidoService(
            IPedidoRepository pedidoRepo,
            IPedidoDetalleRepository detalleRepo,
            ICarritoRepository carritoRepo,
            ICarritoItemRepository itemRepo,
            IProductoRepository productoRepo)
        {
            _pedidoRepo = pedidoRepo;
            _detalleRepo = detalleRepo;
            _carritoRepo = carritoRepo;
            _itemRepo = itemRepo;
            _productoRepo = productoRepo;
        }

        // =====================================
        // CHECKOUT
        // =====================================
        public async Task<PedidoDTO> CrearDesdeCarritoAsync(int usuarioId)
        {
            var carrito = await _carritoRepo.ObtenerConItemsAsync(usuarioId);

            if (carrito == null || !carrito.Items.Any())
                throw new Exception("El carrito está vacío");

            // 🔥 Validar stock
            foreach (var item in carrito.Items)
            {
                var producto = await _productoRepo.ObtenerPorIdAsync(item.ProductoId);

                if (producto == null)
                    throw new Exception($"Producto {item.ProductoId} no existe");

                if (producto.Stock < item.Cantidad)
                    throw new Exception($"Stock insuficiente para {producto.Nombre}");
            }

            // 🔥 Crear Pedido
            var pedido = new Pedido
            {
                UsuarioId = usuarioId,
                Fecha = DateTime.Now,
                Estado = EstadoPedido.Pendiente
            };

            await _pedidoRepo.CrearAsync(pedido);

            // 🔥 Crear Detalles
            var detalles = carrito.Items.Select(item =>
            {
                return new PedidoDetalle
                {
                    ProductoId = item.ProductoId,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = (double)item.Producto.Precio,
                    Pedido = pedido
                };
            }).ToList();

            await _detalleRepo.AgregarRangoAsync(detalles);

            // 🔥 Descontar stock
            foreach (var item in carrito.Items)
            {
                var producto = await _productoRepo.ObtenerPorIdAsync(item.ProductoId);
                producto.Stock -= item.Cantidad;
            }

            // 🔥 Vaciar carrito
            _itemRepo.EliminarRango(carrito.Items);

            // 🔥 Guardar TODO
            await _pedidoRepo.GuardarCambiosAsync();

            return Mapear(pedido, detalles);
        }

        // =====================================
        public async Task<PedidoDTO> ObtenerPorIdAsync(int id)
        {
            var pedido = await _pedidoRepo.ObtenerPorIdAsync(id);

            if (pedido == null)
                throw new Exception("Pedido no encontrado");

            return Mapear(pedido, pedido.Detalles.ToList());
        }

        // =====================================
        private PedidoDTO Mapear(Pedido pedido, List<PedidoDetalle> detalles)
        {
            return new PedidoDTO
            {
                Id = pedido.Id,
                UsuarioId = pedido.UsuarioId,
                Fecha = pedido.Fecha,
                Estado = pedido.Estado.ToString(),
                Total = (double)pedido.Total,
                Detalles = detalles.Select(d => new PedidoDetalleDTO
                {
                    ProductoId = d.ProductoId,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = (decimal)d.PrecioUnitario
                }).ToList()
            };
        }
        public async Task CambiarEstadoAsync(int pedidoId, EstadoPedido nuevoEstado)
        {
            var pedido = await _pedidoRepo.ObtenerPorIdAsync(pedidoId);

            if (pedido == null)
                throw new Exception("Pedido no encontrado");

            // 🔥 Validación de flujo (MUY IMPORTANTE)
            if (!EsTransicionValida(pedido.Estado, nuevoEstado))
                throw new Exception($"No se puede cambiar de {pedido.Estado} a {nuevoEstado}");

            pedido.Estado = nuevoEstado;

            await _pedidoRepo.GuardarCambiosAsync();
        }

        private bool EsTransicionValida(EstadoPedido actual, EstadoPedido nuevo)
        {
            return actual switch
            {
                EstadoPedido.Pendiente => nuevo == EstadoPedido.Pagado || nuevo == EstadoPedido.Cancelado,

                EstadoPedido.Pagado => nuevo == EstadoPedido.Enviado || nuevo == EstadoPedido.Cancelado,

                EstadoPedido.Enviado => nuevo == EstadoPedido.Entregado,

                EstadoPedido.Entregado => false,

                EstadoPedido.Cancelado => false,

                _ => false
            };
        }

    }
}