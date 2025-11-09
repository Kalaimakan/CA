using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentValidation;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Behaviors;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // AutoMapper-ஐ Register செய்தல் (சரிசெய்யப்பட்ட முறை)
            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(Assembly.GetExecutingAssembly());
            });

            // FluentValidation-ஐ Register செய்தல்
            // இது வேலை செய்ய NuGet Package தேவை (கீழே பார்க்கவும்)
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // MediatR-ஐ Register செய்தல்
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            // MediatR Pipeline Behaviors (Validation-ஐ தானாக இயக்க)
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            return services;
        }
    }
}
