using AutoMapper;
using NetBaires.Data;

namespace NetBaires.Api.Features.Members.AddMember
{
    public class AddMemberProfile : Profile
    {
        public AddMemberProfile()
        {
            CreateMap<AddMemberCommand, Member>();
        }
    }
}