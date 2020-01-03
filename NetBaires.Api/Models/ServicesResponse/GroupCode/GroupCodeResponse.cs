using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using NetBaires.Api.Features.BadgeGroups.ViewModels;
using NetBaires.Data;

namespace NetBaires.Api.Models.ServicesResponse.GroupCode
{
    public class GroupCodeResponse
    {
        public int Id { get; set; }
        public string Code { get; protected set; }
        public string Detail { get; set; }
        public bool Open { get; protected set; } = true;
        public int MembersCount { get; set; }
        public class GetBadgeGroupsProfile : Profile
        {
            public GetBadgeGroupsProfile()
            {
                CreateMap<Data.GroupCode, GroupCodeResponse>()
                    .ForMember(x => x.MembersCount, o => o.MapFrom(x => x.Members.Count));
            }
        }
    }
}
