using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Contracts.Auth
{
    public class RegisterResponse
    {
        public Guid UserId { get; init; }
        public string Email { get; init; } = default!;
    }
}
