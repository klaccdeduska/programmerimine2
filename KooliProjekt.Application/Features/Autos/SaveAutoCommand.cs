using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Autos
{
    public class SaveAutoCommand : IRequest<OperationResult<Auto>>
    {
        public int Id { get; set; }
        public string Tootja { get; set; }
        public string Mudel { get; set; }
        public string Numbrimark { get; set; }
    }
}
