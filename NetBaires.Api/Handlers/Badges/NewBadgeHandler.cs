using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetBaires.Api.Auth;
using NetBaires.Api.Options;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Badges
{
    public class NewBadgeHandler : IRequestHandler<NewBadgeHandler.NewBadge, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;
        private readonly ConnectionStringsOptions _connectionStringsOptions;
        private readonly ILogger<GetToAssignHandler> _logger;

        public NewBadgeHandler(ICurrentUser currentUser,
            NetBairesContext context,
            IMapper mapper,
            IOptions<ConnectionStringsOptions> connectionStringsOptions,
            ILogger<GetToAssignHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _connectionStringsOptions = connectionStringsOptions.Value;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(NewBadgeHandler.NewBadge request, CancellationToken cancellationToken)
        {

            var newBadge = _mapper.Map(request, new Badge());

            await _context.SaveChangesAsync();
            var newBadgeResponse = _mapper.Map(newBadge, new NewBadgeResponse());
            if (request.ImageFile.Length > 0)
            {
                using (var fileStream = new FileStream(request.ImageFile.FileName, FileMode.Create))
                {
                    request.ImageFile.CopyTo(fileStream);
                    string storageConnectionString = Environment.GetEnvironmentVariable("CONNECT_STR");
                }
            }

            return new ObjectResult(newBadgeResponse) { StatusCode = 200 };

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