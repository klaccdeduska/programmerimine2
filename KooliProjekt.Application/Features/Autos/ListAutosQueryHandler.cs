using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Autos
{
    public class ListAutosQueryHandler :
        IRequestHandler<ListAutosQuery, OperationResult<PagedResult<Auto>>>
    {
        private const int MaxPageSize = 100;
        private readonly IAutoRepository _repo;

        public ListAutosQueryHandler(IAutoRepository repo)
        {
            _repo = repo;
        }

        public async Task<OperationResult<PagedResult<Auto>>> Handle(
            ListAutosQuery request,
            CancellationToken ct)
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

            var query = _repo.Query();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim().ToLower();

                query = query.Where(x =>
                    ((x.Tootja ?? "") + " " + (x.Mudel ?? "") + " " + (x.Numbrimark ?? ""))
                    .ToLower()
                    .Contains(search));
            }

            var result = new OperationResult<PagedResult<Auto>>();

            result.Value = await query
                .OrderBy(x => x.Id)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}