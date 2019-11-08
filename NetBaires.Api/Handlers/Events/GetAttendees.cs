using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Auth;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Events
{

    public class GetAttendeesHandler : IRequestHandler<GetAttendeesHandler.GetAttendees, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly NetBairesContext _context;
        private readonly ILogger<UpdateEventHandler> _logger;

        public GetAttendeesHandler(ICurrentUser currentUser,
            IMapper mapper,
            NetBairesContext context,
            ILogger<UpdateEventHandler> logger)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(GetAttendees request, CancellationToken cancellationToken)
        {
            var attendees = await _context.Attendances
                                        .Include(x => x.Member)
                                        .Where(x => x.EventId == request.Id)
                                        .ToListAsync();
            if (attendees == null || !attendees.Any())
                return new StatusCodeResult(204);

            return new ObjectResult(_mapper.Map(attendees, new List<GetAttendeesResponse>())) { StatusCode = 200 };
        }


        public class GetAttendees : IRequest<IActionResult>
        {
            public GetAttendees(int id)
            {
                Id = id;
            }

            public int Id { get; }

        }
        public class GetAttendeesResponse
        {
            public int Id { get; set; }
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Picture { get; set; }
            public bool DidNotAttend { get; set; }
            public bool Attended { get; set; }
            public bool NotifiedAbsence { get; set; }
            public bool DoNotKnow { get; set; }
            public bool Organizer { get; set; }
            public bool Speaker { get; set; }
        }
        public class GetAttendeesProfile : Profile
        {
            public GetAttendeesProfile()
            {
                CreateMap<Attendance, GetAttendeesResponse>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Member.Email))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Member.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Member.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Member.LastName))
                .ForMember(dest => dest.Picture, opt => opt.MapFrom(src => src.Member.Picture));

            }

        }

    }
}