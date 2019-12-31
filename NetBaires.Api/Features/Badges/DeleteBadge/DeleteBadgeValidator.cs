using FluentValidation;

namespace NetBaires.Api.Features.Badges.DeleteBadge
{
    public class DeleteBadgeValidator : AbstractValidator<DeleteBadgeCommand>
    {
        public DeleteBadgeValidator()
        {
            RuleFor(x => x.Id).NotNull();
        }
    }
}