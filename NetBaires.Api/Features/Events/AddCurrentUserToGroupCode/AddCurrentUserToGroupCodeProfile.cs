﻿using AutoMapper;
using NetBaires.Api.Features.GroupsCodes.AddMemberToGroupCode;
using NetBaires.Data;

namespace NetBaires.Api.Features.Events.GetEventLiveDetail
{
    public class AddMemberToGroupCodeProfile : Profile
    {
        public AddMemberToGroupCodeProfile()
        {
            CreateMap<GroupCode, AddCurrentUserToGroupCodeCommand.Response>();
        }
    }
}