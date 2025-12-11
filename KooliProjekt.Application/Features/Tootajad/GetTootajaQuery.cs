using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Tootajad
{
    public class GetTootajaQuery : IRequest<OperationResult<Töötaja>>
    {
        public int Id { get; set; }
    }
}
