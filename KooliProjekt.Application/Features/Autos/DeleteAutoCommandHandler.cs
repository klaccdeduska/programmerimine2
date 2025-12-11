using MediatR;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Autos
{
    public class DeleteAutoCommandHandler :
        IRequestHandler<DeleteAutoCommand, OperationResult<bool>>
    {
        private readonly ApplicationDbContext _db;

        public DeleteAutoCommandHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<OperationResult<bool>> Handle(DeleteAutoCommand request, CancellationToken ct)
        {
            var result = new OperationResult<bool>();

            var entity = await _db.Autos.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (entity == null)
            {
                result.Errors.Add("Auto not found.");
                return result;
            }

            _db.Autos.Remove(entity);
            await _db.SaveChangesAsync(ct);

            result.Value = true;
            return result;
        }
    }
}
