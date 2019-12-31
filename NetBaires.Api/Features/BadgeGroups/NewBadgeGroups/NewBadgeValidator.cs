using FluentValidation;

namespace NetBaires.Api.Features.BadgeGroups.NewBadgeGroups
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