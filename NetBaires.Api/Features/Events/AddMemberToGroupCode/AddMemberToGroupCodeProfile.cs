using AutoMapper;
using NetBaires.Data;

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
