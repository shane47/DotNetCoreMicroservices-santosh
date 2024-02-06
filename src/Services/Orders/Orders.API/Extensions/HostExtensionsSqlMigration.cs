using AutoMapper.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Orders.API.Extensions
{
    public static class HostExtensionsSqlMigration
    {
        public static IHost MigrateSqlDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder,  
                                        int? retry = 0) where TContext : DbContext
        {
            int retryForAvailability = retry.Value;

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetService<TContext>();

            try
            {
                logger.LogInformation("Migrating SQL database.");
                InvokeSeeder(seeder, context, services);
                logger.LogInformation("Migrated SQL database.");
            }
            catch (SqlException ex)
            {
                logger.LogError(ex, "An error occurred while migrating the postresql database");

                if (retryForAvailability < 50)
                {
                    retryForAvailability++;
                    System.Threading.Thread.Sleep(2000);
                    MigrateSqlDatabase<TContext>(host, seeder, retryForAvailability);
                }
                else
                    throw;
            }
            return host;
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services) 
                where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}
