using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.OperatsiooniTüübid
{
    public class SaveOperatsiooniTyypCommandHandler :
        IRequestHandler<SaveOperatsiooniTyypCommand, OperationResult<OperatsiooniTyyp>>
    {
        private readonly ApplicationDbContext _db;

        public SaveOperatsiooniTyypCommandHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<OperationResult<OperatsiooniTyyp>> Handle(
            SaveOperatsiooniTyypCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<OperatsiooniTyyp>();
            OperatsiooniTyyp entity;

            if (request.Id == 0)
            {
                entity = new OperatsiooniTyyp();
                _db.OperatsiooniTüübid.Add(entity);
            }
            else
            {
                entity = await _db.OperatsiooniTüübid
                    .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

                if (entity == null)
                {
                    result.AddError("OperatsiooniTyyp not found");
                    return result;
                }
            }

            entity.Nimi = request.Nimi;
            entity.Kirjeldus = request.Kirjeldus;

            await _db.SaveChangesAsync(cancellationToken);

            result.Value = entity;
            return result;
        }
    }
}
