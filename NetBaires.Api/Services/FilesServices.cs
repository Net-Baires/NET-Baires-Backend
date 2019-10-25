using Microsoft.Extensions.Options;
using NetBaires.Api.Options;
using NetBaires.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NetBaires.Api.Services
{
    public interface IBadgesServices
    {
        string GenerateImageUrl(Badge badge);
        BadgeCreationDetail Create(Stream badge);
        BadgeCreationDetail Replace(Stream newBadge, string oldBadgeName);
        bool Remove(string badgeName);
        Stream Get(string name);
    }
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
        public Stream Get(string name)
        {
            return new System.IO.MemoryStream();
        }
        public BadgeCreationDetail Create(Stream badge)
        {
            var fileDetail = filesServices.Upload(badge);
            return new BadgeCreationDetail(new FileDetail(Guid.NewGuid().ToString()));
        }
        public BadgeCreationDetail Replace(Stream newBadge, string oldBadgeName)
        {
            filesServices.Delete(oldBadgeName, Container.Badges);
            var fileDetail = filesServices.Upload(newBadge);
            return new BadgeCreationDetail(new FileDetail(Guid.NewGuid().ToString()));
        }
        public bool Remove(string badgeName)
        {
            filesServices.Delete(badgeName, Container.Badges);
            return true;
        }
        public string GenerateImageUrl(Badge badge)
        {
            return $"{commonOptions.SiteUrl}{badgesOptions.ImageEndPoint.Replace("{id}", badge.Id.ToString())}";
        }
    }
    public interface IFilesServices
    {
        FileDetail Upload(Stream file);
        bool Delete(string fileName, Container container);
    }
    public class FilesServices : IFilesServices
    {

        public FileDetail Upload(Stream file)
        {

            return new FileDetail(Guid.NewGuid().ToString());
        }
        public bool Delete(string fileName, Container container)
        {

            return true;
        }
    }
    public enum Container
    {
        Badges,
        Sponsors
    }
    public class BadgeCreationDetail
    {
        public FileDetail FileDetail { get; }

        public BadgeCreationDetail(FileDetail fileDetail)
        {
            FileDetail = fileDetail ?? throw new ArgumentNullException(nameof(fileDetail));
        }
    }
    public class FileDetail
    {
        public string Name { get; }

        public FileDetail(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}
