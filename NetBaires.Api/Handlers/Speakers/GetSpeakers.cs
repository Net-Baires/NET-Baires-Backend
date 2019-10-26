using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetBaires.Api.Auth;
using NetBaires.Api.Options;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Events
{

    public class GetSpeakersHandler : IRequestHandler<GetSpeakersHandler.GetSpeakers, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly NetBairesContext _context;
        private readonly ILogger<UpdateEventHandler> _logger;

        public GetSpeakersHandler(ICurrentUser currentUser,
            IMapper mapper,
            NetBairesContext context,
            IOptions<AttendanceOptions> assistanceOptions,
            ILogger<UpdateEventHandler> logger)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(GetSpeakers request, CancellationToken cancellationToken)
        {

            var eventToReturn = _context.Members
                                        .Include(x => x.Events)
                                        .Where(x => x.Events.Any(s => s.Speaker))
                                        .ToList();

            if (!eventToReturn.Any())
                return new StatusCodeResult(204);

            return new ObjectResult(_mapper.Map(eventToReturn, new List<GetSpeakersResponse>())) { StatusCode = 200 };
        }


        public class GetSpeakers : IRequest<IActionResult>
        {

        }
        public class GetSpeakersResponse
        {
            public int MemberId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Twitter { get; set; }
            public string Instagram { get; set; }
            public string Linkedin { get; set; }
            public string Github { get; set; }
            public string Picture { get; set; }
        }
        public class GetSpeakersProfile : Profile
        {
            public GetSpeakersProfile()
            {
                CreateMap<MemberAsSpeaker, GetSpeakersResponse>();
                CreateMap<Member, GetSpeakersResponse>();

            }
        }
        class MemberAsSpeaker
        {
            public Member Member { get; set; }
            public int CounEventsAsSpeaker { get; set; }

        }

    }
}