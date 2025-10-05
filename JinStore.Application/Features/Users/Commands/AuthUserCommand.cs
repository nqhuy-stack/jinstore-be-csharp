using JinStore.Application.DTOs;
using JinStore.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JinStore.Application.Features.Users.Commands
{
    public record AuthUserCommand(string Username, string Password) : IRequest<UserDto?>;

    public class AuthUserCommandHandler : IRequestHandler<AuthUserCommand, UserDto?>
    {
        private readonly IUserRepository _repository;

        public AuthUserCommandHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserDto?> Handle(AuthUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByUsernameAsync(request.Username);
            if (user == null) return null;

            bool isValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isValid) return null;

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                Gender = user.Gender,
                Phone = user.Phone,
                Role = user.Role.ToString(),
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
