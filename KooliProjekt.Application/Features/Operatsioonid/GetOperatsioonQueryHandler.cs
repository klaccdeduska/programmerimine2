using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Operatsioonid
{
    public class GetOperatsioonQueryHandler :
        IRequestHandler<GetOperatsioonQuery, OperationResult<OperatsioonDto>>
    {
        private readonly IOperatsioonRepository _repo;

        public GetOperatsioonQueryHandler(IOperatsioonRepository repo)
        {
            _repo = repo;
        }

        public async Task<OperationResult<OperatsioonDto>> Handle(GetOperatsioonQuery request, CancellationToken ct)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<OperatsioonDto>();

            if (request.Id <= 0)
            {
                return result;
            }

            var entity = await _repo.GetByIdAsync(request.Id);

            if (entity == null)
            {
                return result;
            }

            result.Value = new OperatsioonDto
            {
                Id = entity.Id,
                AutoId = entity.AutoId,
                TöötajaId = entity.TöötajaId,
                TüüpId = entity.TüüpId,
                Kuupäev = entity.Kuupäev,
                Staatus = entity.Staatus,
                Maksumus = entity.Maksumus
            };

            return result;
        }
    }
}