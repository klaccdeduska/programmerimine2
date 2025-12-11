using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Tootajad
{
    public class ListTootajadQueryHandler :
        IRequestHandler<ListTootajadQuery, OperationResult<PagedResult<Töötaja>>>
    {
        private readonly ApplicationDbContext _db;

        public ListTootajadQueryHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<OperationResult<PagedResult<Töötaja>>> Handle(
            ListTootajadQuery request, CancellationToken ct)
        {
            var result = new OperationResult<PagedResult<Töötaja>>();

            result.Value = await _db.Töötajad
                .OrderBy(t => t.Nimi)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
