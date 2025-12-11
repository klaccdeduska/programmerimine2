using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Operatsioonid
{
    public class SaveOperatsioonCommandHandler :
        IRequestHandler<SaveOperatsioonCommand, OperationResult<Operatsioon>>
    {
        private readonly ApplicationDbContext _db;

        public SaveOperatsioonCommandHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<OperationResult<Operatsioon>> Handle(
            SaveOperatsioonCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Operatsioon>();
            Operatsioon entity;

            if (request.Id == 0)
            {
                entity = new Operatsioon();
                _db.Operatsioonid.Add(entity);
            }
            else
            {
                entity = await _db.Operatsioonid
                    .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

                if (entity == null)
                {
                    result.AddError("Operatsioon not found");
                    return result;
                }
            }

            entity.AutoId = request.AutoId;
            entity.TüüpId = request.TüüpId;
            entity.TöötajaId = request.TöötajaId;
            entity.Kuupäev = request.Kuupäev;
            entity.Staatus = request.Staatus;
            entity.Maksumus = request.Maksumus;

            await _db.SaveChangesAsync(cancellationToken);

            result.Value = entity;
            return result;
        }
    }
}
