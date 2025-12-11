using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Tootajad
{
    public class GetTootajaQueryHandler : IRequestHandler<GetTootajaQuery, OperationResult<Töötaja>>
    {
        private readonly ApplicationDbContext _db;

        public GetTootajaQueryHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<OperationResult<Töötaja>> Handle(GetTootajaQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Töötaja>();

            var entity = await _db.Töötajad
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                result.AddError("Töötaja not found");
                return result;
            }

            result.Value = entity;
            return result;
        }
    }
}
