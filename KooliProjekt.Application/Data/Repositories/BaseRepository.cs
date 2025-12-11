using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : Entity
    {
        protected readonly ApplicationDbContext _db;
        protected readonly DbSet<T> _table;

        public BaseRepository(ApplicationDbContext db)
        {
            _db = db;
            _table = db.Set<T>();
        }

        public IQueryable<T> Query()
        {
            return _table.AsQueryable();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _table.FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task AddAsync(T entity)
        {
            await _table.AddAsync(entity);
        }

        public void Remove(T entity)  
        {
            _table.Remove(entity);
        }

        public virtual async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
