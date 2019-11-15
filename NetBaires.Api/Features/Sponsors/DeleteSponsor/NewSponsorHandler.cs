using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Helpers;
using NetBaires.Api.Services;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Sponsors
{
    public class DeleteSponsorHandler : IRequestHandler<DeleteSponsorCommand, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;
        private readonly IFilesServices filesServices;

        public DeleteSponsorHandler(NetBairesContext context,
            IMapper mapper,
            IFilesServices filesServices)
        {
            _context = context;
            _mapper = mapper;
            filesServices = filesServices;
        }


        public async Task<IActionResult> Handle(DeleteSponsorCommand request, CancellationToken cancellationToken)
        {
            var sponsorToDelete = await _context.Sponsors.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (sponsorToDelete == null)
                return HttpResponseCodeHelper.NotFound();
            _context.Sponsors.Remove(sponsorToDelete);
            await _context.SaveChangesAsync();
            return HttpResponseCodeHelper.NotContent();
        }
    }
}