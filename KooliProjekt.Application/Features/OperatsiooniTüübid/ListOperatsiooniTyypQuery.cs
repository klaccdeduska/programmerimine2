using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.OperatsiooniTüübid
{
    public class ListOperatsiooniTyypQuery :
        IRequest<OperationResult<PagedResult<OperatsiooniTyyp>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; }
    }
}