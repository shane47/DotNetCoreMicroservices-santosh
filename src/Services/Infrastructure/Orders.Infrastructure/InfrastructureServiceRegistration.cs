
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orders.Application.Contracts.Models.EmailService;
using Orders.Application.Contracts.Persistence;
using Orders.Infrastructure.Mail;
using Orders.Infrastructure.Persistence;
using Orders.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
                                                IConfiguration configuration)
        {
            // Note: All these dependency injection check class which inherit from their root class
            // if they find any class, it will register all the dependency(in the constructor) from the
            // inherited class and inject it in to the assembly(that is why we are passing the assembly path)
            services.AddDbContext<OrderContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("OrderConnectionString"));
            });
            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
            services.AddScoped<IOrderRepository,OrderRepository>();

            services.Configure<EmailSettings>(c=>configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailService, EmailService>();

            return services;
        }
    }
}
