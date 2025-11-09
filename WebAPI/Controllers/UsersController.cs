using Application.Features.Users.Commands.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserCommand command)
        {
            try
            {
                var userDto = await _mediator.Send(command);
                // GetUserById Query இருந்தால், CreatedAtAction-ஐ பயன்படுத்தலாம்
                return CreatedAtAction(nameof(CreateUser), new { id = userDto.Id }, userDto);
            }
            catch (Exception ex)
            {
                // Error Handling Middleware இதை கையாளும், ஆனால் இங்கேயும் கையாளலாம்
                return BadRequest(ex.Message);
            }
        }
    }
}