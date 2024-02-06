using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Orders.Application.Behaviours;
using System.Reflection;

namespace Orders.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Note: All these dependency injection check class which inherit from their root class
            // if they find any class, it will register all the dependency(in the constructor) from the
            // inherited class and inject it in to the assembly(that is why we are passing the assembly path)
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            // Now pipeline behaviour
            // First request will go through UnhandledExceptionBehaviour
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            // then it will pass through ValidationBehaviour 
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            return services;
        }
    }
}
