using CarBookingService.Infrastructure;
using CarBookingService.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CarBookingService;

public class SeedDevelopmentData
{
    public static async Task SeedDevUser(
        IServiceProvider serviceProvider,
        IConfiguration configuration
    )
    {
        var context = serviceProvider.GetRequiredService<CarBookingServiceDbContext>();
        var amplicationRoles = configuration
            .GetSection("AmplicationRoles")
            .AsEnumerable()
            .Where(x => x.Value != null)
            .Select(x => x.Value.ToString())
            .ToArray();

        var usernameValue = "test@email.com";
        var passwordValue = "P@ssw0rd!";
        var user = new IdentityUser
        {
            Email = usernameValue,
            UserName = usernameValue,
            NormalizedUserName = usernameValue.ToUpperInvariant(),
            NormalizedEmail = usernameValue.ToUpperInvariant(),
        };

        var password = new PasswordHasher<IdentityUser>();
        var hashed = password.HashPassword(user, passwordValue);
        user.PasswordHash = hashed;
        var userStore = new UserStore<IdentityUser>(context);
        await userStore.CreateAsync(user);
        var _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        foreach (var role in amplicationRoles)
        {
            await userStore.AddToRoleAsync(user, _roleManager.NormalizeKey(role));
        }

        await context.SaveChangesAsync();
    }
}
