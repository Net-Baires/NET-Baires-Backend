using AutoMapper;
using NetBaires.Data;

namespace NetBaires.Api.Features.Badges.NewBadge
{
    public class NewBadgeGroupProfile : Profile
    {
        public NewBadgeGroupProfile()
        {
            CreateMap<NewBadgeGroupCommand, BadgeGroup>();
        }
    }
}