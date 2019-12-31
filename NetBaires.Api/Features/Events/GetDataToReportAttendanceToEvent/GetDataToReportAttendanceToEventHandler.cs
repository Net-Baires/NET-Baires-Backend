using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Auth;
using NetBaires.Api.Helpers;
using NetBaires.Api.ViewModels;
using NetBaires.Data;

namespace NetBaires.Api.Features.Events.GetDataToReportAttendanceToEvent
{

    public class GetDataToReportAttendanceToEventHandler : IRequestHandler<GetDataToReportAttendanceToEventCommand, IActionResult>
    {
        private readonly IAttendanceService _attendanceService;
        private readonly IMapper _mapper;
        private readonly NetBairesContext _context;

        public GetDataToReportAttendanceToEventHandler(IAttendanceService attendanceService,
                                                       IMapper mapper,
                                                       NetBairesContext context)
        {
            _attendanceService = attendanceService;
            _mapper = mapper;
            _context = context;
        }


        public async Task<IActionResult> Handle(GetDataToReportAttendanceToEventCommand request, CancellationToken cancellationToken)
        {

            var eventToReturn = await _context.Events.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (eventToReturn == null)
                return HttpResponseCodeHelper.NotFound();

            var token = _attendanceService.GetTokenToReportMyAttendance(eventToReturn);

            var toReturn = new EventToReportAttendanceViewModel();
            _mapper.Map(eventToReturn, toReturn.EventDetail);
            toReturn.Token = token;
            return HttpResponseCodeHelper.Ok(toReturn);

        }
    }
}