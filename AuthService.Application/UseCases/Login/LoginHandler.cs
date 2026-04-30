using AuthService.Application.Interfaces.Repositories;
using AuthService.Application.Interfaces.Security;
using AuthService.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.UseCases.Login
{
    public class LoginHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IPasswordHasher _passwordHasher;

        public LoginHandler(
            IUserRepository userRepository,
            IJwtProvider jwtProvider,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> Handle(LoginCommand command)
        {
            var user = await _userRepository.GetByEmailAsync(command.Email);

            if (user == null)
                throw new DomainException("Invalid credentials");

            if (!_passwordHasher.Verify(user.PasswordHash, command.Password))
                throw new DomainException("Invalid credentials");

            return _jwtProvider.GenerateToken(user);
        }

    }
}
