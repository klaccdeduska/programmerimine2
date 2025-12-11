using KooliProjekt.Application.Features.Operatsioonid;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OperatsioonidController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public OperatsioonidController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] ListOperatsioonidQuery query)
        {
            var response = await _mediator.Send(query);
            return Result(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _mediator.Send(new GetOperatsioonQuery { Id = id });
            return Result(response);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] SaveOperatsioonCommand command)
        {
            var response = await _mediator.Send(command);
            return Result(response);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new DeleteOperatsioonCommand { Id = id });
            return Result(response);
        }


    }
}
