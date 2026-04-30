using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Interfaces.Security
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}
