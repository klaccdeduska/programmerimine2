using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Autos
{
    public class ListAutosQueryHandler :
        IRequestHandler<ListAutosQuery, OperationResult<PagedResult<Auto>>>
    {
        private readonly ApplicationDbContext _db;

        public ListAutosQueryHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<OperationResult<PagedResult<Auto>>> Handle(
            ListAutosQuery request, CancellationToken ct)
        {
            var result = new OperationResult<PagedResult<Auto>>();

            result.Value = await _db.Autos
                .OrderBy(a => a.Id)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
