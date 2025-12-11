using MediatR;
using KooliProjekt.Application.Infrastructure.Results;

namespace KooliProjekt.Application.Features.Tootajad
{
    public class DeleteTootajaCommand : IRequest<OperationResult<bool>>
    {
        public int Id { get; set; }
    }
}
