using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pos.Application.Interfaces.Infrastructure;
using Pos.Domain.Interfaces.Repositories;
using Pos.Infrastructure.Data;
using Pos.Infrastructure.Notifications;
using Pos.Infrastructure.Repositories;

namespace Pos.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PosDb");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string 'PosDb' was not found.");
        }

        services.AddDbContext<PosDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<ITransactionalExecutor, TransactionalExecutor>();
        services.AddScoped<IShiftSummaryNotifier, NoOpShiftSummaryNotifier>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISaleRepository, SaleRepository>();
        services.AddScoped<ISaleItemRepository, SaleItemRepository>();
        services.AddScoped<ICashFlowRepository, CashFlowRepository>();
        services.AddScoped<ICashBoxRepository, CashBoxRepository>();
        services.AddScoped<IReturnRepository, ReturnRepository>();
        services.AddScoped<IUserActivityLogRepository, UserActivityLogRepository>();

        return services;
    }
}
