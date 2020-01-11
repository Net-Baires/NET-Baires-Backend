using System.Collections.Generic;
using AutoMapper;

namespace NetBaires.Api.ViewModels.GroupCode
{
    public class GroupCodeFullDetailResponseViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Detail { get; set; }
        public bool Open { get; protected set; }
        public List<MemberSmallDetailViewModel> Members { get; set; }
        public List<BadgeDetailViewModel> Badges { get; set; }
        public class GetBadgeGroupsProfile : Profile
        {
            public GetBadgeGroupsProfile()
            {
                CreateMap<Data.GroupCode, GroupCodeFullDetailResponseViewModel>()
                    .ForMember(dest => dest.Badges, opt => opt.MapFrom(src => src.GroupCodeBadges))

                        ;
            }
        }
    }
}