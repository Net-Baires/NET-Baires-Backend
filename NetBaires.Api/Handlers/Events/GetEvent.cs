using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetBaires.Api.Auth;
using NetBaires.Api.Handlers.Events.Models;
using NetBaires.Api.Options;
using NetBaires.Data;
using Newtonsoft.Json.Converters;

namespace NetBaires.Api.Handlers.Events
{

    public class GetEventHandler : IRequestHandler<GetEventHandler.GetEvent, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly NetBairesContext _context;
        private readonly ILogger<UpdateEventHandler> _logger;

        public GetEventHandler(IMapper mapper,
                               NetBairesContext context,
                               ILogger<UpdateEventHandler> logger)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(GetEvent request, CancellationToken cancellationToken)
        {

            var eventToReturn = await _context.Events.Include(s => s.Sponsors).FirstOrDefaultAsync(x => x.Id == request.Id);

            if (eventToReturn == null)
                return new StatusCodeResult(404);

            return new ObjectResult(_mapper.Map(eventToReturn, new GetEventResponse())) { StatusCode = 200 };
        }


        public class GetEvent : IRequest<IActionResult>
        {
            public GetEvent(int id)
            {
                Id = id;
            }

            public int Id { get; internal set; }
        }
        public class GetEventResponse
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public EventPlatform Platform { get; set; }
            public string ImageUrl { get; set; }
            public List<SponsorEventResponse> Sponsors { get; set; }
            public string Url { get; set; }
            public string EventId { get; set; }
            public bool Done { get; set; } = false;
            public bool Live { get; set; } = false;
            public DateTime Date { get; set; }
            public GetEventResponse()
            {

            }

        }
        public class GetEventProfile : Profile
        {
            public GetEventProfile()
            {
                CreateMap<Event, GetEventResponse>();
                CreateMap<SponsorEvent, SponsorEventResponse>();

                CreateMap<SponsorEvent, GetEventResponse>();


            }
        }

    }
}