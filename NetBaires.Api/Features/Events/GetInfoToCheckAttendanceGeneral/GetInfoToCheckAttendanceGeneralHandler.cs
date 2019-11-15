using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Auth;
using NetBaires.Api.Helpers;
using NetBaires.Api.Models;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Events
{

    public class GetInfoToCheckAttendanceGeneralHandler : IRequestHandler<GetInfoToCheckAttendanceGeneralCommand, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IAttendanceService _attendanceService;
        private readonly IMapper _mapper;

        public GetInfoToCheckAttendanceGeneralHandler(NetBairesContext context,
            IAttendanceService attendanceService,
            IMapper mapper)
        {
            _context = context;
            _attendanceService = attendanceService;
            _mapper = mapper;
        }


        public async Task<IActionResult> Handle(GetInfoToCheckAttendanceGeneralCommand request, CancellationToken cancellationToken)
        {
            var eventToReturn = await _context.Events.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (eventToReturn == null)
                return HttpResponseCodeHelper.NotFound();

            var token = _attendanceService.GetTokenToReportGeneralAttendance(eventToReturn);
            var toReturn = new EventToReportAttendanceViewModel();
            _mapper.Map(eventToReturn, toReturn.EventDetail);
            toReturn.Token = token;
            return HttpResponseCodeHelper.Ok(toReturn);
        }
    }
}