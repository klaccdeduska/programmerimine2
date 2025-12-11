using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Tootajad
{
    public class SaveTootajaCommandHandler :
        IRequestHandler<SaveTootajaCommand, OperationResult<Töötaja>>
    {
        private readonly ITootajaRepository _repo;

        public SaveTootajaCommandHandler(ITootajaRepository repo)
        {
            _repo = repo;
        }

        public async Task<OperationResult<Töötaja>> Handle(SaveTootajaCommand request, CancellationToken ct)
        {
            var result = new OperationResult<Töötaja>();
            Töötaja entity;

            if (request.Id == 0)
            {
                entity = new Töötaja();
                await _repo.AddAsync(entity);
            }
            else
            {
                entity = await _repo.GetByIdAsync(request.Id);

                if (entity == null)
                {
                    result.Errors.Add("Töötaja not found");
                    return result;
                }
            }

            entity.Nimi = request.Nimi;
            entity.Email = request.Email;
            entity.Roll = request.Roll;

            await _repo.SaveChangesAsync();

            result.Value = entity;
            return result;
        }
    }
}
