using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SCRepository.Entity;
using SCRepository.Entity.Models.KeyUserModel;
//using SCRepository.Entity.Repository.RepositoryData;
using SCRepository.Entity.Repository.RepositoryData;
using StudentCardsAdmin.Services;
using System;

namespace StudentCardsAdmin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IDataContextRepository, DataContextRepository>();
            services.AddTransient<SCRepository.Entity.Repository.UserRepository.IDataContextRepository, SCRepository.Entity.Repository.UserRepository.DataContextRepository>();
            var connection = Configuration.GetConnectionString("DefaultConnection");
            var negacon = Configuration.GetConnectionString("Nega");
            services.AddTransient(x => new AutorizationService(connection));
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(3);
                options.Cookie.IsEssential = true;
                options.Cookie.HttpOnly = true;
            });
            services.AddControllersWithViews();
            services.AddMvc();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});

            app.UseRouting();
            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Autorization}/{action=Index}/{id?}");
            });
        }
    }
}
