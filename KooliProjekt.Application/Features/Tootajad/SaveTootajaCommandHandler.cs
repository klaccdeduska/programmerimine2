using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Tootajad
{
    public class SaveTootajaCommandHandler : IRequestHandler<SaveTootajaCommand, OperationResult<Töötaja>>
    {
        private readonly ApplicationDbContext _db;

        public SaveTootajaCommandHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<OperationResult<Töötaja>> Handle(SaveTootajaCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Töötaja>();
            Töötaja entity;

            if (request.Id == 0)
            {
                entity = new Töötaja();
                _db.Töötajad.Add(entity);
            }
            else
            {
                entity = await _db.Töötajad
                    .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

                if (entity == null)
                {
                    result.AddError("Töötaja not found");
                    return result;
                }
            }

            entity.Nimi = request.Nimi;
            entity.Email = request.Email;
            entity.Roll = request.Roll;

            await _db.SaveChangesAsync(cancellationToken);

            result.Value = entity;
            return result;
        }
    }
}
