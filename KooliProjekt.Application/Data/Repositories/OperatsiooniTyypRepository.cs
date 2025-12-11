namespace KooliProjekt.Application.Data.Repositories
{
    public class OperatsiooniTyypRepository : BaseRepository<OperatsiooniTyyp>, IOperatsiooniTyypRepository
    {
        public OperatsiooniTyypRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
