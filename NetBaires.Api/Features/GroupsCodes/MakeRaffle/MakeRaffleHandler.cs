using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Helpers;
using NetBaires.Api.ViewModels;
using NetBaires.Data;

namespace NetBaires.Api.Features.Events.AssignBadgeToAttendances
{

    public class MakeRaffleHandler : IRequestHandler<MakeRaffleCommand, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<AssignBadgeToAttendancesHandler> _logger;

        public MakeRaffleHandler(NetBairesContext context,
            IMapper mapper,
            ILogger<AssignBadgeToAttendancesHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(MakeRaffleCommand request, CancellationToken cancellationToken)
        {
            var members = await _context.GroupCodes.Include(s=> s.Members).ThenInclude(s=> s.Member).Where(x => x.Id == request.GroupCodeId)
                                                   .SelectMany(x => x.Members.Where(s =>
                                                   request.RepeatWinners ? s.Winner : !s.Winner))
                                                   .OrderBy(s => new Random().Next())
                                                   .Take(request.CountOfWinners)
                                                   .ToListAsync();
            var a = await _context.GroupCodes.Include(s => s.Members).ThenInclude(s => s.Member).ToListAsync();

            for (int i = 0; i < request.CountOfWinners; i++)
                members[i].SetAsWinner(i + 1);

            await _context.SaveChangesAsync();

            return HttpResponseCodeHelper.Ok(_mapper.Map<List<MemberDetailViewModel>>(members.Select(s => s.Member)));
        }
    }
}