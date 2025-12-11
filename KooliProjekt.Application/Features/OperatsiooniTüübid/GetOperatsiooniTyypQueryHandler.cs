using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.OperatsiooniTüübid
{
    public class GetOperatsiooniTyypQueryHandler :
        IRequestHandler<GetOperatsiooniTyypQuery, OperationResult<OperatsiooniTyyp>>
    {
        private readonly IOperatsiooniTyypRepository _repo;

        public GetOperatsiooniTyypQueryHandler(IOperatsiooniTyypRepository repo)
        {
            _repo = repo;
        }

        public async Task<OperationResult<OperatsiooniTyyp>> Handle(GetOperatsiooniTyypQuery request, CancellationToken ct)
        {
            var result = new OperationResult<OperatsiooniTyyp>();
            var entity = await _repo.GetByIdAsync(request.Id);

            if (entity == null)
            {
                result.Errors.Add("Operatsiooni tüüp not found");
                return result;
            }

            result.Value = entity;
            return result;
        }
    }
}
