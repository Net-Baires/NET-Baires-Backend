using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Services;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Sponsors
{
    public class NewSponsorHandler : IRequestHandler<NewSponsorHandler.NewSponsor, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;
        private readonly IFilesServices filesServices;
        private readonly ILogger<NewSponsorHandler> logger;

        public NewSponsorHandler(NetBairesContext context,
            IMapper mapper,
            IFilesServices filesServices,
            ILogger<NewSponsorHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            this.filesServices = filesServices;
            this.logger = logger;
        }


        public async Task<IActionResult> Handle(NewSponsor request, CancellationToken cancellationToken)
        {
            var newSponsor = _mapper.Map(request, new Sponsor());
            if (request.ImageFile != null)
            {
                var fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(request.ImageFile.FileName)}";
                var logoUplaod = await filesServices.UploadAsync(request.ImageFile.OpenReadStream(), fileName, Container.Sponsors);
                if (logoUplaod == null)
                    return new StatusCodeResult(400);
                newSponsor.SetFile(logoUplaod.FileUri, logoUplaod.Name);
                await _context.Sponsors.AddAsync(newSponsor);
            }

            await _context.SaveChangesAsync();
            var mapped = _mapper.Map(newSponsor, new NewSponsorResponse());
            return new ObjectResult(mapped) { StatusCode = 200 };

        }



        public class NewSponsor : NewSponsorCommon, IRequest<IActionResult>
        {
            public IFormFile ImageFile { get; set; }
        }
        public class NewSponsorResponse : NewSponsorCommon
        {
            public int Id { get; set; }
        }
        public class NewSponsorCommon
        {
            public string Name { get; set; }
            public string SiteUrl { get; set; }
            public string Description { get; set; }
            public string LogoUrl { get; set; }
        }
        public class NewSponsorProfile : Profile
        {
            public NewSponsorProfile()
            {
                CreateMap<NewSponsor, Sponsor>();
                CreateMap<Sponsor, NewSponsorResponse>();
            }
        }
    }
}