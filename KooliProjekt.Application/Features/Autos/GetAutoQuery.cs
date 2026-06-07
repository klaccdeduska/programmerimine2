using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Autos
{
    public class GetAutoQuery : IRequest<OperationResult<AutoDto>>
    {
        public int Id { get; set; }
    }
}