using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.UseCases.Login
{
    public record LoginCommand(string Email, string Password);
}
