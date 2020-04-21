using AutoMapper;
using NetBaires.Data.Entities;

namespace NetBaires.Api.Features.Events.AddCurrentUserToGroupCode
{
    public class AddMemberToGroupCodeProfile : Profile
    {
        public AddMemberToGroupCodeProfile()
        {
            CreateMap<GroupCode, AddCurrentUserToGroupCodeCommand.Response>();
        }
    }
}
