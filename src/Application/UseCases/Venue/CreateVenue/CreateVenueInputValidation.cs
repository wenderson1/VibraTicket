using FluentValidation;

namespace Application.UseCases.Venue.CreateVenue
{
    public class CreateVenueInputValidation : AbstractValidator<CreateVenueInput>
    {
        public CreateVenueInputValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome do local é obrigatório.")
                .MaximumLength(100).WithMessage("O nome do local não pode exceder 100 caracteres.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("O endereço é obrigatório.")
                .MaximumLength(255).WithMessage("O endereço não pode exceder 255 caracteres.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("A cidade é obrigatória.")
                .MaximumLength(100).WithMessage("A cidade não pode exceder 100 caracteres.");

            RuleFor(x => x.State)
                .NotEmpty().WithMessage("O estado é obrigatório.")
                .Length(2).WithMessage("O estado deve ter exatamente 2 caracteres (UF).");

            RuleFor(x => x.ZipCode)
                .MaximumLength(10).WithMessage("O CEP não pode exceder 10 caracteres.");

            RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage("A capacidade deve ser maior que zero.");

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90).When(x => x.Latitude.HasValue)
                .WithMessage("A latitude deve estar entre -90 e 90.");

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180).When(x => x.Longitude.HasValue)
                .WithMessage("A longitude deve estar entre -180 e 180.");
        }
    }
}