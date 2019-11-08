using AutoMapper;
using NetBaires.Data;

namespace NetBaires.Api.Features.Badges.GetToAssign
{
    public class BadgeAssignResponseProfile : Profile
    {
        public BadgeAssignResponseProfile()
        {
            CreateMap<Badge, BadgeAssignResponse>();
        }
    }
}