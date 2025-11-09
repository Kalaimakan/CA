using Application.Common.Interfaces;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.CreateUser
{
    // CreateUserCommand-ஐ கையாளும் Handler
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // 1. User ஏற்கனவே உள்ளாரா என சோதித்தல்
            var existingUser = await _unitOfWork.Repository<User>()
                .FindAsync(u => u.Email == request.Email);

            if (existingUser != null)
            {
                throw new UserAlreadyExistsException(request.Email);
            }

            // 2. Password-ஐ Hash செய்தல்
            var passwordHash = _passwordHasher.HashPassword(request.Password);

            // 3. புதிய User Entity-ஐ உருவாக்குதல்
            var user = new User(
                Guid.NewGuid(),
                request.Email,
                passwordHash,
                request.FirstName,
                request.LastName
            );

            // 4. Database-ல் சேர்த்தல்
            await _unitOfWork.Repository<User>().AddAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 5. UserDto-வாக மாற்றி அனுப்புதல்
            return _mapper.Map<UserDto>(user);
        }
    }
}
