using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Services;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Badges
{
    public class NewBadgeHandler : IRequestHandler<NewBadgeHandler.NewBadge, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IBadgesServices badgesServices;
        private readonly IMapper _mapper;
        private readonly ILogger<NewBadgeHandler> logger;

        public NewBadgeHandler(NetBairesContext context,
               IBadgesServices badgesServices,
            IMapper mapper,
            ILogger<NewBadgeHandler> logger)
        {
            _context = context;
            this.badgesServices = badgesServices;
            this._mapper = mapper;
            this.logger = logger;
        }


        public async Task<IActionResult> Handle(NewBadgeHandler.NewBadge request, CancellationToken cancellationToken)
        {
            var newBadge = _mapper.Map(request, new Badge());
            if (request.ImageFile != null)
            {
                var badgeCreateResponse = await badgesServices.CreateAsync(request.ImageFile);
                if (badgeCreateResponse == null)
                    return new StatusCodeResult(400);
                newBadge.ImageName = badgeCreateResponse.FileDetail.Name;

                await _context.Badges.AddAsync(newBadge);
            }

            await _context.SaveChangesAsync();


            return new ObjectResult(_mapper.Map(newBadge, new NewBadgeResponse())) { StatusCode = 200 };

        }

        private List<GetToAssignHandler.BadgeAssignResponse> ReduceBadge(Badge item, List<GetToAssignHandler.BadgeAssignResponse> accum, bool assigned)
        {
            var returnValue = _mapper.Map(item, new GetToAssignHandler.BadgeAssignResponse());
            returnValue.Assigned = assigned;
            accum.Add(returnValue);
            return accum;
        }


        public class NewBadge : NewBadgeCommon, IRequest<IActionResult>
        {
            public IFormFile ImageFile { get; set; }
        }
        public class NewBadgeResponse : NewBadgeCommon
        {
            public int Id { get; set; }
        }
        public class NewBadgeCommon
        {
            public string BadgeUrl { get; set; }
            public DateTime Created { get; set; }
            public string BadgeImageUrl { get; set; }
            public string IssuerUrl { get; set; }
            public string Image { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }
        public class NewBadgeProfile : Profile
        {
            public NewBadgeProfile()
            {
                CreateMap<NewBadge, Badge>();
                CreateMap<Badge, NewBadgeResponse>();
            }
        }
    }
}