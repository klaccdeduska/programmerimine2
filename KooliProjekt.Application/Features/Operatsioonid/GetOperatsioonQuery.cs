using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Operatsioonid
{
    public class GetOperatsioonQuery : IRequest<OperationResult<Operatsioon>>
    {
        public int Id { get; set; }
    }
}
