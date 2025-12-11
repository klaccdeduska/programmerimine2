using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.OperatsiooniTüübid
{
    public class DeleteOperatsiooniTyypCommand : IRequest<OperationResult<bool>>
    {
        public int Id { get; set; }
    }
}
