using easy_jwt.identity_exercise.Models; // behövs för att använda AppUser
using Microsoft.AspNetCore.Identity; // behövs för: UserManager, RoleManager, IdentityRole

namespace easy_jwt.identity_exercise.Data
// detta är klassen som körs när appen startar och ser till att
// rollerna finns, adminanvändaren finns
{
    public static class DbSeeder // static eftersom vi bara vill ha en hjälpklass med en metod vi kan anropa vid startup
    {
        public static async Task SeedRolesAndAdminAsync(
            RoleManager<IdentityRole> roleManager, // används för att kolla/skapa roller
            UserManager<AppUser> userManager) // används för att kolla/skapa användare
        {
            string[] roles = { "Admin", "Teacher", "Student" }; // skapa roll-listan

            foreach (var role in roles) // loopa igenom rollerna, kolla om den redan finns, om inte: skapa den
                                        // detta gör att seedningen kan köras flera gånger utan att krascha (idempotent)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // skapa admin-email och lösenord
            var adminEmail = "admin@learnpoint.com";
            var adminPassWord = "Admin123!";

            // kolla om admin finns
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            // skapa admin om den inte finns
            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail
                };

                var result = await userManager.CreateAsync(adminUser, adminPassWord);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // skapa användaren, sätt email/userName,
            // skapa kontot med lösenord, lägg användaren i Admin-rollen
        }
    }
}