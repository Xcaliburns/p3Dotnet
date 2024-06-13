
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P3AddNewFunctionalityDotNetCore;
using P3AddNewFunctionalityDotNetCore.Data;

namespace P3DotNetCore.Tests.Integration
{
    public class CustomWebApplicationFactory<Program>
    : WebApplicationFactory<Program> where Program : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<DbContext>));
              

                services.Remove(dbContextDescriptor);

                // Nouveau DbContext avec la configuration souhaitée
                services.AddDbContext<P3Referential>(options =>
                    options.UseInMemoryDatabase("TestDb"));

                //var dbConnectionDescriptor = services.SingleOrDefault(
                //    d => d.ServiceType ==
                //        typeof(DbConnection));
                // Utiliser une db en dur pour les tests
                //               services.AddSingleton<DbConnection>(container =>
                //               {
                //                   //var connection = new SqlConnection("Server=.;Database=integration_tests;User Id=699cc797-fc68-4207-bf21-97c219e6f255;Password=P@ssword123;");
                //                   //connection.Open();
                //                   var connection = new SqlConnection("Server=.;Database=integration_tests;Trusted_Connection=True;MultipleActiveResultSets=true");
                //                   connection.Open();

                //                   return connection;
                //               });

                //               services.AddDbContext<P3Referential>((container, options) =>
                //{
                //    var connection = container.GetRequiredService<DbConnection>();
                //    options.UseSqlServer(connection);
                //});
            });

            builder.UseEnvironment("Development");
        }
    }
}
