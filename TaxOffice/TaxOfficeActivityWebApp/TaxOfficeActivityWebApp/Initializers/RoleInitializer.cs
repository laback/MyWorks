using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxOfficeActivityWebApp.Models;

namespace TaxOfficeActivityWebApp.Initializers
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string directorLogin = "director";
            string password = "director";
            if (await roleManager.FindByNameAsync("Директор") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Директор"));
            }
            if (await roleManager.FindByNameAsync("Главный бухгалтер") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Главный бухгалтер"));
            }
            if (await roleManager.FindByNameAsync("Бухгалтер") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Бухгалтер"));
            }
            if (await userManager.FindByNameAsync(directorLogin) == null)
            {
                User admin = new User { Email = directorLogin, UserName = directorLogin };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Директор");
                }
            }
        }
    }
}
