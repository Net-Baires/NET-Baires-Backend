using System;
using System.Collections.Generic;
using NetBaires.Data;
using NetBaires.Data.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Profile = AutoMapper.Profile;

namespace NetBaires.Api.ViewModels
{
    public class EventDetail
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public EventPlatform Platform { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }
        public string EventId { get; set; }
        public bool Done { get; set; } = false;
        public bool Live { get; set; } = false;
        public DateTime Date { get; set; }
        public List<SponsorEventResponse> Sponsors { get; set; }
        public class EventDetailProfile : Profile
        {
            public EventDetailProfile()
            {
                CreateMap<Event, EventDetail>().ForAllMembers(
                    opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
                CreateMap<SponsorEvent, SponsorEventResponse>().ForAllMembers(
                    opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            }
        }
        public class SponsorEventResponse
        {
            public int SponsorId { get; set; }
            public string Detail { get; set; }

        }

    }
}
