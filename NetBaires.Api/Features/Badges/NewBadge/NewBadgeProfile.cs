using AutoMapper;
using NetBaires.Data;
using NetBaires.Data.Entities;

namespace NetBaires.Api.Features.Badges.NewBadge
{
    public class NewBadgeProfile : Profile
    {
        public NewBadgeProfile()
        {
            CreateMap<NewBadgeCommand, Badge>();
        }
    }
}