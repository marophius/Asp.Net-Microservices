using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Ordering.Infrastructure.Persistence;

namespace Ordering.API.Extensions
{
    public static class DatabaseMigrationExtension
    {
        public static WebApplication MigrateDatabase<TContext>(this WebApplication app, Action<TContext, IServiceProvider> seeder, int retryForAvailability = 0) where TContext : DbContext
        {
            using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
            {
                
                var context = serviceScope.ServiceProvider.GetService<TContext>();
                var logger = serviceScope.ServiceProvider.GetService<ILogger<OrderContext>>();
                var services = serviceScope.ServiceProvider;
                try
                {
                    logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(OrderContext).Name);

                    InvokeSeeder<TContext>(seeder, context, services);
                    logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(OrderContext).Name);
                }
                catch (SqlException ex)
                {
                    logger.LogError(ex, "Something wrong happened while migrating the database");
                    if(retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        Thread.Sleep(2000);
                        MigrateDatabase<TContext>(app, seeder, retryForAvailability);
                    }
                }
                
            }

            return app;
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, 
                                                    TContext context, 
                                                    IServiceProvider services) where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}
