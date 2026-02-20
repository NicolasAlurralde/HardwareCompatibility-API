using System.Collections.Generic;
using System.Threading.Tasks;

namespace PCBuilder.Domain.Interfaces
{
    // El <T> significa que esta interfaz funcionará para cualquier entidad (Processor, Motherboard, etc.)
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
