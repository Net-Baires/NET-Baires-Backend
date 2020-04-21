using AutoMapper;
using NetBaires.Data;
using NetBaires.Data.Entities;

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
