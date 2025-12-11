using MediatR;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.OperatsiooniTüübid
{
    public class DeleteOperatsiooniTyypCommandHandler :
        IRequestHandler<DeleteOperatsiooniTyypCommand, OperationResult<bool>>
    {
        private readonly ApplicationDbContext _db;

        public DeleteOperatsiooniTyypCommandHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<OperationResult<bool>> Handle(DeleteOperatsiooniTyypCommand request, CancellationToken ct)
        {
            var result = new OperationResult<bool>();

            var entity = await _db.OperatsiooniTüübid.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (entity == null)
            {
                result.Errors.Add("Operatsiooni tüüp not found.");
                return result;
            }

            _db.OperatsiooniTüübid.Remove(entity);
            await _db.SaveChangesAsync(ct);

            result.Value = true;
            return result;
        }
    }
}
