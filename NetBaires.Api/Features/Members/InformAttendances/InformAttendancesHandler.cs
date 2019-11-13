using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Features.Badges.Models;
using NetBaires.Api.Helpers;
using NetBaires.Data;

namespace NetBaires.Api.Features.Badges.AssignMembersToBadge
{
    public class InformAttendancesHandler : IRequestHandler<InformAttendancesCommand, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;

        public InformAttendancesHandler(NetBairesContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<IActionResult> Handle(InformAttendancesCommand request, CancellationToken cancellationToken)
        {
            var eventToUpdate = await _context.Events.Include(x => x.Attendees).FirstOrDefaultAsync(x => x.Id == request.EventId);
            if (eventToUpdate == null)
                return HttpResponseCodeHelper.NotFound();

            var member = await _context.Members.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (member == null)
                return HttpResponseCodeHelper.NotFound();

            var attendance = eventToUpdate.AddAttendance(member);
            if (request.Attended)
                attendance.Attend();
            else
                attendance.NoAttend();

            await _context.SaveChangesAsync();

            return HttpResponseCodeHelper.NotContent();
        }
    }
}