using System;
using System.Linq;
using System.Threading.Tasks;
using Instaroot.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Instaroot.Services;
using Instaroot.Storage.Database;
using Microsoft.AspNetCore.Identity;
using User = Instaroot.Models.User;

namespace Instaroot
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddDbContext<InstarootContext>(o =>
            {
                o.UseNpgsql(Configuration.GetConnectionString("InstarootContext"), b => b.MigrationsAssembly("Instaroot"));
            });
            services.AddEntityFrameworkNpgsql();
            services.AddDbContext<InstarootContext>();
            services.AddIdentity<User, IdentityRole>(options =>
                {
                    // Password settings
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireLowercase = false;

                    // Lockout settings
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                    options.Lockout.MaxFailedAccessAttempts = 10;

                    // Cookie settings
                    options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(15);
                    options.Cookies.ApplicationCookie.LoginPath = "/Accounts/Login";
                    options.Cookies.ApplicationCookie.LogoutPath = "/Accounts/LogOut";

                    // User settings
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<InstarootContext>();


            // Services
            services.AddScoped<IImageService, ImageService>()
                .AddScoped<ICommentService, CommentService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<ILoggingService, LoggingService>()
                .AddScoped<IFileShockerService>(c => new FileShockerService(Configuration.GetValue<string>("FileShockerUsername"), Configuration.GetValue<string>("FileShockerPassword"), Configuration.GetValue<string>("FileShockerAddress"), c.GetRequiredService<ILoggingService>()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            SetupInitial(app);
        }

        private void SetupInitial(IApplicationBuilder app)
        {
            Task.Run(async () =>
            {
                var userManager = app.ApplicationServices.GetRequiredService<UserManager<User>>();
                var roleManager = app.ApplicationServices.GetRequiredService<RoleManager<IdentityRole>>();

                if (userManager == null) throw new Exception("User manager :-(");
                if (roleManager == null) throw new Exception("Role manager :-(");

                if (!roleManager.Roles.Any(role => role.Name == "Administrator"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Administrator"));
                    var root = userManager.Users.FirstOrDefault(user => user.UserName == "instaroot");

                    if (root == null)
                    {
                        var result = await userManager.CreateAsync(new User
                        {
                            UserName = "instaroot",
                            Email = "instaroot@instaroot.com",
                        }, Configuration.GetValue<string>("RootUserPassword"));

                        root = userManager.Users.First(user => user.UserName == "instaroot");

                        if (result.Succeeded)
                        {
                            result = userManager.AddToRoleAsync(root, "Administrator").Result;

                            if (!result.Succeeded)
                            {
                                throw new Exception(":-(");
                            }
                        }
                        else
                        {
                            throw new Exception(":-(");
                        }
                    }
                    await userManager.CreateAsync(new User
                    {
                        UserName = "Alice",
                        Email = "alice@instaroot.com",
                    }, "Hi_1'm:411c3|");
                    await userManager.CreateAsync(new User
                    {
                        UserName = "Bob",
                        Email = "bob@instaroot.com",
                    }, "Hi_1'm:808|");
                    await userManager.CreateAsync(new User
                    {
                        UserName = "Claire",
                        Email = "claire@instaroot.com",
                    }, "Hi_1'm:C141r3|");

                    var context = app.ApplicationServices.GetService<InstarootContext>();

                    if (!context.Images.Any())
                    {
                        context.Images.AddRange(
                            new Image
                            {
                                Id = -3,
                                Owner = root,
                                Path = "/uploads/246e6781-a681-4f84-9dc9-2751a26877f4.jpg",
                                TimeStamp = DateTime.Now
                            },
                            new Image
                            {
                                Id = -2,
                                Owner = root,
                                Path = "/uploads/491e80ec-dd49-4788-bbb1-7c37efdaa043.jpg",
                                TimeStamp = DateTime.Now
                            },
                            new Image
                            {
                                Id = -1,
                                Owner = root,
                                Path = "/uploads/4581a1b9-6a82-4881-b775-ff75afefb259.jpg",
                                TimeStamp = DateTime.Now
                            });

                        await context.SaveChangesAsync();
                    }
                }
            }).Wait();
        }
    }
}