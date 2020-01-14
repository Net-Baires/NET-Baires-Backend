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

namespace NetBaires.Api.Features.Events.DeleteMemberToGroupCode
{
    public class DeleteMemberToGroupCodeHandler : IRequestHandler<DeleteMemberToGroupCodeCommand, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;
        private readonly ILogger<AddAttendeeHandler> _logger;

        public DeleteMemberToGroupCodeHandler(NetBairesContext context,
            ICurrentUser currentUser,
            IMapper mapper,
            ILogger<AddAttendeeHandler> logger)
        {
            _context = context;
            _currentUser = currentUser;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IActionResult> Handle(DeleteMemberToGroupCodeCommand request, CancellationToken cancellationToken)
        {
            var groupCode = await _context.GroupCodes.Include(x=> x.Members).FirstOrDefaultAsync(x => x.Event.Id == request.EventId
                                                                               &&
                                                                               x.Id == request.GroupCodeId);

            if (groupCode == null)
                return HttpResponseCodeHelper.NotFound("El Grupo de codigo no existe");


            var memberInEvent = await _context.Events.AnyAsync(x => x.Id == groupCode.EventId
                                                                    &&
                                                                    x.GroupCodes.Any(g => g.Id == request.GroupCodeId)
                                                                    &&
                                                                    x.Attendees.Any(a =>
                                                                        a.MemberId == request.MemberId), cancellationToken: cancellationToken);

            if (!memberInEvent)
                return HttpResponseCodeHelper.Error("El miembro que intenta agregar código");

            var member = await _context.Members.FirstOrDefaultAsync(x => x.Id == request.MemberId, cancellationToken: cancellationToken);

            groupCode.RemoveMember(member);

            await _context.SaveChangesAsync(cancellationToken);

            return HttpResponseCodeHelper.Ok(_mapper.Map<DeleteMemberToGroupCodeCommand.Response>(groupCode));
        }
    }
}