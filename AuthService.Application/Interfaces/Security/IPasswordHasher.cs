using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Interfaces.Security
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verify(string hash, string password);
    }
}
