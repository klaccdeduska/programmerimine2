using KooliProjekt.Application.Features.Tootajad;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TootajadController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public TootajadController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] ListTootajadQuery query)
        {
            var response = await _mediator.Send(query);
            return Result(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _mediator.Send(new GetTootajaQuery { Id = id });
            return Result(response);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] SaveTootajaCommand command)
        {
            var response = await _mediator.Send(command);
            return Result(response);
        }
    }
}
