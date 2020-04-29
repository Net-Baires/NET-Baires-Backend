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

namespace NetBaires.Api.Features.Templates.NewTemplate
{
    public class NewTemplateHandler : IRequestHandler<NewTemplateCommand, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;

        public NewTemplateHandler(NetBairesContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<IActionResult> Handle(NewTemplateCommand request, CancellationToken cancellationToken)
        {
            var newTemplate = _mapper.Map<Template>(request);
            await _context.Templates.AddAsync(newTemplate);

            await _context.SaveChangesAsync();
            var mapped = _mapper.Map<TemplateViewModel>(newTemplate);

            return HttpResponseCodeHelper.Ok(mapped);
        }
    }
}