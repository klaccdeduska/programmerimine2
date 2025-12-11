using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Autos
{
    public class GetAutoQuery : IRequest<OperationResult<Auto>>
    {
        public int Id { get; set; }
    }
}
