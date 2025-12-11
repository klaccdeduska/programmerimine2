using MediatR;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Tootajad
{
    public class DeleteTootajaCommandHandler :
        IRequestHandler<DeleteTootajaCommand, OperationResult<bool>>
    {
        private readonly ApplicationDbContext _db;

        public DeleteTootajaCommandHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<OperationResult<bool>> Handle(DeleteTootajaCommand request, CancellationToken ct)
        {
            var result = new OperationResult<bool>();

            var entity = await _db.Töötajad.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (entity == null)
            {
                result.Errors.Add("Töötaja not found.");
                return result;
            }

            _db.Töötajad.Remove(entity);
            await _db.SaveChangesAsync(ct);

            result.Value = true;
            return result;
        }
    }
}
