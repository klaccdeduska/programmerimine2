using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.OperatsiooniTüübid
{
    public class GetOperatsiooniTyypQueryHandler :
        IRequestHandler<GetOperatsiooniTyypQuery, OperationResult<OperatsiooniTyypDto>>
    {
        private readonly IOperatsiooniTyypRepository _repo;

        public GetOperatsiooniTyypQueryHandler(IOperatsiooniTyypRepository repo)
        {
            _repo = repo;
        }

        public async Task<OperationResult<OperatsiooniTyypDto>> Handle(GetOperatsiooniTyypQuery request, CancellationToken ct)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<OperatsiooniTyypDto>();

            if (request.Id <= 0)
            {
                return result;
            }

            var entity = await _repo.GetByIdAsync(request.Id);

            if (entity == null)
            {
                return result;
            }

            result.Value = new OperatsiooniTyypDto
            {
                Id = entity.Id,
                Nimi = entity.Nimi,
                Kirjeldus = entity.Kirjeldus
            };

            return result;
        }
    }
}