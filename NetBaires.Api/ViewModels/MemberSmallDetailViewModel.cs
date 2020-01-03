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
                CreateMap<Member, MemberSmallDetailViewModel>();
            }
        }
    }
}