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

    public class GetEventsHandler : IRequestHandler<GetEventsHandler.GetEvents, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly NetBairesContext _context;
        private readonly ILogger<UpdateEventHandler> _logger;

        public GetEventsHandler(ICurrentUser currentUser,
            IMapper mapper,
            NetBairesContext context,
            IOptions<AttendanceOptions> assistanceOptions,
            ILogger<UpdateEventHandler> logger)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(GetEvents request, CancellationToken cancellationToken)
        {

            var eventToReturn = _context.Events.OrderByDescending(x => x.Id).AsNoTracking();

            if (!eventToReturn.Any())
                return new StatusCodeResult(204);

            return new ObjectResult(_mapper.Map(eventToReturn, new List<GetEventsResponse>())) { StatusCode = 200 };
        }


        public class GetEvents : IRequest<IActionResult>
        {

        }
        public class GetEventsResponse
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
        }
        public class GetEventsProfile : Profile
        {
            public GetEventsProfile()
            {
                CreateMap<Event, GetEventsResponse>();

            }
        }

    }
}