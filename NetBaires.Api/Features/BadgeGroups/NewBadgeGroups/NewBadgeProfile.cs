using AutoMapper;
using NetBaires.Data;

namespace NetBaires.Api.Features.BadgeGroups.NewBadgeGroups
{
    public class NewBadgeGroupProfile : Profile
    {
        public NewBadgeGroupProfile()
        {
            CreateMap<NewBadgeGroupCommand, BadgeGroup>();
        }
    }
}