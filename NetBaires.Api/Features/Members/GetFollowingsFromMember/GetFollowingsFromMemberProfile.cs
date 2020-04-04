using AutoMapper;
using NetBaires.Api.Features.Events.DeleteMemberToGroupCode;
using NetBaires.Data;
using NetBaires.Data.Entities;

namespace NetBaires.Api.Features.Members.GetFollowingsFromMember
{
    public class GetFollowingsFromMemberProfile : Profile
    {
        public GetFollowingsFromMemberProfile()
        {
            CreateMap<FollowingMember, GetFollowingsFromMemberQuery.Response>();
        }
    }
}
