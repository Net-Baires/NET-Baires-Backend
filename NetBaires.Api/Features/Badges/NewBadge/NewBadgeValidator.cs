using FluentValidation;

namespace NetBaires.Api.Features.Badges.NewBadge
{
    public class NewBadgeValidator : AbstractValidator<NewBadgeCommand>
    {
        public NewBadgeValidator()
        {
            RuleFor(x => x.ImageFiles).NotNull();
            RuleFor(x => x.Name).NotNull();
            RuleFor(x => x.Description).NotNull();
        }
    }
}