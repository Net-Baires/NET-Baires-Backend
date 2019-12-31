using AutoMapper;
using NetBaires.Data;

namespace NetBaires.Api.Features.BadgeGroups.ViewModels
{
    public class BadgeGroupDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Badges { get; set; }
        public BadgeGroupDetailViewModel(BadgeGroup badgeGroup, int badgeCount)
        {
            Id = badgeGroup.Id;
            Name = badgeGroup.Name;
            Description = badgeGroup.Description;
            Badges = badgeCount;
        }
        public BadgeGroupDetailViewModel()
        {

        }

        public class GetBadgeGroupsProfile : Profile
        {
            public GetBadgeGroupsProfile()
            {
                CreateMap<BadgeGroup, BadgeGroupDetailViewModel>();
            }
        }
    }
}