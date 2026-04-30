using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.UseCases.Register
{
    public record RegisterCommand(string Email, string Password);

}
