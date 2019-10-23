using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetBaires.Api.Auth;
using NetBaires.Api.Options;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Events
{

    public class PutCheckAssistanceGeneralHandler : IRequestHandler<PutCheckAssistanceGeneralHandler.PutCheckAssistanceGeneral, IActionResult>
    {
        private readonly ICurrentUser _currentUser;
        private readonly NetBairesContext _context;
        private readonly AssistanceOptions _assistanceOptions;
        private readonly ILogger<PutCheckAssistanceGeneralHandler> _logger;

        public PutCheckAssistanceGeneralHandler(ICurrentUser currentUser,
            NetBairesContext context,
            IOptions<AssistanceOptions> assistanceOptions,
            ILogger<PutCheckAssistanceGeneralHandler> logger)
        {
            _currentUser = currentUser;
            _context = context;
            _assistanceOptions = assistanceOptions.Value;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(PutCheckAssistanceGeneral request, CancellationToken cancellationToken)
        {
            var response = TokenService.Validate(_assistanceOptions.AskAssistanceSecret, request.Token);
            if (!response.Valid)
                return new StatusCodeResult(400);


            var eventId = int.Parse(response.Claims.FirstOrDefault(x => x.Type == "EventId").Value.ToString());
            var memberId = _currentUser.User.Id;

            var eventToAdd = _context.EventMembers.FirstOrDefault(x => x.EventId == eventId && x.MemberId == memberId);
            if (eventToAdd == null)
                eventToAdd = new EventMember(memberId, eventId);
            eventToAdd.Attend();
            await _context.EventMembers.AddAsync(eventToAdd);
            await _context.SaveChangesAsync();
            return new StatusCodeResult(200);
        }


        public class PutCheckAssistanceGeneral : IRequest<IActionResult>
        {
            public PutCheckAssistanceGeneral(string token)
            {
                Token = token;
            }

            public string Token { get; }
        }

    }
}