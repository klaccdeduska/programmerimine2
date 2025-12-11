using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Operatsioonid
{
    public class GetOperatsioonQueryHandler :
        IRequestHandler<GetOperatsioonQuery, OperationResult<Operatsioon>>
    {
        private readonly IOperatsioonRepository _repo;

        public GetOperatsioonQueryHandler(IOperatsioonRepository repo)
        {
            _repo = repo;
        }

        public async Task<OperationResult<Operatsioon>> Handle(GetOperatsioonQuery request, CancellationToken ct)
        {
            var result = new OperationResult<Operatsioon>();
            var entity = await _repo.GetByIdAsync(request.Id);

            if (entity == null)
            {
                result.Errors.Add("Operatsioon not found");
                return result;
            }

            result.Value = entity;
            return result;
        }
    }
}
