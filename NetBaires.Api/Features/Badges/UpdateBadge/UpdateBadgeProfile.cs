using AutoMapper;
using NetBaires.Data;
using NetBaires.Data.Entities;

namespace NetBaires.Api.Features.Badges.UpdateBadge
{
    public class UpdateBadgeProfile : Profile
    {
        public UpdateBadgeProfile()
        {
            CreateMap<UpdateBadgeCommand, Badge>().ForAllMembers(
                opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
        }
    }
}