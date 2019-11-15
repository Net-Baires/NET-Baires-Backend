using System;
using System.IO;
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
    public class UpdateSponsorHandler : IRequestHandler<UpdateSponsorCommand, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;
        private readonly IFilesServices filesServices;

        public UpdateSponsorHandler(NetBairesContext context,
                                    IMapper mapper,
                                    IFilesServices filesServices)
        {
            _context = context;
            _mapper = mapper;
            this.filesServices = filesServices;
        }


        public async Task<IActionResult> Handle(UpdateSponsorCommand request, CancellationToken cancellationToken)
        {
            var sponsor = await _context.Sponsors.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (sponsor == null)
                return HttpResponseCodeHelper.NotFound();
            _mapper.Map(request, sponsor);

            if (request.ImageFile != null)
            {
                var fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(request.ImageFile.FileName)}";
                var response = await filesServices.UploadAsync(request.ImageFile.OpenReadStream(), fileName, Container.Sponsors);
                await filesServices.DeleteAsync(sponsor.LogoFileName, Container.Sponsors);
                sponsor.SetFile(response.FileUri, response.Name);
            }

            await _context.SaveChangesAsync();

            return HttpResponseCodeHelper.Ok(_mapper.Map<Sponsor>(sponsor));

        }
    }
}