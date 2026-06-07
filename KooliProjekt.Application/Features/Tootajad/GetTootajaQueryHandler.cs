using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Tootajad
{
    public class GetTootajaQueryHandler :
        IRequestHandler<GetTootajaQuery, OperationResult<TootajaDto>>
    {
        private readonly ITootajaRepository _repo;

        public GetTootajaQueryHandler(ITootajaRepository repo)
        {
            _repo = repo;
        }

        public async Task<OperationResult<TootajaDto>> Handle(GetTootajaQuery request, CancellationToken ct)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<TootajaDto>();

            if (request.Id <= 0)
            {
                return result;
            }

            var entity = await _repo.GetByIdAsync(request.Id);

            if (entity == null)
            {
                return result;
            }

            result.Value = new TootajaDto
            {
                Id = entity.Id,
                Nimi = entity.Nimi,
                Email = entity.Email,
                Roll = entity.Roll
            };

            return result;
        }
    }
}