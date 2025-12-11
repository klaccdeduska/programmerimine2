using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Operatsioonid
{
    public class GetOperatsioonQueryHandler
        : IRequestHandler<GetOperatsioonQuery, OperationResult<Operatsioon>>
    {
        private readonly ApplicationDbContext _db;

        public GetOperatsioonQueryHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<OperationResult<Operatsioon>> Handle(
            GetOperatsioonQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Operatsioon>();

            var entity = await _db.Operatsioonid
                .Include(o => o.Auto)
                .Include(o => o.Töötaja)
                .Include(o => o.Tüüp)
                .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                result.AddError("Operatsioon not found");
                return result;
            }

            result.Value = entity;
            return result;
        }
    }
}
