using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KooliProjekt.Application.Data.Repositories
{
    public class OperatsioonRepository : BaseRepository<Operatsioon>, IOperatsioonRepository
    {
        public OperatsioonRepository(ApplicationDbContext db) : base(db)
        {
        }

        // пример, если хочешь включения по умолчанию
        public IQueryable<Operatsioon> QueryWithIncludes()
        {
            return _table
                .Include(o => o.Auto)
                .Include(o => o.Töötaja)
                .Include(o => o.Tüüp);
        }
    }
}
