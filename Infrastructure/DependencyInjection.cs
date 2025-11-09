using Application.Common.Interfaces;
using Infrastructure.Identity;
using Infrastructure.Identity.Jwt;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // EF Core (In-Memory Database-ஐ உதாரணத்திற்காக பயன்படுத்துகிறேன்)
            //SQL Server-க்கு:
            services.AddDbContext<AppDbContext>(options =>
     options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
         b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

            //services.AddDbContext<AppDbContext>(options =>
            //    options.UseInMemoryDatabase("CleanArchDb"));

            // Services-ஐ Register செய்தல்
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddSingleton<IJwtService, JwtService>();
            // services.AddTransient<IEmailService, EmailService>();

            return services;
        }
    }
}
