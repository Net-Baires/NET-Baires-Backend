using AutoMapper;
using NetBaires.Data;
using NetBaires.Data.Entities;

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