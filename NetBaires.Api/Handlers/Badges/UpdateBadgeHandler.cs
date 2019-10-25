using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Services;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Badges
{
    public class UpdateBadgeHandler : IRequestHandler<UpdateBadgeHandler.UpdateBadge, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;
        private readonly IBadgesServices badgesServices;
        private readonly ILogger<GetToAssignHandler> _logger;

        public UpdateBadgeHandler(NetBairesContext context,
            IMapper mapper,
            IBadgesServices badgesServices,
            ILogger<UpdateBadgeHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            this.badgesServices = badgesServices;
        }


        public async Task<IActionResult> Handle(UpdateBadge request, CancellationToken cancellationToken)
        {
            var badge = await _context.Badges.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (badge == null)
                return new StatusCodeResult(404);
            _mapper.Map(request, badge);


            if (request.ImageFile != null)
            {
                var response = badgesServices.Replace(request.ImageFile.OpenReadStream(), badge.ImageName);
                badge.ImageName = response.FileDetail.Name;
            }

            await _context.SaveChangesAsync();
                       
            return new ObjectResult(_mapper.Map(badge, new UpdateBadgeResponse())) { StatusCode = 200 };

        }


        public class UpdateBadge : UpdateBadgeCommon, IRequest<IActionResult>
        {
            public IFormFile ImageFile { get; set; }
        }
        public class UpdateBadgeResponse : UpdateBadgeCommon
        {
        }
        public class UpdateBadgeCommon
        {
            public int Id { get; set; }
            public string BadgeUrl { get; set; }
            public DateTime Created { get; set; }
            public string BadgeImageUrl { get; set; }
            public string IssuerUrl { get; set; }
            public string Image { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }
        public class UpdateBadgeProfile : Profile
        {
            public UpdateBadgeProfile()
            {
                CreateMap<UpdateBadge, Badge>().ForAllMembers(
                    opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
                CreateMap<Badge, UpdateBadgeResponse>();
            }
        }
    }
}