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
    }
}
