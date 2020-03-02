using DictionaryOfWords.Core.DataBase;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryOfWords.DAL.Data.Init
{
    public class DataDbInitializer
    {
        public async Task SeedAsync(IApplicationBuilder app)
        {
            using (var score = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var userManager = score.ServiceProvider.GetRequiredService<UserManager<User>>();

                var roleManager = score.ServiceProvider.GetRequiredService<RoleManager<Role>>();

                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    Role roleAdmin = new Role("Admin");
                    await roleManager.CreateAsync(roleAdmin);
                }

                if (!await roleManager.RoleExistsAsync("User"))
                {
                    Role roleUser = new Role("User");
                    await roleManager.CreateAsync(roleUser);
                }
                User admin = await userManager.FindByNameAsync("admin");

                if (admin == null)
                {
                    var user = new User
                    {
                        UserName = "admin",
                        Email = "admin@test.ru",
                        Login = "admin",
                        FullName = "Админ"
                        //ImagePath = "images/admin.png"
                    };

                    var result = await userManager.CreateAsync(user, "123456");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                }

                admin = await userManager.FindByNameAsync("user");
                if (admin == null)
                {
                    var user = new User
                    {
                        UserName = "user",
                        Email = "user@test.ru",
                        Login = "user",
                        FullName = "Пользователь"
                        //ImagePath = "images/admin.png"
                    };
                    var result = await userManager.CreateAsync(user, "123456");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "User");
                    }
                }
            }
        }
    }
}

