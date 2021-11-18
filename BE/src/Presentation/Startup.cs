using Application;
using Application.Interfaces;
using Application.Manager;
using AutoMapper;
using Domain.Enums;
using Domain.Enums.Notifications;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.MemoryStorage;
using Infrastructure.Persistance;
using Infrastructure.Repository;
using Infrastructure.Repository.Implementations;
using Infrastructure.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace BookSharing
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BookSharingContext>(options => options.UseSqlServer(Configuration.GetConnectionString("BookSharingDev")))
                    .AddControllersWithViews();
            //Managers Injection
            services.AddTransient<IBookManager, BookManager>()
                    .AddTransient<IAuthorManager, AuthorManager>()
                    .AddTransient<IGenreManager, GenreManager>()
                    .AddTransient<ILanguageManager, LanguageManager>()
                    .AddTransient<IAssignmentManager, AssignmentManager>()
                    .AddTransient<IUserManager, UserManager>()
                    .AddTransient<IWishBookManager, WishBookManager>()
                    .AddTransient<IExtendManager, ExtendManager>()
                    .AddTransient<IReviewManager, ReviewManager>()
                    .AddTransient<INotificationManager, NotificationManager>()
                    .AddTransient<IJobManager, JobManager>();

            //Repositories Injection
            services.AddTransient<IBookRepository, BookRepository>()
                    .AddTransient<ILanguageRepository, LanguageRepository>()
                    .AddTransient<IAuthorRepository, AuthorRepository>()
                    .AddTransient<IGenreRepository, GenreRepository>()
                    .AddTransient<IAssignmentRepository, AssignmentRepository>()
                    .AddTransient<IUserRepository, UserRepository>()
                    .AddTransient<IWishBookRepository, WishBookRepository>()
                    .AddTransient<IExtendRepository, ExtendRepository>()
                    .AddTransient<IReviewRepository, ReviewRepository>()
                    .AddTransient<INotificationRepository, NotificationRepository>();

            services.AddHttpContextAccessor();

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddIdentity<Infrastructure.Persistance.User, Role>()
                    .AddEntityFrameworkStores<BookSharingContext>()
                    .AddTokenProvider<DataProtectorTokenProvider<Infrastructure.Persistance.User>>(TokenOptions.DefaultProvider);

            services.AddTransient<UserStoreRepository>();

            services.AddTransient<BookSharingDbSeeder>();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/Login";
                options.SlidingExpiration = true;
            });


            services.AddHangfire(c => c.UseMemoryStorage());

            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.Configure<Notifications>(Configuration.GetSection("Notifications"));
            services.Configure<Admin>(Configuration.GetSection("Admin"));
            services.Configure<Domain.Enums.Notifications.User>(Configuration.GetSection("User"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, BookSharingDbSeeder dbSeeder, IJobManager jobManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            dbSeeder.SeedUserRoles(AccessRole.SuperAdmin, AccessRole.Admin, AccessRole.User).GetAwaiter().GetResult();
            dbSeeder.SeedAdmin("Admin", "Admin", "admin", "admin", "admin@locahost.com", AccessRole.Admin, AccessRole.User).GetAwaiter().GetResult();
            dbSeeder.SeedAdmin("SuperAdmin", "SuperAdmin", "superadmin", "superadmin", "superadmin@locahost.com", AccessRole.SuperAdmin, AccessRole.Admin, AccessRole.User).GetAwaiter().GetResult();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                Authorization = new List<IDashboardAuthorizationFilter>()
            });

            RecurringJob.AddOrUpdate(() => jobManager.CheckBooksAvailability(), Cron.Daily, TimeZoneInfo.Local);
        }
    }
}
