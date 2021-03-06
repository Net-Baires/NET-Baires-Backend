﻿using AutoMapper;
using Microsoft.EntityFrameworkCore.Internal;
using NetBaires.Data;
using NetBaires.Data.Entities;

namespace NetBaires.Api.Features.Events.UpdateEvent
{
    public class UpdateEventProfile : Profile
    {
        public UpdateEventProfile()
        {
            AllowNullCollections = true;
            CreateMap<UpdateEventCommand, Event>()
                .ForMember(x => x.Live, opt => opt.Ignore())
                .ForMember(x => x.GeneralAttended, opt => opt.Ignore())
                .ForMember(x => x.Sponsors, opts => opts.PreCondition((src) => src.Sponsors != null))
                .ForAllMembers(
                opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<SponsorEventViewModel, SponsorEvent>()
                .ForAllMembers(
                    opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));


        }
    }
}