using AutoMapper;
using NetBaires.Api.Features.Badges.GetBadge;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Events
{
    public class AddMemberProfile : Profile
    {
        public AddMemberProfile()
        {
            CreateMap<AddMemberCommand, Member>();
        }
    }
}