using Microsoft.EntityFrameworkCore;
using SmartStockV1.Data;
using SmartStockV1.Models;
namespace SmartStockV1.Repositories.Usuarios
{

    //Implementacion de acceso a datos para usuarios usando Entity Framework Core
    //Solo trabaja con entidades de dominio, sin dtos ni logica de negocio
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly SmartStockDbContext _context;
        public UsuarioRepository(SmartStockDbContext context)
        {
            _context = context;
        }
        
        //Lectura
        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _context.Usuario
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.UsuarioId == id);
        }
        public async Task<Usuario?> GetByNombreAsync(string nombre)
        {
            return await _context.Usuario
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.UsuarioNombre == nombre);
        }
        public async Task<bool> ExistsByNombreAsync(string nombre)
        {
            return await _context.Usuario
                .AnyAsync(u => u.UsuarioNombre == nombre);
        }
        public async Task<List<Usuario>> GetAllAsync()
        {
            return await _context.Usuario
                .Include(u => u.Rol)
                .ToListAsync();
        }
        public async Task<List<Usuario>> GetActivoAsync()
        {
            return await _context.Usuario
                .Include(u => u.Rol)
                .Where(u => u.EstadoUsuario != EstadoUsuario.Inactivo)
                .ToListAsync();
        }

        //Escritura
        public async Task AddAsync(Usuario usuario)
        {
            await _context.Usuario.AddAsync(usuario);
        }
        public void Update(Usuario usuario)
        {
            _context.Usuario.Update(usuario);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
