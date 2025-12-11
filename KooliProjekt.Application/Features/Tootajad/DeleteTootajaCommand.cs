using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Tootajad
{
    public class DeleteTootajaCommand : IRequest<OperationResult<bool>>
    {
        public int Id { get; set; }
    }
}
