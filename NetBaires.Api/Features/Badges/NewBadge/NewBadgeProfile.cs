using AutoMapper;
using NetBaires.Data;

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