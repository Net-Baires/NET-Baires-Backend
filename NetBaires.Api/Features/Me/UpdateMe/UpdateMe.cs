using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Auth;
using NetBaires.Api.Features.Members.ViewModels;
using NetBaires.Api.Helpers;
using NetBaires.Api.Services;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Me
{

    public class UpdateMeHandler : IRequestHandler<UpdateMeCommand, IActionResult>
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


        public async Task<IActionResult> Handle(UpdateMeCommand request, CancellationToken cancellationToken)
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
            return HttpResponseCodeHelper.Ok(_mapper.Map(member, new MemberDetailViewModel()));
        }
    }
}