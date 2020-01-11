using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Helpers;
using NetBaires.Api.ViewModels.GroupCode;
using NetBaires.Data;

namespace NetBaires.Api.Features.GroupsCodes.DeleteGroupCode
{

    public class DeleteGroupCodeHandler : IRequestHandler<DeleteGroupCodeCommand, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly NetBairesContext _context;

        public DeleteGroupCodeHandler(IMapper mapper,
            NetBairesContext context)
        {
            _mapper = mapper;
            _context = context;
        }


        public async Task<IActionResult> Handle(DeleteGroupCodeCommand request, CancellationToken cancellationToken)
        {

            var groupCode = await _context.GroupCodes.Include(x => x.Members)
                .FirstOrDefaultAsync(x => x.Id == request.GroupCodeId);

            if (groupCode == null)
                return HttpResponseCodeHelper.NotFound();

            _context.GroupCodes.Remove(groupCode);
            await _context.SaveChangesAsync();

            return HttpResponseCodeHelper.NotContent();

        }
    }

}