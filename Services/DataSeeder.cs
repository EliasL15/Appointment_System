using Microsoft.AspNetCore.Identity;

public class DataSeeder
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;

    public DataSeeder(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task SeedRolesAndAdminAsync()
    {
        // Add Roles
        if (!await _roleManager.RoleExistsAsync("Admin"))
        {
            await _roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        if (!await _roleManager.RoleExistsAsync("Customer"))
        {
            await _roleManager.CreateAsync(new IdentityRole("Customer"));
        }

        // Create Admin User
        var adminUser = await _userManager.FindByEmailAsync("admin@example.com");
        if (adminUser == null)
        {
            adminUser = new IdentityUser
            {
                UserName = "admin@example.com",
                Email = "admin@example.com",
                EmailConfirmed = true
            };
            await _userManager.CreateAsync(adminUser, "Admin@123");

            // Assign Admin Role
            await _userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}
