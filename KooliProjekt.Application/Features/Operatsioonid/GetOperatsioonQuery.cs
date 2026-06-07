using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Operatsioonid
{
    public class GetOperatsioonQuery :
        IRequest<OperationResult<OperatsioonDto>>
    {
        public int Id { get; set; }
    }
}