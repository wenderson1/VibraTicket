using FluentValidation;

namespace Application.UseCases.Event.CreateEvent
{
    public class CreateEventInputValidator : AbstractValidator<CreateEventInput>
    {
        public CreateEventInputValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome do evento é obrigatório")
                .MaximumLength(200).WithMessage("O nome do evento deve ter no máximo 200 caracteres");

            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("A descrição do evento deve ter no máximo 2000 caracteres")
                .When(x => x.Description != null);

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("A data de início do evento é obrigatória")
                .GreaterThan(DateTime.UtcNow).WithMessage("A data de início deve ser maior que a data atual");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("A data de término do evento é obrigatória")
                .GreaterThan(x => x.StartDate).WithMessage("A data de término deve ser maior que a data de início");

            RuleFor(x => x.MinimumAge)
                .InclusiveBetween(0, 100).WithMessage("A idade mínima deve estar entre 0 e 100 anos");

            RuleFor(x => x.VenueId)
                .GreaterThan(0).WithMessage("O ID do local é obrigatório");

            RuleFor(x => x.AffiliateId)
                .GreaterThan(0).WithMessage("O ID do afiliado é obrigatório");

            RuleFor(x => x.BannerImageUrl)
                .MaximumLength(500).WithMessage("A URL do banner deve ter no máximo 500 caracteres")
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrEmpty(x.BannerImageUrl))
                .WithMessage("A URL do banner deve ser uma URL válida");
        }
    }
}