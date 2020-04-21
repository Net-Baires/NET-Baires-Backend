using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Auth;
using NetBaires.Api.Helpers;
using NetBaires.Data;

namespace NetBaires.Api.Features.Events.CreateGroupCode
{
    public class CreateGroupCodeHandler : IRequestHandler<CreateGroupCodeCommand, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;

        public CreateGroupCodeHandler(NetBairesContext context,
            IMapper mapper,
            ICurrentUser currentUser)
        {
            _context = context;
            _mapper = mapper;
            _currentUser = currentUser;
        }


        public async Task<IActionResult> Handle(CreateGroupCodeCommand request, CancellationToken cancellationToken)
        {
            var eventToUpdate = await _context.Events.FirstOrDefaultAsync(x => x.Id == request.EventId);

            if (eventToUpdate == null)
                return HttpResponseCodeHelper.NotFound();

            var groupCode = eventToUpdate.CreateGroupCode(request.Detail);

            await _context.SaveChangesAsync(cancellationToken);
            return HttpResponseCodeHelper.Ok(_mapper.Map<CreateGroupCodeCommand.Response>(groupCode));
        }
    }
}