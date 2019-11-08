using AutoMapper;
using NetBaires.Data;

namespace NetBaires.Api.ViewModels
{
    public class MemberDetailViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Twitter { get; set; }
        public string WorkPosition { get; set; }

        public string Instagram { get; set; }
        public string Linkedin { get; set; }
        public string Github { get; set; }
        public string Biography { get; set; }
        public string Picture { get; set; }
        public string PictureName { get; set; }
        public bool Blocked { get; set; }
        public bool Organized { get; set; } = false;
        public bool Colaborator { get; set; } = false;
        public class MemberDetailViewModelProfile : Profile
        {
            public MemberDetailViewModelProfile()
            {
                CreateMap<Member, MemberDetailViewModel>();
            }
        }
    }
}
