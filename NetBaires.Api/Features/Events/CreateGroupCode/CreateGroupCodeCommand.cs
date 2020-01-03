using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetBaires.Api.Features.Events.UpdateEvent;
using NetBaires.Api.Services.EventBrite.Models;
using NetBaires.Data;
using Newtonsoft.Json;
using Profile = AutoMapper.Profile;

namespace NetBaires.Api.Features.Events.PutReportAttendance
{
    public class CreateGroupCodeCommand : IRequest<IActionResult>
    {
        [JsonIgnore]
        public int EventId { get; set; }
        public string Detail { get; set; }


        public class Response
        {
            public int Id { get; set; }
            public string Code { get;  set; }
            public string Detail { get; set; }
            public bool Open { get;  set; } 
        }
        
    }
    public class CreateGroupCodeProfile : Profile
    {
        public CreateGroupCodeProfile()
        {
            CreateMap<GroupCode, CreateGroupCodeCommand.Response>();
        }
    }
}