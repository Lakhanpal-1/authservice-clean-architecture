using AuthService.Application.Interfaces.Repositories;
using AuthService.Application.Interfaces.Security;
using AuthService.Domain.Entities;
using AuthService.Domain.Enums;
using AuthService.Domain.Exceptions;

namespace AuthService.Application.UseCases.Register
{
    public class RegisterHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Guid> Handle(RegisterCommand command)
        {
            var existingUser = await _userRepository.GetByEmailAsync(command.Email);
            if (existingUser != null)
                throw new DomainException("User already exists");

            var passwordHash = _passwordHasher.Hash(command.Password);

            var user = new User(
                command.Email,
                passwordHash,
                UserRole.Employee
            );

            await _userRepository.AddAsync(user);

            return user.Id; // ✅ return minimal data
        }

    }
}
