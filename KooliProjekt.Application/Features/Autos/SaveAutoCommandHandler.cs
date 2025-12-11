using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Autos
{
    public class SaveAutoCommandHandler : IRequestHandler<SaveAutoCommand, OperationResult<Auto>>
    {
        private readonly ApplicationDbContext _db;

        public SaveAutoCommandHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<OperationResult<Auto>> Handle(SaveAutoCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Auto>();
            Auto entity;

            if (request.Id == 0)
            {
                entity = new Auto();
                _db.Autos.Add(entity);
            }
            else
            {
                entity = await _db.Autos
                    .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

                if (entity == null)
                {
                    result.AddError("Auto not found");
                    return result;
                }
            }

            entity.Tootja = request.Tootja;
            entity.Mudel = request.Mudel;
            entity.Numbrimark = request.Numbrimark;

            await _db.SaveChangesAsync(cancellationToken);

            result.Value = entity;
            return result;
        }
    }
}
