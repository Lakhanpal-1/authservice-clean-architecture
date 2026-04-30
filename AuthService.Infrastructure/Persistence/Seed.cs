using AuthService.Domain.Entities;
using AuthService.Domain.Enums;
using AuthService.Application.Interfaces.Security;
using AuthService.Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Persistence
{
    public static class AdminSeeder
    {
        public static async Task SeedAsync(
            AuthDbContext context,
            IPasswordHasher passwordHasher)
        {
            if (await context.Users.AnyAsync())
                return;

            var admin = new User(
                email: "admin@system.com",
                passwordHash: passwordHasher.Hash("Admin@123"),
                role: UserRole.Admin
            );

            context.Users.Add(admin);
            await context.SaveChangesAsync();
        }
    }
}
