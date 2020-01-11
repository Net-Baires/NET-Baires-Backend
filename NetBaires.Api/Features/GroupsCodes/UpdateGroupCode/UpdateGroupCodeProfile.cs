using AutoMapper;
using NetBaires.Data;

namespace NetBaires.Api.Features.GroupsCodes.UpdateGroupCode
{
    public class UpdateGroupCodeProfile : Profile
    {
        public UpdateGroupCodeProfile()
        {
            CreateMap<UpdateGroupCodeCommand, GroupCode>()
                .ForAllMembers(
                    opt => opt.Condition((src, dest, sourceMember) => sourceMember != null)); ;
        }
    }
}