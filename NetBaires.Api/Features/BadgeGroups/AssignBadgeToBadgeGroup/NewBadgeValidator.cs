using FluentValidation;

namespace NetBaires.Api.Features.BadgeGroups.AssignBadgeToBadgeGroup
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