using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Autos
{
    public class GetAutoQueryHandler : IRequestHandler<GetAutoQuery, OperationResult<Auto>>
    {
        private readonly ApplicationDbContext _db;

        public GetAutoQueryHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<OperationResult<Auto>> Handle(GetAutoQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Auto>();

            var entity = await _db.Autos
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                result.AddError("Auto not found");
                return result;
            }

            result.Value = entity;
            return result;
        }
    }
}
