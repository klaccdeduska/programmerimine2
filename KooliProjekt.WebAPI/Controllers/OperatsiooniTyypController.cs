using KooliProjekt.Application.Features.OperatsiooniTüübid;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _mediator.Send(new GetOperatsiooniTyypQuery { Id = id });
            return Result(response);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] SaveOperatsiooniTyypCommand command)
        {
            var response = await _mediator.Send(command);
            return Result(response);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new DeleteOperatsiooniTyypCommand { Id = id });
            return Result(response);
        }

    }
}
