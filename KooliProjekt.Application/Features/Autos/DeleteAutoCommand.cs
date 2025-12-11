using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Autos
{
    public class DeleteAutoCommand : IRequest<OperationResult<bool>>
    {
        public int Id { get; set; }
    }
}
