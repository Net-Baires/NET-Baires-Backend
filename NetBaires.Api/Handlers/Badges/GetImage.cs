using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetBaires.Api.Auth;
using NetBaires.Api.Options;
using NetBaires.Api.Services;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Badges
{

    public class GetImageHandler : IRequestHandler<GetImageHandler.GetIamge, Stream>
    {
        private readonly NetBairesContext _context;
        private readonly IBadgesServices badgesServices;
        private readonly IMapper _mapper;
        private readonly ILogger<GetToAssignHandler> _logger;

        public GetImageHandler(ICurrentUser currentUser,
            NetBairesContext context,
            IBadgesServices badgesServices,
            IMapper mapper,
            ILogger<GetToAssignHandler> logger)
        {
            _context = context;
            this.badgesServices = badgesServices;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<Stream> Handle(GetIamge request, CancellationToken cancellationToken)
        {
            var badge = await _context.Badges.FirstOrDefaultAsync(x => x.Id == request.BadgeId);

            // if (badge == null)
            //     return new HttpResponseMessage(HttpStatusCode.NotFound);

            var file = await badgesServices.GetAsync(badge.ImageName);
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(file)
            };
            result.Content.Headers.ContentDisposition =
                new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "CertificationCard.pdf"
                };
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");

            return file;

        }


        public class GetIamge : IRequest<Stream>
        {
            public GetIamge(int badgeId)
            {
                BadgeId = badgeId;
            }

            public int BadgeId { get; }
        }


    }
}