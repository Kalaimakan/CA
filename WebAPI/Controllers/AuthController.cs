using Application.DTOs;
using Application.Features.Users.Commands.Login;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            // DTO-வை Command-ஆக Map செய்கிறோம்
            var command = new LoginCommand
            {
                Email = request.Email,
                Password = request.Password
            };

            try
            {
                var authResponse = await _mediator.Send(command);
                return Ok(authResponse);
            }
            catch (Exception ex)
            {
                // NotFoundException, ValidationException போன்றவற்றை Error Handling Middleware கையாளும்
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
