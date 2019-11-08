using AutoMapper;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Badges.NewBadge
{
    public class NewBadgeProfile : Profile
    {
        public NewBadgeProfile()
        {
            CreateMap<NewBadgeCommand, Badge>();
        }
    }
}