using AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; } = null!;
        public string PasswordHash { get; private set; } = null!;
        public UserRole Role { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private User() { } // EF Core

        public User(string email, string passwordHash, UserRole role)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required");

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password hash is required");

            Id = Guid.NewGuid();
            Email = email.ToLowerInvariant();
            PasswordHash = passwordHash;
            Role = role;
            CreatedAt = DateTime.UtcNow;
        }

        public void ChangePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
                throw new ArgumentException("Password hash is required");

            PasswordHash = newPasswordHash;
        }

        public void ChangeRole(UserRole role)
        {
            Role = role;
        }
    }
}
