using Ditransa.Application.Interfaces.Repositories;
using Ditransa.Application.Interfaces.Repositories.Evidences;
using Ditransa.Application.Interfaces.Repositories.Inspections;
using Ditransa.Application.Interfaces.Repositories.Menu;
using Ditransa.Application.Interfaces.Repositories.MenuRol;
using Ditransa.Application.Interfaces.Repositories.Users;
using Ditransa.Persistence.Contexts;
using Ditransa.Persistence.Repositories;
using Ditransa.Persistence.Repositories.Evidences;
using Ditransa.Persistence.Repositories.Inspections;
using Ditransa.Persistence.Repositories.Menu;
using Ditransa.Persistence.Repositories.MenuRol;
using Ditransa.Persistence.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ditransa.Persistence.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddMappings();
            services.AddDbContext(configuration);
            services.AddRepositories();
        }

        //private static void AddMappings(this IServiceCollection services)
        //{
        //    services.AddAutoMapper(Assembly.GetExecutingAssembly());
        //}

        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            /*var host = configuration["RDS_BACKEND_HOST"];
            var port = configuration["RDS_BACKEND_PORT"];
            var database = configuration["RDS_BACKEND_DATABASE"];
            var username = configuration["RDS_BACKEND_USERNAME"];
            var password = configuration["RDS_BACKEND_PASSWORD"];

            var connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password}";*/
            var connectionString = configuration.GetConnectionString("SqlConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    connectionString,
                    sqlServerOptions =>
                    {
                        sqlServerOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                        sqlServerOptions.CommandTimeout(200); 
                    }));

        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services
                .AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork))
                .AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>))
                .AddTransient<IUserRepository, UserRepository>()
                .AddTransient<IMenuRepository, MenuRepository>()
                .AddTransient<IMenuRolRepository, MenuRolRepository>()
                .AddTransient<IInspectionRepository, InspectionRepository>()
                .AddTransient<IEvidencesRepository, EvidencesRepository>()
                ;
        }
    }
}