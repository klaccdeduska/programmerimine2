using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.OperatsiooniTüübid
{
    public class SaveOperatsiooniTyypCommandHandler :
        IRequestHandler<SaveOperatsiooniTyypCommand, OperationResult<OperatsiooniTyyp>>
    {
        private readonly IOperatsiooniTyypRepository _repo;

        public SaveOperatsiooniTyypCommandHandler(IOperatsiooniTyypRepository repo)
        {
            _repo = repo;
        }

        public async Task<OperationResult<OperatsiooniTyyp>> Handle(SaveOperatsiooniTyypCommand request, CancellationToken ct)
        {
            var result = new OperationResult<OperatsiooniTyyp>();
            OperatsiooniTyyp entity;

            if (request.Id == 0)
            {
                entity = new OperatsiooniTyyp();
                await _repo.AddAsync(entity);
            }
            else
            {
                entity = await _repo.GetByIdAsync(request.Id);

                if (entity == null)
                {
                    result.Errors.Add("Operatsiooni tüüp not found");
                    return result;
                }
            }

            entity.Nimi = request.Nimi;
            entity.Kirjeldus = request.Kirjeldus;

            await _repo.SaveChangesAsync();

            result.Value = entity;
            return result;
        }
    }
}
