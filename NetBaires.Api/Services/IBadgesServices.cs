using Microsoft.AspNetCore.Http;
using NetBaires.Data;
using System.IO;
using System.Threading.Tasks;
using NetBaires.Data.Entities;

namespace NetBaires.Api.Services
{
    public interface IBadgesServices
    {
        string GenerateImageUrl(Badge badge);
        Task<BadgeCreationDetail> CreateAsync(IFormFile badge);
        Task<BadgeCreationDetail> ReplaceAsync(IFormFile newBadge, string oldBadgeName);
        Task<bool> RemoveAsync(string badgeName);
        Task<Stream> GetAsync(string badgeFileName);
        string GenerateDetailUrl(int badgeId);
        string GenerateDetailUrl(Badge badge);
        string GeneratePublicMemberUrl(Badge badge, Member member);
    }
}
