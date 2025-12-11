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
    }
}
