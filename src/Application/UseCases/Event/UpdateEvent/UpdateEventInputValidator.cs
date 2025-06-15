using FluentValidation;

namespace Application.UseCases.Event.UpdateEvent
{
    public class UpdateEventInputValidator : AbstractValidator<UpdateEventInput>
    {
        public UpdateEventInputValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(200).WithMessage("O nome do evento deve ter no m�ximo 200 caracteres")
                .When(x => x.Name != null);

            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("A descri��o do evento deve ter no m�ximo 2000 caracteres")
                .When(x => x.Description != null);

            RuleFor(x => x.StartDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("A data de in�cio deve ser maior que a data atual")
                .When(x => x.StartDate.HasValue);

            RuleFor(x => x.EndDate)
                .Must((input, endDate) => !input.StartDate.HasValue || endDate > input.StartDate)
                .WithMessage("A data de t�rmino deve ser maior que a data de in�cio")
                .When(x => x.EndDate.HasValue);

            RuleFor(x => x.MinimumAge)
                .InclusiveBetween(0, 100).WithMessage("A idade m�nima deve estar entre 0 e 100 anos")
                .When(x => x.MinimumAge.HasValue);

            RuleFor(x => x.VenueId)
                .GreaterThan(0).WithMessage("O ID do local deve ser maior que zero")
                .When(x => x.VenueId.HasValue);

            RuleFor(x => x.AffiliateId)
                .GreaterThan(0).WithMessage("O ID do afiliado deve ser maior que zero")
                .When(x => x.AffiliateId.HasValue);

            RuleFor(x => x.BannerImageUrl)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .WithMessage("A URL do banner deve ser uma URL v�lida")
                .When(x => !string.IsNullOrEmpty(x.BannerImageUrl));
        }
    }
}