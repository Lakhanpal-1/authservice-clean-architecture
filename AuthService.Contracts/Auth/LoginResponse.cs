using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Contracts.Auth
{
    public class LoginResponse
    {
        public string AccessToken { get; init; } = default!;
    }
}
