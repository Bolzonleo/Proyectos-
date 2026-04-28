using Gestion_de_productos.Data.Context;
using Gestion_de_productos.Models;
using Gestion_de_productos.Services.Interfaces;
using Gestion_de_productos.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Gestion_de_productos.Services
{
    public class RolService : IRolService
    {
        private readonly AppDbContext _context;

        public RolService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RolDTO>> ObtenerTodosAsync()
        {
            var roles = await _context.Roles.ToListAsync();
            return roles.Select(r => new RolDTO
            {
                Id = r.Id,
                Nombre = r.Nombre
            }).ToList();
        }

        public async Task<RolDTO> ObtenerPorIdAsync(int id)
        {
            var rol = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
            if (rol == null)
                throw new Exception($"Rol con ID {id} no encontrado");

            return new RolDTO
            {
                Id = rol.Id,
                Nombre = rol.Nombre
            };
        }

        public async Task<bool> ExistePorNombreAsync(string nombre, int? excluirId = null)
        {
            var nombreNormalizado = nombre.Trim().ToLower();

            return await _context.Roles
                .AnyAsync(r => r.Nombre.ToLower() == nombreNormalizado && (!excluirId.HasValue || r.Id != excluirId.Value));
        }

        public async Task<RolDTO> CrearAsync(CrearRolDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new Exception("El nombre del rol es obligatorio");

            var existe = await ExistePorNombreAsync(dto.Nombre);
            if (existe)
                throw new Exception("Ya existe un rol con ese nombre");

            var rol = new Rol
            {
                Nombre = dto.Nombre
            };

            _context.Roles.Add(rol);
            await _context.SaveChangesAsync();

            return new RolDTO
            {
                Id = rol.Id,
                Nombre = rol.Nombre
            };
        }

        public async Task<bool> ActualizarAsync(int id, CrearRolDTO dto)
        {
            var rol = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
            if (rol == null)
                throw new Exception($"Rol con ID {id} no encontrado");

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new Exception("El nombre del rol es obligatorio");

            var existe = await ExistePorNombreAsync(dto.Nombre, id);
            if (existe)
                throw new Exception("Ya existe un rol con ese nombre");

            rol.Nombre = dto.Nombre;
            _context.Roles.Update(rol);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var rol = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
            if (rol == null)
                throw new Exception($"Rol con ID {id} no encontrado");

            var tieneUsuariosAsociados = await _context.Usuarios.AnyAsync(u => u.RolId == id);
            if (tieneUsuariosAsociados)
                throw new Exception("No se puede eliminar el rol porque tiene usuarios asociados");

            _context.Roles.Remove(rol);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
