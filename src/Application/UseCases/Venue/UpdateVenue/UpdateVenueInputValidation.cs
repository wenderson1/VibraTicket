using FluentValidation;

namespace Application.UseCases.Venue.UpdateVenue
{
    public class UpdateVenueInputValidation : AbstractValidator<UpdateVenueInput>
    {
        public UpdateVenueInputValidation()
        {
            RuleFor(x => x.Name)
                .MaximumLength(100)
                .When(x => x.Name != null);

            RuleFor(x => x.Address)
                .MaximumLength(200)
                .When(x => x.Address != null);

            RuleFor(x => x.City)
                .MaximumLength(100)
                .When(x => x.City != null);

            RuleFor(x => x.State)
                .Length(2)
                .When(x => x.State != null);

            RuleFor(x => x.ZipCode)
                .Matches(@"^\d{5}-?\d{3}$")
                .When(x => x.ZipCode != null)
                .WithMessage("CEP inválido. Use o formato: 00000-000 ou 00000000");

            RuleFor(x => x.Capacity)
                .GreaterThan(0)
                .When(x => x.Capacity.HasValue);

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90)
                .When(x => x.Latitude.HasValue);

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180)
                .When(x => x.Longitude.HasValue);
        }
    }
}