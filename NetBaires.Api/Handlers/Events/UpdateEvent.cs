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
using NetBaires.Api.Handlers.Events.Models;
using NetBaires.Api.Options;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Events
{

    public class UpdateEventHandler : IRequestHandler<UpdateEventHandler.UpdateEvent, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly NetBairesContext _context;
        private readonly ILogger<UpdateEventHandler> _logger;

        public UpdateEventHandler(ICurrentUser currentUser,
            IMapper mapper,
            NetBairesContext context,
            IOptions<AssistanceOptions> assistanceOptions,
            ILogger<UpdateEventHandler> logger)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(UpdateEvent request, CancellationToken cancellationToken)
        {
            var eventToUpdate = await _context.Events.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (eventToUpdate == null)
                return new StatusCodeResult(402);

            _mapper.Map(request, eventToUpdate);


            _context.Entry(eventToUpdate).State = EntityState.Modified;

            await _context.SaveChangesAsync();


            return new ObjectResult(_mapper.Map(eventToUpdate, new EventDetail())) { StatusCode = 200 };
        }


        public class UpdateEvent : IRequest<IActionResult>
        {
            public int Id { get; set; }
            public string? Title { get; set; }
            public string? Description { get; set; }
            public string? ImageUrl { get; set; }
            public string? Url { get; set; }
            public bool? Done { get; set; } = false;
            public bool? Live { get; set; } = false;
        }
        public class UpdateEventProfile : Profile
        {
            public UpdateEventProfile()
            {
                CreateMap<UpdateEvent, Event>().ForAllMembers(
                    opt => opt.Condition((src, dest, sourceMember) => sourceMember != null)); ;
            }
        }

    }
}