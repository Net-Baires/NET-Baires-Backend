using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NetBaires.Api.Options;
using NetBaires.Data;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NetBaires.Api.Services
{
    public class BadgesServices : IBadgesServices
    {
        private readonly CommonOptions _commonOptions;
        private readonly BadgesOptions _badgesOptions;
        private readonly IFilesServices _filesServices;

        public BadgesServices(IOptions<CommonOptions> commonOptions,
            IOptions<BadgesOptions> badgesOptions,
            IFilesServices filesServices)
        {
            this._commonOptions = commonOptions.Value;
            this._badgesOptions = badgesOptions.Value;
            this._filesServices = filesServices;
        }
        public async Task<Stream> GetAsync(string badgeFileName)
        {
            return await _filesServices.GetAsync(badgeFileName, Container.Badges);
        }
        public async Task<BadgeCreationDetail> CreateAsync(IFormFile badge)
        {
            var fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(badge.FileName)}";
            var fileDetail = await _filesServices.UploadAsync(badge.OpenReadStream(), fileName, Container.Badges);
            return new BadgeCreationDetail(fileDetail);
        }
        public async Task<BadgeCreationDetail> ReplaceAsync(IFormFile newBadge, string oldBadgeName)
        {
            var fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(newBadge.FileName)}";

            await _filesServices.DeleteAsync(oldBadgeName, Container.Badges);
            var fileDetail = await _filesServices.UploadAsync(newBadge.OpenReadStream(), fileName, Container.Badges);
            return new BadgeCreationDetail(fileDetail);
        }
        public async Task<bool> RemoveAsync(string badgeName)
        {
            return await _filesServices.DeleteAsync(badgeName, Container.Badges);
        }
        public string GenerateImageUrl(Badge badge)
        {
            return $"{_commonOptions.ApiUrl}{_badgesOptions.ImageEndPoint.Replace("{badgeImageName}", badge.ImageName)}";
        }
        public string GenerateDetailUrl(Badge badge)
        {
            return $"{_commonOptions.SiteUrl}/{_badgesOptions.DetailUrl.Replace("{id}", badge.Id.ToString())}";
        }
        public string GenerateDetailUrl(int badgeId)
        {
            return $"{_commonOptions.SiteUrl}/{_badgesOptions.DetailUrl.Replace("{id}", badgeId.ToString())}";
        }
        public string GeneratePublicMemberUrl(Badge badge, Member member)
        {
            return $"{_commonOptions.SiteUrl}/{_badgesOptions.PublicUrl.Replace("{badgeId}", badge.Id.ToString()).Replace("{memberId}", member.Id.ToString())}";
        }
        public string GenerateImageUrl(int badgeId)
        {
            return $"{_commonOptions.ApiUrl}{_badgesOptions.ImageEndPoint.Replace("{id}", badgeId.ToString())}";
        }
    }
}
