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
        private readonly CommonOptions commonOptions;
        private readonly BadgesOptions badgesOptions;
        private readonly IFilesServices filesServices;

        public BadgesServices(IOptions<CommonOptions> commonOptions,
            IOptions<BadgesOptions> badgesOptions,
            IFilesServices filesServices)
        {
            this.commonOptions = commonOptions.Value;
            this.badgesOptions = badgesOptions.Value;
            this.filesServices = filesServices;
        }
        public async Task<Stream> GetAsync(string badgeFileName)
        {
            return await filesServices.GetAsync(badgeFileName, Container.Badges);
        }
        public async Task<BadgeCreationDetail> CreateAsync(IFormFile badge)
        {
            var fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(badge.FileName)}";
            var fileDetail = await filesServices.UploadAsync(badge.OpenReadStream(), fileName, Container.Badges);
            return new BadgeCreationDetail(fileDetail);
        }
        public async Task<BadgeCreationDetail> ReplaceAsync(IFormFile newBadge, string oldBadgeName)
        {
            var fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(newBadge.FileName)}";

            await filesServices.DeleteAsync(oldBadgeName, Container.Badges);
            var fileDetail = await filesServices.UploadAsync(newBadge.OpenReadStream(), fileName, Container.Badges);
            return new BadgeCreationDetail(fileDetail);
        }
        public async Task<bool> RemoveAsync(string badgeName)
        {
            return await filesServices.DeleteAsync(badgeName, Container.Badges);
        }
        public string GenerateImageUrl(Badge badge)
        {
            return $"{commonOptions.ApiUrl}{badgesOptions.ImageEndPoint.Replace("{badgeImageName}", badge.ImageName)}";
        }
        public string GenerateDetailUrl(Badge badge)
        {
            return $"{commonOptions.SiteUrl}{badgesOptions.DetailUrl.Replace("{id}", badge.Id.ToString())}";
        }
        public string GenerateDetailUrl(int badgeId)
        {
            return $"{commonOptions.SiteUrl}{badgesOptions.DetailUrl.Replace("{id}", badgeId.ToString())}";
        }
        public string GeneratePublicMemberUrl(Badge badge, Member member)
        {
            return $"{commonOptions.SiteUrl}{badgesOptions.PublicUrl.Replace("{badgeId}", badge.Id.ToString()).Replace("{memberId}", member.Id.ToString())}";
        }
        public string GenerateImageUrl(int badgeId)
        {
            return $"{commonOptions.ApiUrl}{badgesOptions.ImageEndPoint.Replace("{id}", badgeId.ToString())}";
        }
    }
}
