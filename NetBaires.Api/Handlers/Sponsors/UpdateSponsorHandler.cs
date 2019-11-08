using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Features.Badges.GetToAssign;
using NetBaires.Api.Services;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Sponsors
{
    public class UpdateSponsorHandler : IRequestHandler<UpdateSponsorHandler.UpdateSponsor, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;
        private readonly IFilesServices filesServices;
        private readonly ILogger<GetToAssignHandler> _logger;

        public UpdateSponsorHandler(NetBairesContext context,
            IMapper mapper,
                        IFilesServices filesServices,
            ILogger<UpdateSponsorHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            this.filesServices = filesServices;
        }


        public async Task<IActionResult> Handle(UpdateSponsor request, CancellationToken cancellationToken)
        {
            var sponsor = await _context.Sponsors.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (sponsor == null)
                return new StatusCodeResult(404);
            _mapper.Map(request, sponsor);


            if (request.ImageFile != null)
            {
                var fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(request.ImageFile.FileName)}";
                var response = await filesServices.UploadAsync(request.ImageFile.OpenReadStream(), fileName, Container.Sponsors);
                await filesServices.DeleteAsync(sponsor.LogoFileName, Container.Sponsors);
                sponsor.SetFile(response.FileUri, response.Name);
            }

            await _context.SaveChangesAsync();

            return new ObjectResult(_mapper.Map(sponsor, new UpdateSponsorResponse())) { StatusCode = 200 };

        }


        public class UpdateSponsor : UpdateSponsorCommon, IRequest<IActionResult>
        {
            public IFormFile ImageFile { get; set; }
        }
        public class UpdateSponsorResponse : UpdateSponsorCommon
        {
        }
        public class UpdateSponsorCommon
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string SiteUrl { get; set; }
            public string Description { get; set; }
            public string LogoUrl { get; set; }
        }
        public class UpdateSponsorProfile : Profile
        {
            public UpdateSponsorProfile()
            {
                CreateMap<UpdateSponsor, Sponsor>();
                CreateMap<Sponsor, UpdateSponsorResponse>();
            }
        }
    }
}