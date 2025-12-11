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
        private readonly IAutoRepository _repo;

        public ListAutosQueryHandler(IAutoRepository repo)
        {
            _repo = repo;
        }

        public async Task<OperationResult<PagedResult<Auto>>> Handle(
            ListAutosQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<Auto>>();

            result.Value = await _repo
                .Query()
                .OrderBy(a => a.Id)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
