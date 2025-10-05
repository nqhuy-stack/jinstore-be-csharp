using AutoMapper;
using JinStore.Application.DTOs;
using JinStore.Domain.Entities;
using JinStore.Domain.Enums;
using JinStore.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JinStore.Application.Features.Users.Commands
{
    public record CreateUserCommand(
           string Username,
           string Password,
           string Email,
           string FullName,
           string Gender,
           string Phone,
           string Role
       ) : IRequest<UserDto>;

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByUsernameAsync(request.Username);
            if (existing != null)
                throw new Exception("Username already exists");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Username = request.Username,
                PasswordHash = hashedPassword,
                Email = request.Email,
                FullName = request.FullName,
                Gender = request.Gender,
                Phone = request.Phone,
                Role = Enum.Parse<UserRole>(request.Role, true)
            };

            await _repository.AddAsync(user);
            return _mapper.Map<UserDto>(user);
        }
    }
}
