using FluentValidation;

namespace Application.UseCases.Event.CreateEvent
{
    public class CreateEventInputValidator : AbstractValidator<CreateEventInput>
    {
        public CreateEventInputValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome do evento � obrigat�rio")
                .MaximumLength(200).WithMessage("O nome do evento deve ter no m�ximo 200 caracteres");

            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("A descri��o do evento deve ter no m�ximo 2000 caracteres")
                .When(x => x.Description != null);

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("A data de in�cio do evento � obrigat�ria")
                .GreaterThan(DateTime.UtcNow).WithMessage("A data de in�cio deve ser maior que a data atual");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("A data de t�rmino do evento � obrigat�ria")
                .GreaterThan(x => x.StartDate).WithMessage("A data de t�rmino deve ser maior que a data de in�cio");

            RuleFor(x => x.MinimumAge)
                .InclusiveBetween(0, 100).WithMessage("A idade m�nima deve estar entre 0 e 100 anos");

            RuleFor(x => x.VenueId)
                .GreaterThan(0).WithMessage("O ID do local � obrigat�rio");

            RuleFor(x => x.AffiliateId)
                .GreaterThan(0).WithMessage("O ID do afiliado � obrigat�rio");

            RuleFor(x => x.BannerImageUrl)
                .MaximumLength(500).WithMessage("A URL do banner deve ter no m�ximo 500 caracteres")
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrEmpty(x.BannerImageUrl))
                .WithMessage("A URL do banner deve ser uma URL v�lida");
        }
    }
}