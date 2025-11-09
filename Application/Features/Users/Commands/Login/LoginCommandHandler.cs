using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.Login
{
    // LoginCommandHandler
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public LoginCommandHandler(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IJwtService jwtService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // 1. User-ஐ Email மூலம் தேடுதல்
            var user = await _unitOfWork.Repository<User>()
                .FindAsync(u => u.Email == request.Email);

            if (user == null)
            {
                throw new NotFoundException(nameof(User), request.Email);
            }

            // 2. Password-ஐ சரிபார்த்தல்
            var isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                // FluentValidation-ல் இருந்து வரும் ValidationException-ஐ பயன்படுத்தலாம்
                throw new ValidationException("Invalid email or password.");
            }

            // 3. JWT Token உருவாக்குதல்
            var token = _jwtService.GenerateToken(user);

            // 4. Response-ஐ தயார் செய்தல்
            return new AuthResponseDto
            {
                Token = token,
                User = _mapper.Map<UserDto>(user)
            };
        }
    }
}
