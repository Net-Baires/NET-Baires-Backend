using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Helpers;
using NetBaires.Data;

namespace NetBaires.Api.Features.GroupsCodes.UpdateGroupCode
{

    public class UpdateGroupCodeHandler : IRequestHandler<UpdateGroupCodeCommand, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly NetBairesContext _context;

        public UpdateGroupCodeHandler(IMapper mapper,
            NetBairesContext context)
        {
            _mapper = mapper;
            _context = context;
        }


        public async Task<IActionResult> Handle(UpdateGroupCodeCommand request, CancellationToken cancellationToken)
        {
            var groupCode = await _context.GroupCodes.FirstOrDefaultAsync(x => x.Id == request.GroupCodeId);

            if (groupCode == null)
                return HttpResponseCodeHelper.NotFound("El Grupo de codigo no existe");

            _mapper.Map(request, groupCode);
            _context.Entry(groupCode).State = EntityState.Modified;

            await _context.SaveChangesAsync();


            return HttpResponseCodeHelper.NotContent();
        }
    }
}