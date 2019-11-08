using AutoMapper;
using NetBaires.Data;

namespace NetBaires.Api.Features.Badges.GetBadge
{
    public class GetBadgeResponseProfile : Profile
    {
        public GetBadgeResponseProfile()
        {
            CreateMap<Badge, GetBadgeResponse>();
        }
    }
}