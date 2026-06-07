using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.OperatsiooniTüübid
{
    public class GetOperatsiooniTyypQuery :
        IRequest<OperationResult<OperatsiooniTyypDto>>
    {
        public int Id { get; set; }
    }
}