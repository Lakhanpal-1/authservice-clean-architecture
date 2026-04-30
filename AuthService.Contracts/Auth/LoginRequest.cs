using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Contracts.Auth
{
    public class LoginRequest
    {
        public string Email { get; init; } = default!;
        public string Password { get; init; } = default!;
    }
}
