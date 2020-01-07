using AutoMapper;
using NetBaires.Data;

namespace NetBaires.Api.ViewModels
{
    public class MemberSmallDetailViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
       
        public string Picture { get; set; }
        public class MemberSmallDetailViewModelProfile : Profile
        {
            public MemberSmallDetailViewModelProfile()
            {
                CreateMap<GroupCodeMember, MemberSmallDetailViewModel>()
                    .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Member.Id))
                    .ForMember(d => d.FirstName, opt => opt.MapFrom(s => s.Member.FirstName))
                    .ForMember(d => d.LastName, opt => opt.MapFrom(s => s.Member.LastName))
                    .ForMember(d => d.Picture, opt => opt.MapFrom(s => s.Member.Picture));
            }
        }
    }
}