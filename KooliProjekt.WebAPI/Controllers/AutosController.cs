using KooliProjekt.Application.Features.Autos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutosController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public AutosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] ListAutosQuery query)
        {
            var response = await _mediator.Send(query);
            return Result(response);
        }
    }
}
