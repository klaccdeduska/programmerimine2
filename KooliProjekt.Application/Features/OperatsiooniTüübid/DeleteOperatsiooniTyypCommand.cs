using MediatR;
using KooliProjekt.Application.Infrastructure.Results;

namespace KooliProjekt.Application.Features.OperatsiooniTüübid
{
    public class DeleteOperatsiooniTyypCommand : IRequest<OperationResult<bool>>
    {
        public int Id { get; set; }
    }
}
