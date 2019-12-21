﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Features.Members.ViewModels;
using NetBaires.Api.Helpers;
using NetBaires.Data;

namespace NetBaires.Api.Features.Badges.GetBadge
{

    public class AddMemberHandler : IRequestHandler<AddMemberCommand, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;

        public AddMemberHandler(
            NetBairesContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<IActionResult> Handle(AddMemberCommand request, CancellationToken cancellationToken)
        {
            var memberToCheck = await _context.Members.FirstOrDefaultAsync(x => (x.Email.ToUpper() == request.Email.ToUpper())
                                        ||
                                        (request.MeetupId != null ? x.MeetupId == request.MeetupId : true));
            if (memberToCheck != null)
                return HttpResponseCodeHelper.Conflict("Ya se encuentra un usuario registrado con ese email");

            var member = _mapper.Map<Member>(request);
            member.Role = UserRole.Member;
            await _context.Members.AddAsync(member);
            await _context.SaveChangesAsync();


            return HttpResponseCodeHelper.Ok(_mapper.Map<MemberDetailViewModel>(member));

        }
    }
}