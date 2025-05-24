using System.Security.Cryptography;
using System.Text;

namespace Laundrify.Data
{
    public static class DbSeeder
    {
        public static async Task Seed(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Seed admin user
            var adminEmail = "admin@laundrify.com";
            if (!context.Users.Any(u => u.Email == adminEmail))
            {
                context.Users.Add(new User
                {
                    Email = adminEmail,
                    PasswordHash = HashPassword("Admin#123"),
                    Role = "Admin"
                });
            }

            // Seed test clients
            for (int i = 1; i <= 3; i++)
            {
                var clientEmail = $"client{i}@laundrify.com";
                if (!context.Users.Any(u => u.Email == clientEmail))
                {
                    context.Users.Add(new User
                    {
                        Email = clientEmail,
                        PasswordHash = HashPassword($"Client#{i}23"),
                        Role = "Client"
                    });
                }
            }

            // Seed services
            if (!context.Services.Any())
            {
                context.Services.Add(new Service { Name = "Clothes Washing", Price = 5.0m, UnitType = "kg" });
                context.Services.Add(new Service { Name = "Carpet Cleaning", Price = 20.0m, UnitType = "item" });
                context.Services.Add(new Service { Name = "Dry Cleaning", Price = 10.0m, UnitType = "kg" });
            }

            await context.SaveChangesAsync();
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
} 