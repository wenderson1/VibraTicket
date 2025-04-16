using Application.Interfaces.Repository;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Api.Configurations
{
    public static class DependencyInjectionSetup
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            // --- Registrar Repositórios (Infrastructure) ---
            // Scoped: Uma instância por requisição HTTP. Adequado para repositórios que usam um DbContext Scoped.
            services.AddScoped<IVenueRepository, VenueRepository>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(Infrastructure.Persistence.AppDbContext).Assembly.FullName);
                }));
            // Adicione aqui os outros repositórios quando criá-los:
            // services.AddScoped<IEventRepository, EventRepository>();
            // services.AddScoped<ICustomerRepository, CustomerRepository>();
            // ... e assim por diante

            // --- Registrar Use Cases (Application) ---
            // Scoped ou Transient geralmente funcionam bem para Use Cases. Scoped se eles dependerem de serviços Scoped (como repositórios).
            // Adicione aqui os Use Cases quando criá-los:
            // services.AddScoped<ICreateVenueUseCase, CreateVenueUseCase>();
            // services.AddScoped<IGetVenueByIdUseCase, GetVenueByIdUseCase>();
            // ... e assim por diante

            // --- Registrar Validadores (Application - FluentValidation) ---
            // Quando você adicionar o FluentValidation, registre os validadores aqui.
            // Geralmente eles são registrados como Scoped ou Transient.
            // Exemplo (requer pacote FluentValidation.DependencyInjectionExtensions):
            // services.AddValidatorsFromAssembly(typeof(Application.AssemblyReference).Assembly); // Precisa de uma classe marcador no Application ou use typeof(AlgumTipoDoApplication).Assembly

            // --- Registrar Outros Serviços ---
            // Mappers (AutoMapper, Mapster), Serviços de Email, Serviços Externos, etc.
            // Exemplo com AutoMapper (requer pacote AutoMapper.Extensions.Microsoft.DependencyInjection):
            // services.AddAutoMapper(typeof(Application.AssemblyReference).Assembly);


            // Retorna services para permitir encadeamento (fluent API) se necessário
            return services;
        }
    }
}