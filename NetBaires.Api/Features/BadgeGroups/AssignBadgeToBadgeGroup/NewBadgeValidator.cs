using FluentValidation;

namespace NetBaires.Api.Features.Badges.NewBadge
{
    public class AssignBadgeToBadgeGroupValidator : AbstractValidator<AssignBadgeToBadgeGroupCommand>
    {
        public AssignBadgeToBadgeGroupValidator()
        {
            RuleFor(x => x.BadgeId).NotNull();
            RuleFor(x => x.BadgeGroupId).NotNull();
        }
    }
}