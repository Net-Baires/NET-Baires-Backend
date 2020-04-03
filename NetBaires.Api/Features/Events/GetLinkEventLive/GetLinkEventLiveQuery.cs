using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Events.GetLinkEventLive
{
    public class GetLinkEventLiveQuery : IRequest<IActionResult>
    {
    
        public class Response
        {
            public Response(string onlineLink)
            {
                OnlineLink = onlineLink;
            }

            public string OnlineLink { get; set; }

        }
    }
}