using SmartStockV1.Models;

namespace SmartStockV1.Repositories.Usuarios
{
    public interface IUsuarioRepository
    {
        //Metodos Lectura
        Task<Usuario?> GetByIdAsync(int id); //Obtener usuario por Id
        Task<Usuario?> GetByNombreAsync(string nombre); //Obtener usuario por Nombre
        Task<bool> ExistsByNombreAsync(string nombre); //Verificar si un usuario existe por Nombre
        Task<List<Usuario>> GetAllAsync(); //Obtener todos los usuarios
        Task<List<Usuario>> GetActivoAsync(); //Obtener todos los usuarios activos

        //Metodos Escritura
        Task AddAsync(Usuario usuario); //Agregar nuevo usuario
        void Update(Usuario usuario); //Actualizar usuario existente
        Task SaveChangesAsync(); //Guardar cambios en la base de datos
    }
}
