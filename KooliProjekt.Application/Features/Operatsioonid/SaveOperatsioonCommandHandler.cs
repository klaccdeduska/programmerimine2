using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Operatsioonid
{
    public class SaveOperatsioonCommandHandler :
        IRequestHandler<SaveOperatsioonCommand, OperationResult<Operatsioon>>
    {
        private readonly IOperatsioonRepository _repo;

        public SaveOperatsioonCommandHandler(IOperatsioonRepository repo)
        {
            _repo = repo;
        }

        public async Task<OperationResult<Operatsioon>> Handle(SaveOperatsioonCommand request, CancellationToken ct)
        {
            var result = new OperationResult<Operatsioon>();
            Operatsioon entity;

            if (request.Id == 0)
            {
                entity = new Operatsioon();
                await _repo.AddAsync(entity);
            }
            else
            {
                entity = await _repo.GetByIdAsync(request.Id);

                if (entity == null)
                {
                    result.Errors.Add("Operatsioon not found");
                    return result;
                }
            }

            entity.AutoId = request.AutoId;
            entity.TöötajaId = request.TöötajaId;
            entity.TüüpId = request.TüüpId;
            entity.Kuupäev = request.Kuupäev;
            entity.Staatus = request.Staatus;

            entity.Maksumus = request.Maksumus ?? 0m;

            await _repo.SaveChangesAsync();

            result.Value = entity;
            return result;
        }

    }
}
