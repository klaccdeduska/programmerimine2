using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.OperatsiooniTüübid
{
    public class GetOperatsiooniTyypQueryHandler
        : IRequestHandler<GetOperatsiooniTyypQuery, OperationResult<OperatsiooniTyyp>>
    {
        private readonly ApplicationDbContext _db;

        public GetOperatsiooniTyypQueryHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<OperationResult<OperatsiooniTyyp>> Handle(
            GetOperatsiooniTyypQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<OperatsiooniTyyp>();

            var entity = await _db.OperatsiooniTüübid
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                result.AddError("OperatsiooniTyyp not found");
                return result;
            }

            result.Value = entity;
            return result;
        }
    }
}
