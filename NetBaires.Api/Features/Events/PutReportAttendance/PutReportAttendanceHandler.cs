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
        private readonly ICurrentUser _currentUser;
        private readonly IAttendanceService attendanceService;

        public PutReportAttendanceHandler(NetBairesContext context,
            ICurrentUser currentUser,
            IAttendanceService attendanceService)
        {
            _context = context;
            _currentUser = currentUser;
            this.attendanceService = attendanceService;
        }


        public async Task<IActionResult> Handle(PutReportAttendanceCommand request, CancellationToken cancellationToken)
        {
            var response = attendanceService.ValidateTokenToReportMyAttendance(request.Token);
            var eventToAdd = _context.Events.Include(x => x.Attendees).FirstOrDefault(x => x.Id == response.EventId);
            var member = _context.Members.FirstOrDefault(x => x.Id == response.UserId);
            eventToAdd.Attended(member, AttendanceRegisterType.CurrentEvent);
            await _context.SaveChangesAsync();
            return HttpResponseCodeHelper.Ok(new PutReportAttendanceCommand.Response(eventToAdd.Id, member.Id));
        }
    }
}