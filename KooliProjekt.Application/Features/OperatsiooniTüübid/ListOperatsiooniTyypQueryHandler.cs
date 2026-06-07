using System.Linq;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.OperatsiooniTüübid
{
    public class ListOperatsiooniTyypQueryHandler :
        IRequestHandler<ListOperatsiooniTyypQuery, OperationResult<PagedResult<OperatsiooniTyyp>>>
    {
        private const int MaxPageSize = 100;
        private readonly IOperatsiooniTyypRepository _repo;

        public ListOperatsiooniTyypQueryHandler(IOperatsiooniTyypRepository repo)
        {
            _repo = repo;
        }

        public async Task<OperationResult<PagedResult<OperatsiooniTyyp>>> Handle(ListOperatsiooniTyypQuery request, CancellationToken ct)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Page <= 0)
            {
                throw new ArgumentException("Page must be greater than zero.", nameof(request.Page));
            }

            if (request.PageSize <= 0)
            {
                throw new ArgumentException("PageSize must be greater than zero.", nameof(request.PageSize));
            }

            if (request.PageSize > MaxPageSize)
            {
                throw new ArgumentException($"PageSize must be less than or equal to {MaxPageSize}.", nameof(request.PageSize));
            }

            var result = new OperationResult<PagedResult<OperatsiooniTyyp>>();

            result.Value = await _repo
                .Query()
                .OrderBy(x => x.Id)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}