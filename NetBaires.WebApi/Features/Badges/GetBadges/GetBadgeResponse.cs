using System;

namespace NetBaires.Api.Features.Badges.GetBadges
{
    public class GetBadgeResponse
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string BadgeImageUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BadgeUrl { get; set; }
    }
}