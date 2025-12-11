using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Tootajad
{
    public class GetTootajaQueryHandler :
        IRequestHandler<GetTootajaQuery, OperationResult<Töötaja>>
    {
        private readonly ITootajaRepository _repo;

        public GetTootajaQueryHandler(ITootajaRepository repo)
        {
            _repo = repo;
        }

        public async Task<OperationResult<Töötaja>> Handle(GetTootajaQuery request, CancellationToken ct)
        {
            var result = new OperationResult<Töötaja>();
            var entity = await _repo.GetByIdAsync(request.Id);

            if (entity == null)
            {
                result.Errors.Add("Töötaja not found");
                return result;
            }

            result.Value = entity;
            return result;
        }
    }
}
