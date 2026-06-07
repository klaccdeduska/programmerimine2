using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Autos
{
    public class GetAutoQueryHandler :
        IRequestHandler<GetAutoQuery, OperationResult<AutoDto>>
    {
        private readonly IAutoRepository _repo;

        public GetAutoQueryHandler(IAutoRepository repo)
        {
            _repo = repo;
        }

        public async Task<OperationResult<AutoDto>> Handle(GetAutoQuery request, CancellationToken ct)
        {
            var result = new OperationResult<AutoDto>();

            if (request == null)
            {
                return result;
            }

            var entity = await _repo.GetByIdAsync(request.Id);

            if (entity == null)
            {
                return result;
            }

            result.Value = new AutoDto
            {
                Id = entity.Id,
                Tootja = entity.Tootja,
                Mudel = entity.Mudel,
                Numbrimark = entity.Numbrimark
            };

            return result;
        }
    }
}