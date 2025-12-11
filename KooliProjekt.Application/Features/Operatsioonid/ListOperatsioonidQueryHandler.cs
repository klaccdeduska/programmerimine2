using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Operatsioonid
{
    public class ListOperatsioonidQueryHandler :
        IRequestHandler<ListOperatsioonidQuery, OperationResult<PagedResult<Operatsioon>>>
    {
        private readonly ApplicationDbContext _db;

        public ListOperatsioonidQueryHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<OperationResult<PagedResult<Operatsioon>>> Handle(
            ListOperatsioonidQuery request, CancellationToken ct)
        {
            var result = new OperationResult<PagedResult<Operatsioon>>();

            result.Value = await _db.Operatsioonid
                .OrderBy(o => o.Kuupäev)
                .Include(o => o.Auto)
                .Include(o => o.Töötaja)
                .Include(o => o.Tüüp)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
