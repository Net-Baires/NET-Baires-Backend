using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Auth;
using NetBaires.Api.Features.Events.AddAttendee;
using NetBaires.Api.Helpers;
using NetBaires.Data;

namespace NetBaires.Api.Features.GroupsCodes.AddMemberToGroupCode
{
    public class AddMemberToGroupCodeHandler : IRequestHandler<AddMemberToGroupCodeCommand, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly ILogger<AddAttendeeHandler> _logger;

        public AddMemberToGroupCodeHandler(NetBairesContext context,
            ICurrentUser currentUser,
            ILogger<AddAttendeeHandler> logger)
        {
            _context = context;
            _currentUser = currentUser;
            _logger = logger;
        }
        public async Task<IActionResult> Handle(AddMemberToGroupCodeCommand request, CancellationToken cancellationToken)
        {
            var groupCode = await _context.GroupCodes.FirstOrDefaultAsync(x => x.Id == request.GroupCodeId);

            if (groupCode == null)
                return HttpResponseCodeHelper.NotFound("El Grupo de codigo no existe");

            if (groupCode.Code.ToUpper() != request.Code.ToUpper())
                return HttpResponseCodeHelper.NotFound("El código ingresado es incorrecto");

            var memberInEvent = await _context.Events.AnyAsync(x => x.Id == groupCode.EventId
                                                                    &&
                                                                    x.GroupCodes.Any(g => g.Code == request.Code)
                                                                    &&
                                                                    x.Attendees.Any(a =>
                                                                        a.MemberId == _currentUser.User.Id));

            if (!memberInEvent)
                return HttpResponseCodeHelper.Error("El miembro logueado no se encuentra registrado en el evento al cual pertenece el codigo");

            var member = await _context.Members.FirstOrDefaultAsync(x => x.Id == _currentUser.User.Id);

            groupCode.AddMember(member, request.Code);

            await _context.SaveChangesAsync();

            return HttpResponseCodeHelper.NotContent();
        }
    }
}