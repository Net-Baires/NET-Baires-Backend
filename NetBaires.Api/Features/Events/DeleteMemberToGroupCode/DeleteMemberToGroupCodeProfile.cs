using AutoMapper;
using NetBaires.Data;

namespace NetBaires.Api.Features.Events.DeleteMemberToGroupCode
{
    public class DeleteMemberToGroupCodeProfile : Profile
    {
        public DeleteMemberToGroupCodeProfile()
        {
            CreateMap<GroupCode, DeleteMemberToGroupCodeCommand.Response>();
        }
    }
}
