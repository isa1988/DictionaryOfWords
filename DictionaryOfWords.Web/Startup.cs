using System;
using Autofac.Extensions.DependencyInjection;
using DictionaryOfWords.DAL;
using DictionaryOfWords.SignalR;
using DictionaryOfWords.Web.AppStart;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DictionaryOfWords.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private readonly IConfiguration configuration;

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddCors();
            services.AddDatabaseContext(configuration);
            services.AddSettingUserAutorization();
            services.AddAutoMapperCustom();

            services.AddSignalR();

            // Add Database Initializer
            services.AddScoped<IDbInitializer, DataDbInitializer>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            return new AutofacServiceProvider(services.ConfigureAutofac());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IDbInitializer dbInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            dbInitializer.Initialize();

            app.UseSignalR(routes =>
            {
                routes.MapHub<ProgressHub>("/prog");
                routes.MapHub<ChatHub>("/chat");
            });

            app.UseAuthentication();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
