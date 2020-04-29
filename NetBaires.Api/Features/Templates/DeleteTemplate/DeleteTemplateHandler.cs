using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Helpers;
using NetBaires.Api.Services;
using NetBaires.Data;

namespace NetBaires.Api.Features.Templates.DeleteTemplate
{
    public class DeleteTemplateHandler : IRequestHandler<DeleteTemplateCommand, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;

        public DeleteTemplateHandler(NetBairesContext context,
            IMapper mapper,
            IFilesServices filesServices)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<IActionResult> Handle(DeleteTemplateCommand request, CancellationToken cancellationToken)
        {
            var templateToDelete = await _context.Templates.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (templateToDelete == null)
                return HttpResponseCodeHelper.NotFound();

            var anyEvent = await _context.Events.AnyAsync(x => x.EmailTemplateThanksSpeakers == templateToDelete
                                                            ||
                                                            x.EmailTemplateThanksAttended == templateToDelete
                                                            ||
                                                            x.EmailTemplateThanksSponsors == templateToDelete);
            if (anyEvent)
                return HttpResponseCodeHelper.Conflict(
                    "El template que intenta eliminar se encuentra asociado a uno o mas eventos.");

            _context.Templates.Remove(templateToDelete);
            await _context.SaveChangesAsync(cancellationToken);
            return HttpResponseCodeHelper.NotContent();
        }
    }
}