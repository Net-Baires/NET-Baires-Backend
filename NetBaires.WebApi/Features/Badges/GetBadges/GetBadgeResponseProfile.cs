using AutoMapper;
using NetBaires.Data;

namespace NetBaires.Api.Features.Badges.GetBadges
{
    public class GetBadgeResponseProfile : Profile
    {
        public GetBadgeResponseProfile()
        {
            CreateMap<Badge, GetBadgeResponse>();
            CreateMap<BadgeMember, GetBadgeResponse>()
                .ForMember(dest => dest.BadgeImageUrl, opt => opt.MapFrom(src => src.BadgeUrl))
                .ForAllMembers(
                    opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<Badge, GetBadgeResponse>()
                .ForMember(dest => dest.BadgeImageUrl, opt => opt.MapFrom(src => src.ImageName))
                .ForAllMembers(
                    opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
        }
    }
}