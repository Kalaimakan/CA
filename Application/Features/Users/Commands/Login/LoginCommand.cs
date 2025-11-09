using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.Login
{
    // Login Command
    public class LoginCommand : IRequest<AuthResponseDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
