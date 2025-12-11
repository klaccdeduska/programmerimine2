using KooliProjekt.Application.Features.OperatsiooniTüübid;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.WebAPI.Controllers
{
    public class OperatsiooniTyypController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public OperatsiooniTyypController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] ListOperatsiooniTyypQuery query)
        {
            var response = await _mediator.Send(query);
            return Result(response);
        }
    }
}
