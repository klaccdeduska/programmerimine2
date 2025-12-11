namespace KooliProjekt.Application.Data.Repositories
{
    public class TootajaRepository : BaseRepository<Töötaja>, ITootajaRepository
    {
        public TootajaRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
