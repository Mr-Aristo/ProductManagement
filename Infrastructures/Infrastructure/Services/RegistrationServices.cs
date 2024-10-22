using Application.Abstractions;
using Infrastructure.Concrete;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure.Services;

public static class RegistrationServices
{
    public static void AddInfrasturctureServices(this IServiceCollection services,IConfiguration configuration)
    {
        //var connectionString = configuration.GetConnectionString("NPSQL");
        //services.AddDbContext<ProductContext>(options => options.UseNpgsql(connectionString));

        var connectionString = configuration.GetConnectionString("MsSQL");        
        services.AddDbContext<ProductContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
    }

}
