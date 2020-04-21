using AutoMapper;
using NetBaires.Data;
using NetBaires.Data.Entities;

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