using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Infrastructure.Context;

namespace Infrastructure
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ProductContext>
    {
        public ProductContext CreateDbContext(string[] args)
        {


           
            
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) 
                .AddJsonFile("appsettings.json")              
                .Build();

            //var connectionString = configuration.GetConnectionString("NPSQL");
            var connectionString = configuration.GetConnectionString("MsSQL");

            DbContextOptionsBuilder<ProductContext> dbContextOptions = new();

            
          // dbContextOptions.UseNpgsql(connectionString);            
           dbContextOptions.UseSqlServer(connectionString);

            return new ProductContext(dbContextOptions.Options);

        }
    }
}
