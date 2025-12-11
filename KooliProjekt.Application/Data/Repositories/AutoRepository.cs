namespace KooliProjekt.Application.Data.Repositories
{
    public class AutoRepository : BaseRepository<Auto>, IAutoRepository
    {
        public AutoRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
