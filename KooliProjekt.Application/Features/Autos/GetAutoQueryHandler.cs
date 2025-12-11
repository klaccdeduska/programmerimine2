using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Autos
{
    public class GetAutoQueryHandler : IRequestHandler<GetAutoQuery, OperationResult<Auto>>
    {
        private readonly IAutoRepository _repo;

        public GetAutoQueryHandler(IAutoRepository repo)
        {
            _repo = repo;
        }

        public async Task<OperationResult<Auto>> Handle(GetAutoQuery request, CancellationToken ct)
        {
            var result = new OperationResult<Auto>();

            var auto = await _repo.GetByIdAsync(request.Id);

            if (auto == null)
            {
                result.Errors.Add("Auto not found");
                return result;
            }

            result.Value = auto;
            return result;
        }
    }
}
