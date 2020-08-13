using DictionaryOfWords.Core.DataBase;
using DictionaryOfWords.DAL.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryOfWords.DAL
{
    public class DataDbInitializer : IDbInitializer
    {
        public DataDbInitializer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private readonly IServiceProvider _serviceProvider;

        // Заведи себе за правило Никогда не использовать сигнатуру async void.
        // Всегда async Task
        // Когда ты используешь async void, то в месте вызова ты перестаешь ожидать результат выполнения всего этого метода,
        // И в итоге у тебя получится некая извращенная многопоточность, но только вот из этого потока ты не вытащишь
        // ни результат выполнения, ни эксепшн, если он тут произойдет.
        public async void Initialize()
        {
            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<DbContextDictionaryOfWords>();
                context.Database.EnsureCreated();
                
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();

                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<Role>>();

                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    Role roleAdmin = new Role("Admin");

                    // У тебя асинхронный метод, используй асинхронные вызовы
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

                    // Вопрос к логическому алгоритму.
                    // А если юзер не создался, то что должно произойти?
                    // Я бы добавил выброс исключения, чтобы клиенты твоего кода понимали, что произошла ошибка
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

