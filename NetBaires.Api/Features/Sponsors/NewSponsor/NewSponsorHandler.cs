using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetBaires.Api.Helpers;
using NetBaires.Api.Services;
using NetBaires.Api.ViewModels;
using NetBaires.Data;
using NetBaires.Data.Entities;

namespace NetBaires.Api.Features.Sponsors.NewSponsor
{
    public class NewSponsorHandler : IRequestHandler<NewSponsorCommand, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;
        private readonly IFilesServices filesServices;

        public NewSponsorHandler(NetBairesContext context,
            IMapper mapper,
            IFilesServices filesServices)
        {
            _context = context;
            _mapper = mapper;
            this.filesServices = filesServices;
        }


        public async Task<IActionResult> Handle(NewSponsorCommand request, CancellationToken cancellationToken)
        {
            var newSponsor = _mapper.Map<Sponsor>(request);
            if (request.ImageFile != null)
            {
                var fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(request.ImageFile.FileName)}";
                var logoUplaod = await filesServices.UploadAsync(request.ImageFile.OpenReadStream(), fileName, Container.Sponsors);
                if (logoUplaod == null)
                    return HttpResponseCodeHelper.NotFound();

                newSponsor.SetFile(logoUplaod.FileUri, logoUplaod.Name);
                await _context.Sponsors.AddAsync(newSponsor);
            }

            await _context.SaveChangesAsync();
            var mapped = _mapper.Map<SponsorDetailViewModel>(newSponsor);
            return HttpResponseCodeHelper.Ok(mapped);
        }
    }
}