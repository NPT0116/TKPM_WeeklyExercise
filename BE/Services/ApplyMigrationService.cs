using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BE.Data;
using BE.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BE.Services
{
    public class ApplyMigrationService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ApplyMigrationService> _logger;

        public ApplyMigrationService(IServiceProvider serviceProvider, ILogger<ApplyMigrationService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();

            try
            {
                _logger.LogInformation("Checking for pending migrations...");
                var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(cancellationToken);
                if (pendingMigrations.Any())
                {
                    _logger.LogInformation("Applying pending migrations...");
                    await dbContext.Database.MigrateAsync(cancellationToken);
                    _logger.LogInformation("Migrations applied successfully.");
                }
                else
                {
                    _logger.LogInformation("No pending migrations found.");
                }

                // Seed dữ liệu vào database
                _logger.LogInformation("Seeding data...");
                await  SeedData.InitializeAsync(dbContext, env, _logger);
                _logger.LogInformation("Data seeded successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while applying migrations and seeding data");
                throw;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
