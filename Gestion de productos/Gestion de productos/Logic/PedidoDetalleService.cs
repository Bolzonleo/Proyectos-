using Gestion_de_productos.Services.Interfaces;
using Gestion_de_productos.Shared.DTOs;
using Gestion_de_productos.Shared.Entities;

namespace Gestion_de_productos.Services
{
    public class PedidoDetalleService : IPedidoDetalleService
    {
        private readonly IPedidoDetalleRepository _detalleRepo;
        private readonly IPedidoRepository _pedidoRepo;
        private readonly IProductoRepository _productoRepo;

        public PedidoDetalleService(
            IPedidoDetalleRepository detalleRepo,
            IPedidoRepository pedidoRepo,
            IProductoRepository productoRepo)
        {
            _detalleRepo = detalleRepo;
            _pedidoRepo = pedidoRepo;
            _productoRepo = productoRepo;
        }

        // =====================================
        public async Task<IEnumerable<PedidoDetalleDTO>> ObtenerPorPedidoIdAsync(int pedidoId)
        {
            var detalles = await _detalleRepo.ObtenerPorPedidoIdAsync(pedidoId);

            return detalles.Select(Mapear);
        }

        // =====================================
        public async Task<PedidoDetalleDTO> AgregarDetalleAsync(int pedidoId, CrearPedidoDetalleDTO dto)
        {
            if (dto.Cantidad <= 0)
                throw new Exception("La cantidad debe ser mayor a 0");

            var pedido = await _pedidoRepo.ObtenerPorIdAsync(pedidoId);
            if (pedido == null)
                throw new Exception("Pedido no encontrado");

            var producto = await _productoRepo.ObtenerPorIdAsync(dto.ProductoId);
            if (producto == null)
                throw new Exception("Producto no encontrado");

            var precio = dto.PrecioUnitario > 0
                ? dto.PrecioUnitario
                : producto.Precio;

            var detalle = new PedidoDetalle
            {
                PedidoId = pedidoId,
                ProductoId = dto.ProductoId,
                Cantidad = dto.Cantidad,
                PrecioUnitario = (double)precio
            };

            await _detalleRepo.AgregarAsync(detalle);

            // 🔥 recalcular total (mejor que sumar/restar incremental)
            pedido.Total += (double)(dto.Cantidad * precio);

            await _pedidoRepo.GuardarCambiosAsync();

            detalle.Producto = producto;

            return Mapear(detalle);
        }

        // =====================================
        public async Task EliminarDetalleAsync(int id)
        {
            var detalle = await _detalleRepo.ObtenerPorIdAsync(id);

            if (detalle == null)
                throw new Exception("Detalle no encontrado");

            var pedido = await _pedidoRepo.ObtenerPorIdAsync(detalle.PedidoId);

            if (pedido != null)
            {
                pedido.Total -= detalle.PrecioUnitario * detalle.Cantidad;

                if (pedido.Total < 0)
                    pedido.Total = 0;
            }

            _detalleRepo.Eliminar(detalle);

            await _pedidoRepo.GuardarCambiosAsync();
        }

        // =====================================
        private static PedidoDetalleDTO Mapear(PedidoDetalle d)
        {
            return new PedidoDetalleDTO
            {
                Id = d.Id,
                PedidoId = d.PedidoId,
                ProductoId = d.ProductoId,
                Cantidad = d.Cantidad,
                PrecioUnitario = (decimal)d.PrecioUnitario,
                ProductoNombre = d.Producto?.Nombre ?? string.Empty
            };
        }
    }
}