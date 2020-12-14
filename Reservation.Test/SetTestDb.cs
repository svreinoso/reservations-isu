using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reservation.Data;
using System;

namespace Reservation.Test
{
    public class SetTestDb
    {
        public static IServiceProvider GetServiceProvider()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging(config =>
            {
                config.AddDebug();
                config.AddConsole();
            });

            var applicationBuilder = new ApplicationBuilder(serviceCollection.BuildServiceProvider());

            var serviceProvider = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                .CreateScope().ServiceProvider;

            return serviceProvider;
        }

        public static ApplicationDbContext CreateContextForInMemory()
        {
            var option = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_Database").Options;

            var context = new ApplicationDbContext(option);
            if (context != null)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            return context;
        }
    }
}
