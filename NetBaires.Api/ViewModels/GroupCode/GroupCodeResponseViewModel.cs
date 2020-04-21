using AutoMapper;

namespace NetBaires.Api.ViewModels.GroupCode
{
    public class GroupCodeResponseViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Detail { get; set; }
        public bool Open { get; protected set; }
        public int MembersCount { get; set; }
        public class GetBadgeGroupsProfile : Profile
        {
            public GetBadgeGroupsProfile()
            {
                CreateMap<Data.Entities.GroupCode, GroupCodeResponseViewModel>()
                    .ForMember(x => x.MembersCount, o => o.MapFrom(x => x.Members.Count));
            }
        }
    }
}
