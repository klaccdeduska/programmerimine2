using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Operatsioonid
{
    public class DeleteOperatsioonCommand : IRequest<OperationResult<bool>>
    {
        public int Id { get; set; }
    }
}
