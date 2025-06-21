using Application.Interfaces;
using Application.Query.Venue.GetVenueById;
using Application.UseCases.Venue.CreateVenue;
using Application.UseCases.Venue.DeleteVenue;
using Application.UseCases.Venue.UpdateVenue;
using FluentValidation;
using Infrastructure.Persistence;
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

            services.AddValidatorsFromAssembly(typeof(UpdateVenueInputValidation).Assembly);
            // --- Registrar Use Cases (Application) ---

            services.AddScoped<ICreateVenueUseCase, CreateVenueUseCase>();
            services.AddScoped<IGetVenueByIdQuery, GetVenueByIdQuery>();
            services.AddScoped<IUpdateVenueUseCase, UpdateVenueUseCase>();
            services.AddScoped<IDeleteVenueUseCase, DeleteVenueUseCase>();

            // --- Event Use Cases ---
            services.AddScoped<Application.UseCases.Event.CreateEvent.ICreateEventUseCase, Application.UseCases.Event.CreateEvent.CreateEventUseCase>();
            services.AddScoped<Application.UseCases.Event.UpdateEvent.IUpdateEventUseCase, Application.UseCases.Event.UpdateEvent.UpdateEventUseCase>();
            services.AddScoped<Application.UseCases.Event.DeleteEvent.IDeleteEventUseCase, Application.UseCases.Event.DeleteEvent.DeleteEventUseCase>();
            services.AddScoped<Application.Query.Event.GetEventById.IGetEventByIdQuery, Application.Query.Event.GetEventById.GetEventByIdQuery>();
            services.AddScoped<Application.Query.Event.GetEventByTicketId.IGetEventByTicketIdQuery, Application.Query.Event.GetEventByTicketId.GetEventByTicketIdQuery>();

            // --- Order Use Cases ---
            services.AddScoped<Application.UseCases.Order.CreateOrder.ICreateOrderUseCase, Application.UseCases.Order.CreateOrder.CreateOrderUseCase>();

            // --- Payment Use Cases ---
            services.AddScoped<Application.UseCases.Payment.CreatePayment.ICreatePaymentUseCase, Application.UseCases.Payment.CreatePayment.CreatePaymentUseCase>();

            // --- Customer Use Cases & Queries ---
            services.AddScoped<Application.UseCases.Customer.CreateCustomer.ICreateCustomerUseCase, Application.UseCases.Customer.CreateCustomer.CreateCustomerUseCase>();
            services.AddScoped<Application.UseCases.Customer.UpdateCustomer.IUpdateCustomerUseCase, Application.UseCases.Customer.UpdateCustomer.UpdateCustomerUseCase>();
            services.AddScoped<Application.UseCases.Customer.DeleteCustomer.IDeleteCustomerUseCase, Application.UseCases.Customer.DeleteCustomer.DeleteCustomerUseCase>();
            services.AddScoped<Application.Query.Customer.GetCustomerById.IGetCustomerByIdQuery, Application.Query.Customer.GetCustomerById.GetCustomerByIdQuery>();
            services.AddScoped<Application.Query.Customer.GetCustomerByDocument.IGetCustomerByDocumentQuery, Application.Query.Customer.GetCustomerByDocument.GetCustomerByDocumentQuery>();
            services.AddScoped<Application.Query.Customer.GetCustomerByEmail.IGetCustomerByEmailQuery, Application.Query.Customer.GetCustomerByEmail.GetCustomerByEmailQuery>();
            
            // --- Affiliate Use Cases & Queries ---
            services.AddScoped<Application.Query.Affiliate.GetAffiliateById.IGetAffiliateByIdQuery, Application.Query.Affiliate.GetAffiliateById.GetAffiliateByIdQuery>();
            services.AddScoped<Application.Query.Affiliate.GetAffiliateByDocument.IGetAffiliateByDocumentQuery, Application.Query.Affiliate.GetAffiliateByDocument.GetAffiliateByDocumentQuery>();
            services.AddScoped<Application.UseCases.Affiliate.CreateAffiliate.ICreateAffiliateUseCase, Application.UseCases.Affiliate.CreateAffiliate.CreateAffiliateUseCase>();
            services.AddScoped<Application.UseCases.Affiliate.UpdateAffiliate.IUpdateAffiliateUseCase, Application.UseCases.Affiliate.UpdateAffiliate.UpdateAffiliateUseCase>();
            services.AddScoped<Application.UseCases.Affiliate.DeleteAffiliate.IDeleteAffiliateUseCase, Application.UseCases.Affiliate.DeleteAffiliate.DeleteAffiliateUseCase>();

            // --- Sector Use Cases ---
            services.AddScoped<Application.UseCases.Sector.CreateSector.ICreateSectorUseCase, Application.UseCases.Sector.CreateSector.CreateSectorUseCase>();
            services.AddScoped<Application.UseCases.Sector.UpdateSector.IUpdateSectorUseCase, Application.UseCases.Sector.UpdateSector.UpdateSectorUseCase>();

            services.AddScoped<IUpdateVenueUseCase, UpdateVenueUseCase>();

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