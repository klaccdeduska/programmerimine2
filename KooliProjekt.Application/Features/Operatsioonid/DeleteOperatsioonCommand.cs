using MediatR;
using KooliProjekt.Application.Infrastructure.Results;

namespace KooliProjekt.Application.Features.Operatsioonid
{
    public class DeleteOperatsioonCommand : IRequest<OperationResult<bool>>
    {
        public int Id { get; set; }
    }
}
