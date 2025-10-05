using AutoMapper;
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
    public record UpdateUserCommand(
            Guid Id,
            string Email,
            string FullName,
            string Gender,
            string Phone,
            string Role,
            bool IsActive
        ) : IRequest<UserDto>;

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByIdAsync(request.Id);
            if (user == null)
                throw new Exception("User not found");

            user.Email = request.Email;
            user.FullName = request.FullName;
            user.Gender = request.Gender;
            user.Phone = request.Phone;
            user.Role = Enum.Parse<Domain.Enums.UserRole>(request.Role, true);
            user.IsActive = request.IsActive;

            await _repository.UpdateAsync(user);

            return _mapper.Map<UserDto>(user);
        }
    }
}
