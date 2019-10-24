using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetBaires.Api.Auth;
using NetBaires.Api.Options;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Badges
{
    public class DeleteBadgeHandler : IRequestHandler<DeleteBadgeHandler.DeleteBadge, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetToAssignHandler> _logger;

        public DeleteBadgeHandler(ICurrentUser currentUser,
            NetBairesContext context,
            IMapper mapper,
            IOptions<AssistanceOptions> assistanceOptions,
            ILogger<GetToAssignHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(DeleteBadge request, CancellationToken cancellationToken)
        {
            var badge = await _context.Badges.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (badge == null)
                return new StatusCodeResult(404);
            _context.Remove(badge);
            await _context.SaveChangesAsync();
            return new ObjectResult(request) { StatusCode = 200 };

        }

        public class DeleteBadge : IRequest<IActionResult>
        {
            public DeleteBadge(int id)
            {
                Id = id;
            }

            public int Id { get;  }
        }
       
    }
}