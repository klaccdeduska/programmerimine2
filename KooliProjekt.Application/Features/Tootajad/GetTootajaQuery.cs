using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Tootajad
{
    public class GetTootajaQuery : IRequest<OperationResult<TootajaDto>>
    {
        public int Id { get; set; }
    }
}