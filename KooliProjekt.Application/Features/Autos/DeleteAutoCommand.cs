using MediatR;
using KooliProjekt.Application.Infrastructure.Results;

namespace KooliProjekt.Application.Features.Autos
{
    public class DeleteAutoCommand : IRequest<OperationResult<bool>>
    {
        public int Id { get; set; }
    }
}
