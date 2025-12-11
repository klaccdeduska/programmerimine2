using MediatR;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Operatsioonid
{
    public class DeleteOperatsioonCommandHandler :
        IRequestHandler<DeleteOperatsioonCommand, OperationResult<bool>>
    {
        private readonly ApplicationDbContext _db;

        public DeleteOperatsioonCommandHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<OperationResult<bool>> Handle(DeleteOperatsioonCommand request, CancellationToken ct)
        {
            var result = new OperationResult<bool>();

            var entity = await _db.Operatsioonid.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (entity == null)
            {
                result.Errors.Add("Operatsioon not found.");
                return result;
            }

            _db.Operatsioonid.Remove(entity);
            await _db.SaveChangesAsync(ct);

            result.Value = true;
            return result;
        }
    }
}
