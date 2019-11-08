using System.IO;
using MediatR;

namespace NetBaires.Api.Features.Badges.GetImage
{
    public class GetIamgeCommand : IRequest<Stream>
    {
        public GetIamgeCommand(int badgeId)
        {
            BadgeId = badgeId;
        }

        public int BadgeId { get; }
    }
}