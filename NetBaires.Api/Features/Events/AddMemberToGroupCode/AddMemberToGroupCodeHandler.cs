using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Auth;
using NetBaires.Api.Features.Events.AddAttendee;
using NetBaires.Api.Helpers;
using NetBaires.Data;
using NetBaires.Data.Entities;

namespace NetBaires.Api.Features.Events.AddMemberToGroupCode
{
    public class AddMemberToGroupCodeHandler : IRequestHandler<AddMemberToGroupCodeCommand, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;
        private readonly ILogger<AddAttendeeHandler> _logger;

        public AddMemberToGroupCodeHandler(NetBairesContext context,
            ICurrentUser currentUser,
            IMapper mapper,
            ILogger<AddAttendeeHandler> logger)
        {
            _context = context;
            _currentUser = currentUser;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IActionResult> Handle(AddMemberToGroupCodeCommand request, CancellationToken cancellationToken)
        {
            var groupCode = await _context.GroupCodes.Include(x=> x.Members).FirstOrDefaultAsync(x => x.Event.Id == request.EventId
                                                                               &&
                                                                               x.Id == request.GroupCodeId);

            if (groupCode == null)
                return HttpResponseCodeHelper.NotFound("El Grupo de código no existe");


            var memberInEvent = await _context.Events.Include(x=> x.Attendees).FirstOrDefaultAsync(x => x.Id == groupCode.EventId
                                                                    &&
                                                                    x.GroupCodes.Any(g => g.Id == request.GroupCodeId)
                                                                    &&
                                                                    x.Attendees.Any(a =>
                                                                        a.MemberId == request.MemberId));

            if (memberInEvent == null)
                return HttpResponseCodeHelper.Error("El miembro que intenta agregar código");

            var member = await _context.Members.FirstOrDefaultAsync(x => x.Id == request.MemberId);
            memberInEvent.Attended(member, AttendanceRegisterType.CurrentEvent);

            groupCode.AddMember(member);

            await _context.SaveChangesAsync();

            return HttpResponseCodeHelper.Ok(_mapper.Map<AddMemberToGroupCodeCommand.Response>(groupCode));
        }
    }
}