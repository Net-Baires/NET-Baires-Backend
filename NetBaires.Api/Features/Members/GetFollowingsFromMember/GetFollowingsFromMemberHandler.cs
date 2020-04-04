using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Helpers;
using NetBaires.Api.ViewModels;
using NetBaires.Data;

namespace NetBaires.Api.Features.Members.GetFollowingsFromMember
{
    public class GetFollowingsFromMemberHandler : IRequestHandler<GetFollowingsFromMemberQuery, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;

        public GetFollowingsFromMemberHandler(NetBairesContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<IActionResult> Handle(GetFollowingsFromMemberQuery request, CancellationToken cancellationToken)
        {
            var followings = await _context.Members.Where(x=> x.Id ==request.Id).Take(1)
                .SelectMany(x=> x.FollowingMembers)
                .Select(s=> new GetFollowingsFromMemberQuery.Response
                {
                    FollowingDate =  s.FollowingDate,
                    Member =  _mapper.Map<MemberDetailViewModel>(s.Following)
                })
                .ToListAsync(cancellationToken: cancellationToken);

            if (followings == null)
                return HttpResponseCodeHelper.NotFound("El miembro indicado no sigue a ningún otro miembro de la comunidad de NET-Baires.");

            return HttpResponseCodeHelper.Ok(followings);
        }
    }
}