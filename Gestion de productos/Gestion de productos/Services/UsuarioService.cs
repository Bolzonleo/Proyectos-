using Gestion_de_productos.Data;
using Gestion_de_productos.DTOs;
using Gestion_de_productos.Models;
using Gestion_de_productos.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gestion_de_productos.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AppDbContext _context;

        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UsuarioDTO>> ObtenerTodosAsync()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return usuarios.Select(u => new UsuarioDTO
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Email = u.Email,
                RolId = u.RolId
            }).ToList();
        }

        public async Task<UsuarioDTO> ObtenerPorIdAsync(int id)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
            if (usuario == null)
                throw new Exception($"Usuario con ID {id} no encontrado");

            return new UsuarioDTO
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                RolId = usuario.RolId
            };
        }

        public async Task<UsuarioDTO> CrearAsync(CrearUsuarioDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new Exception("El nombre del usuario es obligatorio");
            if (string.IsNullOrWhiteSpace(dto.Email) || !dto.Email.Contains("@"))
                throw new Exception("El email no es válido");
            if (string.IsNullOrWhiteSpace(dto.Contraseña) || dto.Contraseña.Length < 6)
                throw new Exception("La contraseña debe tener al menos 6 caracteres");

            var rolExiste = await _context.Roles.AnyAsync(r => r.Id == dto.RolId);
            if (!rolExiste)
                throw new Exception("El rol indicado no existe");

            var emailEnUso = await _context.Usuarios.AnyAsync(u => u.Email == dto.Email.Trim());
            if (emailEnUso)
                throw new Exception("El email ya está registrado");

            var usuario = new Usuario
            {
                Nombre = dto.Nombre.Trim(),
                Email = dto.Email.Trim(),
                Contraseña = dto.Contraseña,
                RolId = dto.RolId
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return new UsuarioDTO
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                RolId = usuario.RolId
            };
        }

        public async Task<bool> ActualizarAsync(int id, ActualizarUsuarioDTO dto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
            if (usuario == null)
                throw new Exception($"Usuario con ID {id} no encontrado");

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new Exception("El nombre del usuario es obligatorio");
            if (string.IsNullOrWhiteSpace(dto.Email) || !dto.Email.Contains("@"))
                throw new Exception("El email no es válido");

            var emailEnUso = await _context.Usuarios.AnyAsync(u => u.Id != id && u.Email == dto.Email.Trim());
            if (emailEnUso)
                throw new Exception("El email ya está registrado por otro usuario");

            usuario.Nombre = dto.Nombre.Trim();
            usuario.Email = dto.Email.Trim();

            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Pedidos)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
                throw new Exception($"Usuario con ID {id} no encontrado");

            if (usuario.Pedidos.Any())
                throw new Exception("No se puede eliminar un usuario con pedidos asociados");

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
