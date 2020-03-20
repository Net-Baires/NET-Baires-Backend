using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Auth;
using NetBaires.Api.Helpers;
using NetBaires.Data;

namespace NetBaires.Api.Features.Members.SearchMember
{

    public class UpdateInformationHandler : IRequestHandler<UpdateInformationCommand, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;

        public UpdateInformationHandler(
            NetBairesContext context,
            ICurrentUser currentUser,
            IMapper mapper)
        {
            _context = context;
            _currentUser = currentUser;
            _mapper = mapper;
        }


        public async Task<IActionResult> Handle(UpdateInformationCommand request, CancellationToken cancellationToken)
        {
            var member = await _context.Members.Include(x=> x.PushNotifications).FirstOrDefaultAsync(x => x.Id == _currentUser.User.Id);
            if (member == null)
                return HttpResponseCodeHelper.NotFound();


            member.AddPushNotification(request.PushNotificationId);
            await _context.SaveChangesAsync();
            return HttpResponseCodeHelper.NotContent();
        }
    }
}