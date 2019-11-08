using System;

namespace NetBaires.Api.Features.Badges.GetToAssign
{
    public class BadgeAssignResponse
    {
        public int Id { get; set; }
        public string BadgeUrl { get; set; }
        public string BadgeImageUrl { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public bool Assigned { get; set; }
    }
}