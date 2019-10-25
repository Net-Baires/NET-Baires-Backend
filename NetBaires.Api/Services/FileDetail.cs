using System;

namespace NetBaires.Api.Services
{
    public class FileDetail
    {
        public string Name { get; }
        public Uri FileUri { get; }

        public FileDetail(string name, Uri fileUri)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            FileUri = fileUri;
        }
        public FileDetail(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}
