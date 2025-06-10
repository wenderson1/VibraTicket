using Application.Interfaces;
using Application.Interfaces.Repository;
using Application.Query.Venue.GetVenueById;
using Application.UseCases.Venue.CreateVenue;
using Application.UseCases.Venue.DeleteVenue;
using Application.UseCases.Venue.UpdateVenue;
using FluentValidation;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Api.Configurations
{
    public static class DependencyInjectionSetup
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString,
                    sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(Infrastructure.Persistence.AppDbContext).Assembly
                            .FullName);
                    }));

            // --- Registrar Unit of Work e Repositórios (Infrastructure) ---
            services.AddScoped<IUnitOfWork, UnitOfWork>(); // Registra a Unit of Work
            // Não precisamos mais registrar IVenueRepository explicitamente se acessarmos via IUnitOfWork
            // services.AddScoped<IVenueRepository, VenueRepository>();


            // --- Registrar Use Cases (Application) ---
            services.AddScoped<ICreateVenueUseCase, CreateVenueUseCase>();
            services.AddScoped<IGetVenueByIdQuery, GetVenueByIdQuery>();
            services.AddScoped<IUpdateVenueUseCase, UpdateVenueUseCase>();
            services.AddScoped<IDeleteVenueUseCase, DeleteVenueUseCase>();

            // --- Customer Use Cases & Queries ---
            services.AddScoped<Application.UseCases.Customer.CreateCustomer.ICreateCustomerUseCase, Application.UseCases.Customer.CreateCustomer.CreateCustomerUseCase>();
            services.AddScoped<Application.UseCases.Customer.UpdateCustomer.IUpdateCustomerUseCase, Application.UseCases.Customer.UpdateCustomer.UpdateCustomerUseCase>();
            services.AddScoped<Application.Query.Customer.GetCustomerById.IGetCustomerByIdQuery, Application.Query.Customer.GetCustomerById.GetCustomerByIdQuery>();
            services.AddScoped<Application.Query.Customer.GetCustomerByDocument.IGetCustomerByDocumentQuery, Application.Query.Customer.GetCustomerByDocument.GetCustomerByDocumentQuery>();
            services.AddScoped<Application.Query.Customer.GetCustomerByEmail.IGetCustomerByEmailQuery, Application.Query.Customer.GetCustomerByEmail.GetCustomerByEmailQuery>();
            // Adicione outros Use Cases aqui...


            // --- Registrar Validadores (Application - FluentValidation) ---
            // Esta linha busca por todas as classes que herdam de AbstractValidator
            // no assembly onde CreateVenueInputValidation (ou qualquer outra classe do Application) reside
            // e as registra automaticamente no container de DI.
            services.AddValidatorsFromAssembly(typeof(CreateVenueInputValidation).Assembly, ServiceLifetime.Scoped);


            // --- Registrar Outros Serviços ---
            // Mappers, etc.

            return services;
        }
    }
}