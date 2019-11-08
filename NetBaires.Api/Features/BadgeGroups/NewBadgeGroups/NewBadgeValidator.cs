using FluentValidation;

namespace NetBaires.Api.Features.Badges.NewBadge
{
    public class NewBadgeGroupValidator : AbstractValidator<NewBadgeGroupCommand>
    {
        public NewBadgeGroupValidator()
        {
            RuleFor(x => x.Name).NotNull();
            RuleFor(x => x.Description).NotNull();
        }
    }
}