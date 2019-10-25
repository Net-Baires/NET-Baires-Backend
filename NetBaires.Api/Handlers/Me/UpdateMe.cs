using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetBaires.Api.Auth;
using NetBaires.Api.Handlers.Events.Models;
using NetBaires.Api.Options;
using NetBaires.Api.Services;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Events
{

    public class UpdateMeHandler : IRequestHandler<UpdateMeHandler.UpdateMe, IActionResult>
    {
        private readonly ICurrentUser currentUser;
        private readonly IMapper _mapper;
        private readonly IFilesServices filesServices;
        private readonly NetBairesContext _context;
        private readonly ILogger<UpdateMeHandler> _logger;

        public UpdateMeHandler(ICurrentUser currentUser,
                               IMapper mapper,
                               IFilesServices filesServices,
                               NetBairesContext context,
                               ILogger<UpdateMeHandler> logger)
        {
            this.currentUser = currentUser;
            _mapper = mapper;
            this.filesServices = filesServices;
            _context = context;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(UpdateMe request, CancellationToken cancellationToken)
        {
            var currentMemberId = currentUser.User.Id;
            var member = await _context.Members.FirstOrDefaultAsync(x => x.Id == currentMemberId);

            _mapper.Map(request, member);


            if (request.ImageFile != null)
            {
                var fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(request.ImageFile.FileName)}";
                var response = await filesServices.UploadAsync(request.ImageFile.OpenReadStream(), fileName, Container.Members);
                if (!string.IsNullOrEmpty(member.PictureName))
                    await filesServices.DeleteAsync(member.PictureName, Container.Members);
                member.SetFile(response.FileUri, response.Name);
            }

            await _context.SaveChangesAsync();

            return new ObjectResult(_mapper.Map(member, new UpdateMeResponse())) { StatusCode = 200 };
        }


        public class UpdateMe : UpdateMeCommon, IRequest<IActionResult>
        {
            public IFormFile ImageFile { get; set; }

        }
        public class UpdateMeResponse : UpdateMeCommon
        {
            public string Email { get; set; }

        }
        public class UpdateMeCommon
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Username { get; set; }
            public string Twitter { get; set; }
            public string WorkPosition { get; set; }

            public string Instagram { get; set; }
            public string Linkedin { get; set; }
            public string Github { get; set; }
            public string Biography { get; set; }
            public string Picture { get; set; }

        }
        public class UpdateMeProfile : Profile
        {
            public UpdateMeProfile()
            {
                CreateMap<UpdateMe, Member>();
                CreateMap<Member, UpdateMeResponse>();
            }
        }

    }
}