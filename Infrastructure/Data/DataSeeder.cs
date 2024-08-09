using Domain.Entities;

namespace Infrastructure.Data
{
    public class DataSeeder
    {
        public static async Task Seed(UserDbContext context)
        {
            context.Database.EnsureCreated();
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    if (!context.Users.Any())
                    {

                        var users = new List<User>
                        {
                            new User (username: "SuperAdmin", password: "password", email: "superadmin@admin.com", role: "super"),
                            new User (username: "Admin", password: "password", email: "admin@admin.com", role: "admin"),
                            new User (username: "Guest", password: "password", email: "guest@admin.com", role: "user"),
                        };
                        users.ForEach(user =>
                        {
                            user.UpdatePassword(BCrypt.Net.BCrypt.HashPassword(user.Password));
                        });
                        await context.ResetPrimaryKeyAutoIncrementAsync();
                        context.Users.AddRange(users);
                        await context.SaveChangesAsync();
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction?.Rollback();
                    throw;
                }
            }
        }
    }
}
