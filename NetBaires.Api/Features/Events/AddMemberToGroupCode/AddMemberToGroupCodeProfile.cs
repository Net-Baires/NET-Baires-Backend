using AutoMapper;
using NetBaires.Data;
using NetBaires.Data.Entities;

namespace NetBaires.Api.Features.Events.AddMemberToGroupCode
{
    public class AddMemberToGroupCodeProfile : Profile
    {
        public AddMemberToGroupCodeProfile()
        {
            CreateMap<GroupCode, AddMemberToGroupCodeCommand.Response>();
        }
    }
}
