using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Services.ProjectService.Command;

namespace TaskFlow.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Endpoint destinado a la creación de proyectos.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] CreateProjectCommand request)
        {
            await _mediator.Send(request);
            return Ok();
        }
    }
}
