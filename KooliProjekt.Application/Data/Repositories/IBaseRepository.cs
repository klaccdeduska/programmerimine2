using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IBaseRepository<T> where T : Entity
    {
        IQueryable<T> Query();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Remove(T entity);         // ← ДОБАВЛЕНО
        Task SaveChangesAsync();
    }
}
