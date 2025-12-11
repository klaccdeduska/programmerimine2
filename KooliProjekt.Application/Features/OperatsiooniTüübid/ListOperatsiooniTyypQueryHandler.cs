using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.OperatsiooniTüübid
{
    public class ListOperatsiooniTyypQueryHandler :
        IRequestHandler<ListOperatsiooniTyypQuery, OperationResult<PagedResult<OperatsiooniTyyp>>>
    {
        private readonly ApplicationDbContext _db;

        public ListOperatsiooniTyypQueryHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<OperationResult<PagedResult<OperatsiooniTyyp>>> Handle(
            ListOperatsiooniTyypQuery request, CancellationToken ct)
        {
            var result = new OperationResult<PagedResult<OperatsiooniTyyp>>();

            result.Value = await _db.OperatsiooniTüübid
                .OrderBy(t => t.Nimi)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
