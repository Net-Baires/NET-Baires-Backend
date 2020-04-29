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
using NetBaires.Api.ViewModels;
using NetBaires.Data;
using NetBaires.Data.Entities;

namespace NetBaires.Api.Features.Templates.UpdateTemplate
{
    public class UpdateTemplateHandler : IRequestHandler<UpdateTemplateCommand, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;

        public UpdateTemplateHandler(NetBairesContext context,
                                    IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<IActionResult> Handle(UpdateTemplateCommand request, CancellationToken cancellationToken)
        {
            var template = await _context.Templates.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (template == null)
                return HttpResponseCodeHelper.NotFound();
            _mapper.Map(request, template);
            
            await _context.SaveChangesAsync(cancellationToken);

            return HttpResponseCodeHelper.Ok(_mapper.Map<TemplateViewModel>(template));

        }
    }
}