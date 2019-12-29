using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Auth;
using NetBaires.Api.Helpers;
using NetBaires.Data;

namespace NetBaires.Api.Features.Events.PutReportAttendance
{

    public class PutReportAttendanceHandler : IRequestHandler<PutReportAttendanceCommand, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IAttendanceService attendanceService;

        public PutReportAttendanceHandler(NetBairesContext context,
            IAttendanceService attendanceService)
        {
            _context = context;
            this.attendanceService = attendanceService;
        }


        public async Task<IActionResult> Handle(PutReportAttendanceCommand request, CancellationToken cancellationToken)
        {
            var response = attendanceService.ValidateTokenToReportMyAttendance(request.Token);
            var eventToAdd = _context.Events.Include(x => x.Attendees).FirstOrDefault(x => x.Id == response.EventId);
            var member = _context.Members.FirstOrDefault(x => x.Id == response.UserId);
            eventToAdd.Attended(member);
            await _context.SaveChangesAsync();
            return HttpResponseCodeHelper.NotContent();
        }
    }
}