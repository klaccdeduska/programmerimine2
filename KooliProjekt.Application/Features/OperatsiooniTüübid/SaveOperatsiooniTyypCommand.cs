using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.OperatsiooniTüübid
{
    public class SaveOperatsiooniTyypCommand :
        IRequest<OperationResult<OperatsiooniTyyp>>
    {
        public int Id { get; set; }
        public string Nimi { get; set; }
        public string Kirjeldus { get; set; }
    }
}
