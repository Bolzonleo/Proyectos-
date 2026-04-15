using Gestion_de_productos.Data;
using Gestion_de_productos.DTOs;
using Gestion_de_productos.Models;
using Gestion_de_productos.Services.Interfaces;
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

        public async Task<RolDTO> CrearAsync(CrearRolDTO dto)
        {
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

            _context.Roles.Remove(rol);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}