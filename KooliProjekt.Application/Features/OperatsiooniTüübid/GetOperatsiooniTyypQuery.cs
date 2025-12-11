using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.OperatsiooniTüübid
{
    public class GetOperatsiooniTyypQuery : IRequest<OperationResult<OperatsiooniTyyp>>
    {
        public int Id { get; set; }
    }
}
